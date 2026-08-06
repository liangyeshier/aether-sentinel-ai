namespace AetherSentinel.Core.Network;

public sealed record DnsResolverCandidate(
    string Name,
    IReadOnlyList<string> Addresses,
    IReadOnlyList<DnsResolverProtocol> Protocols,
    string Region,
    IReadOnlyList<string> RecommendedIspTags,
    string Provider,
    bool OfficialEndpointConfirmed,
    Uri? OfficialDocumentationUrl,
    DateOnly VerifiedOn,
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

public enum DnsResolverProtocol
{
    PlainDns,
    DnsOverHttps,
    DnsOverTls
}
