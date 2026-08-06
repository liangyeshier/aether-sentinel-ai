using System.Diagnostics;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using AetherSentinel.Core.Network;
using AetherSentinel.Core.Scanning;

namespace AetherSentinel.Platforms.Network;

public sealed class LocalNetworkDiagnosticsProvider : INetworkDiagnosticsProvider
{
    public async ValueTask<NetworkDiagnosticsReport> RunQuickDiagnosticsAsync(
        NetworkDiagnosticsRequest request,
        CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var latencyResults = new List<NetworkLatencyResult>();
        foreach (var target in request.LatencyTargets)
        {
            latencyResults.Add(await MeasurePingAsync(target, request.SampleCount, request.Timeout, cancellationToken));
        }

        var dnsResults = new List<DnsBenchmarkResult>();
        foreach (var candidate in request.DnsCandidates.Where(candidate => candidate.OfficialEndpointConfirmed))
        {
            dnsResults.Add(await MeasureDnsAsync(candidate, request.DnsLookupDomain, request.SampleCount, request.Timeout, cancellationToken));
        }

        var bestLatency = latencyResults
            .Where(result => result.FailureRatePercent < 100)
            .OrderBy(result => result.AverageLatencyMs)
            .FirstOrDefault();

        var quality = bestLatency?.QualityLevel ?? NetworkQualityLevel.Unknown;
        var server = new SpeedTestServerCandidate(
            Name: bestLatency?.Name ?? "No reachable latency target",
            Endpoint: new Uri($"icmp://{bestLatency?.Host ?? "unavailable"}"),
            Region: bestLatency?.Region ?? "Unknown",
            Provider: bestLatency?.Provider ?? "AETHER",
            SelfHosted: false,
            Priority: 0);

        var speedResult = new NetworkSpeedTestResult(
            TestedAt: DateTimeOffset.Now,
            Server: server,
            IspRegion: new IspRegionInfo(
                Country: "Unknown",
                Region: "Unknown",
                City: "Unknown",
                Isp: "Unknown",
                Source: "Not collected in quick latency mode",
                Confidence: 0),
            DownloadMbps: 0,
            UploadMbps: 0,
            LatencyMs: bestLatency?.AverageLatencyMs ?? 0,
            JitterMs: bestLatency?.JitterMs ?? 0,
            PacketLossPercent: bestLatency?.FailureRatePercent ?? 100,
            QualityLevel: quality,
            Method: "QuickLatency: ICMP ping + DNS UDP lookup; no bandwidth download/upload");

        var summary = CreateSummary(latencyResults, dnsResults);

        return new NetworkDiagnosticsReport(
            TestedAt: DateTimeOffset.Now,
            LatencyResults: latencyResults,
            DnsBenchmarkResults: dnsResults,
            SpeedResult: speedResult,
            Summary: summary,
            ConsumedBandwidth: false);
    }

    private static async ValueTask<NetworkLatencyResult> MeasurePingAsync(
        NetworkLatencyTarget target,
        int sampleCount,
        TimeSpan timeout,
        CancellationToken cancellationToken)
    {
        var samples = new List<double>();
        var failures = 0;
        using var ping = new Ping();

        for (var index = 0; index < Math.Max(1, sampleCount); index++)
        {
            cancellationToken.ThrowIfCancellationRequested();

            try
            {
                var reply = await ping.SendPingAsync(
                    target.Host,
                    (int)Math.Clamp(timeout.TotalMilliseconds, 250, 5000));

                if (reply.Status == IPStatus.Success)
                {
                    samples.Add(reply.RoundtripTime);
                }
                else
                {
                    failures++;
                }
            }
            catch
            {
                failures++;
            }
        }

        return CreateLatencyResult(target, samples, failures, sampleCount);
    }

    private static async ValueTask<DnsBenchmarkResult> MeasureDnsAsync(
        DnsResolverCandidate resolver,
        string lookupDomain,
        int sampleCount,
        TimeSpan timeout,
        CancellationToken cancellationToken)
    {
        var samples = new List<double>();
        var attempts = 0;
        var failures = 0;

        foreach (var address in resolver.Addresses.Take(2))
        {
            for (var index = 0; index < Math.Max(1, sampleCount); index++)
            {
                attempts++;
                var elapsedMs = await TryResolveViaDnsServerAsync(address, lookupDomain, timeout, cancellationToken);
                if (elapsedMs > 0)
                {
                    samples.Add(elapsedMs);
                }
                else
                {
                    failures++;
                }
            }
        }

        var failureRate = attempts <= 0 ? 100 : Math.Round((double)failures / attempts * 100, 1);
        var average = samples.Count == 0 ? 0 : Math.Round(samples.Average(), 1);
        var jitter = CalculateJitter(samples);
        var recommendation = failureRate switch
        {
            >= 50 => DnsRecommendationLevel.Avoid,
            <= 10 when average is > 0 and <= 80 => DnsRecommendationLevel.Recommended,
            <= 25 => DnsRecommendationLevel.Candidate,
            _ => DnsRecommendationLevel.Unknown
        };

        return new DnsBenchmarkResult(
            Resolver: resolver,
            AverageLatencyMs: average,
            JitterMs: jitter,
            FailureRatePercent: failureRate,
            Recommendation: recommendation);
    }

    private static async ValueTask<double> TryResolveViaDnsServerAsync(
        string dnsServer,
        string lookupDomain,
        TimeSpan timeout,
        CancellationToken cancellationToken)
    {
        if (!IPAddress.TryParse(dnsServer, out var address))
        {
            return 0;
        }

        try
        {
            using var udp = new UdpClient(address.AddressFamily);
            udp.Connect(address, 53);

            var query = CreateDnsQuery(lookupDomain);
            var stopwatch = Stopwatch.StartNew();
            await udp.SendAsync(query, cancellationToken);
            var receiveTask = udp.ReceiveAsync(cancellationToken).AsTask();
            var completed = await Task.WhenAny(receiveTask, Task.Delay(timeout, cancellationToken));
            stopwatch.Stop();

            if (completed != receiveTask)
            {
                return 0;
            }

            var response = await receiveTask;
            return response.Buffer.Length >= 12 ? Math.Round(stopwatch.Elapsed.TotalMilliseconds, 1) : 0;
        }
        catch
        {
            return 0;
        }
    }

    private static byte[] CreateDnsQuery(string domain)
    {
        var random = Random.Shared.Next(0, ushort.MaxValue);
        var bytes = new List<byte>
        {
            (byte)(random >> 8),
            (byte)(random & 0xFF),
            0x01,
            0x00,
            0x00,
            0x01,
            0x00,
            0x00,
            0x00,
            0x00,
            0x00,
            0x00
        };

        foreach (var label in domain.Split('.', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries))
        {
            var labelBytes = System.Text.Encoding.ASCII.GetBytes(label);
            bytes.Add((byte)Math.Min(labelBytes.Length, 63));
            bytes.AddRange(labelBytes.Take(63));
        }

        bytes.Add(0x00);
        bytes.Add(0x00);
        bytes.Add(0x01);
        bytes.Add(0x00);
        bytes.Add(0x01);

        return bytes.ToArray();
    }

    private static NetworkLatencyResult CreateLatencyResult(
        NetworkLatencyTarget target,
        IReadOnlyList<double> samples,
        int failures,
        int expectedSamples)
    {
        var average = samples.Count == 0 ? 0 : Math.Round(samples.Average(), 1);
        var jitter = CalculateJitter(samples);
        var failureRate = expectedSamples <= 0 ? 100 : Math.Round((double)failures / expectedSamples * 100, 1);

        return new NetworkLatencyResult(
            Name: target.Name,
            Host: target.Host,
            Region: target.Region,
            Provider: target.Provider,
            AverageLatencyMs: average,
            JitterMs: jitter,
            FailureRatePercent: failureRate,
            QualityLevel: CalculateQuality(average, jitter, failureRate));
    }

    private static double CalculateJitter(IReadOnlyList<double> samples)
    {
        if (samples.Count <= 1)
        {
            return 0;
        }

        var deltas = samples
            .Zip(samples.Skip(1), (left, right) => Math.Abs(right - left))
            .ToArray();

        return Math.Round(deltas.Average(), 1);
    }

    private static NetworkQualityLevel CalculateQuality(double average, double jitter, double failureRate)
    {
        if (failureRate >= 50 || average <= 0)
        {
            return NetworkQualityLevel.Poor;
        }

        if (average <= 60 && jitter <= 20 && failureRate <= 10)
        {
            return NetworkQualityLevel.Good;
        }

        return NetworkQualityLevel.Watch;
    }

    private static string CreateSummary(
        IReadOnlyList<NetworkLatencyResult> latencyResults,
        IReadOnlyList<DnsBenchmarkResult> dnsResults)
    {
        var bestLatency = latencyResults
            .Where(result => result.FailureRatePercent < 100)
            .OrderBy(result => result.AverageLatencyMs)
            .FirstOrDefault();
        var bestDns = dnsResults
            .Where(result => result.FailureRatePercent < 100)
            .OrderBy(result => result.AverageLatencyMs)
            .FirstOrDefault();

        if (bestLatency is null && bestDns is null)
        {
            return "No reachable latency or DNS target was found in quick diagnostics.";
        }

        return $"Best latency: {bestLatency?.Name ?? "n/a"} {bestLatency?.AverageLatencyMs:0.0} ms; best DNS: {bestDns?.Resolver.Name ?? "n/a"} {bestDns?.AverageLatencyMs:0.0} ms.";
    }
}
