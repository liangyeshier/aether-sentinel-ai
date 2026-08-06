namespace AetherSentinel.Core.Network;

public sealed record DnsResolverCandidate(
    string Name,
    IReadOnlyList<string> Addresses,
    string Region,
    string Provider,
    bool OfficialEndpointConfirmed,
    string UsageNotes);

public sealed record DnsBenchmarkResult(
    DnsResolverCandidate Resolver,
    double AverageLatencyMs,
    double JitterMs,
    double FailureRatePercent,
    DnsRecommendationLevel Recommendation);

public enum DnsRecommendationLevel
{
    Unknown,
    Candidate,
    Recommended,
    Avoid
}
