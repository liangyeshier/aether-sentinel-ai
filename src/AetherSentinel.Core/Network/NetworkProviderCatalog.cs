namespace AetherSentinel.Core.Network;

public static class NetworkProviderCatalog
{
    public static IReadOnlyList<DnsResolverCandidate> DefaultDnsCandidates { get; } =
    [
        new(
            Name: "360 Secure DNS",
            Addresses: ["101.226.4.6", "218.30.118.6"],
            Protocols: [DnsResolverProtocol.PlainDns, DnsResolverProtocol.DnsOverHttps, DnsResolverProtocol.DnsOverTls],
            Region: "China Mainland",
            RecommendedIspTags: ["China Telecom", "China Mobile", "China Tietong", "China Unicom"],
            Provider: "360",
            OfficialEndpointConfirmed: true,
            OfficialDocumentationUrl: new Uri("https://sdns.360.net/dnsPublic.html"),
            VerifiedOn: new DateOnly(2026, 8, 6),
            UsageNotes: "Official public DNS candidate. Future recommendation still requires local latency, jitter, failure-rate, and rollback checks."),
        new(
            Name: "AliDNS",
            Addresses: [],
            Protocols: [DnsResolverProtocol.PlainDns],
            Region: "China Mainland",
            RecommendedIspTags: [],
            Provider: "Alibaba Cloud",
            OfficialEndpointConfirmed: false,
            OfficialDocumentationUrl: null,
            VerifiedOn: DateOnly.MinValue,
            UsageNotes: "Candidate for future latency and reliability comparison."),
        new(
            Name: "DNSPod Public DNS",
            Addresses: [],
            Protocols: [DnsResolverProtocol.PlainDns],
            Region: "China Mainland",
            RecommendedIspTags: [],
            Provider: "Tencent DNSPod",
            OfficialEndpointConfirmed: false,
            OfficialDocumentationUrl: null,
            VerifiedOn: DateOnly.MinValue,
            UsageNotes: "Candidate for future latency and reliability comparison."),
        new(
            Name: "Cloudflare DNS",
            Addresses: [],
            Protocols: [DnsResolverProtocol.PlainDns],
            Region: "Global",
            RecommendedIspTags: [],
            Provider: "Cloudflare",
            OfficialEndpointConfirmed: false,
            OfficialDocumentationUrl: null,
            VerifiedOn: DateOnly.MinValue,
            UsageNotes: "Global fallback candidate; China mainland latency must be measured before recommendation.")
    ];
}
