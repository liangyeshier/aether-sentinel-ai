namespace AetherSentinel.Core.Network;

public static class NetworkProviderCatalog
{
    public static IReadOnlyList<DnsResolverCandidate> DefaultDnsCandidates { get; } =
    [
        new(
            Name: "360 Secure DNS",
            Addresses: [],
            Region: "China Mainland",
            Provider: "360",
            OfficialEndpointConfirmed: false,
            UsageNotes: "Candidate only until official endpoint and usage terms are documented in the provider registry."),
        new(
            Name: "AliDNS",
            Addresses: [],
            Region: "China Mainland",
            Provider: "Alibaba Cloud",
            OfficialEndpointConfirmed: false,
            UsageNotes: "Candidate for future latency and reliability comparison."),
        new(
            Name: "DNSPod Public DNS",
            Addresses: [],
            Region: "China Mainland",
            Provider: "Tencent DNSPod",
            OfficialEndpointConfirmed: false,
            UsageNotes: "Candidate for future latency and reliability comparison."),
        new(
            Name: "Cloudflare DNS",
            Addresses: [],
            Region: "Global",
            Provider: "Cloudflare",
            OfficialEndpointConfirmed: false,
            UsageNotes: "Global fallback candidate; China mainland latency must be measured before recommendation.")
    ];
}
