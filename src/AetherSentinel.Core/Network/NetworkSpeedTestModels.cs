using AetherSentinel.Core.Scanning;

namespace AetherSentinel.Core.Network;

public sealed record NetworkSpeedTestRequest(
    SpeedTestMode Mode,
    IReadOnlyList<SpeedTestServerCandidate> CandidateServers,
    TimeSpan Timeout,
    bool IdentifyChinaMainlandIsp,
    bool RequireUserConsentForTraffic);

public sealed record SpeedTestServerCandidate(
    string Name,
    Uri Endpoint,
    string Region,
    string Provider,
    bool SelfHosted,
    int Priority);

public sealed record NetworkSpeedTestResult(
    DateTimeOffset TestedAt,
    SpeedTestServerCandidate Server,
    IspRegionInfo IspRegion,
    double DownloadMbps,
    double UploadMbps,
    double LatencyMs,
    double JitterMs,
    double PacketLossPercent,
    NetworkQualityLevel QualityLevel,
    string Method);

public enum SpeedTestMode
{
    QuickLatency,
    Balanced,
    FullBandwidth
}
