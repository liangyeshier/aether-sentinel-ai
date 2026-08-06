using AetherSentinel.Core.Scanning;

namespace AetherSentinel.Core.Network;

public sealed record NetworkDiagnosticsRequest(
    int SampleCount,
    TimeSpan Timeout,
    IReadOnlyList<NetworkLatencyTarget> LatencyTargets,
    IReadOnlyList<DnsResolverCandidate> DnsCandidates,
    string DnsLookupDomain);

public sealed record NetworkLatencyTarget(
    string Name,
    string Host,
    string Region,
    string Provider);

public sealed record NetworkDiagnosticsReport(
    DateTimeOffset TestedAt,
    IReadOnlyList<NetworkLatencyResult> LatencyResults,
    IReadOnlyList<DnsBenchmarkResult> DnsBenchmarkResults,
    NetworkSpeedTestResult SpeedResult,
    string Summary,
    bool ConsumedBandwidth);

public sealed record NetworkLatencyResult(
    string Name,
    string Host,
    string Region,
    string Provider,
    double AverageLatencyMs,
    double JitterMs,
    double FailureRatePercent,
    NetworkQualityLevel QualityLevel);
