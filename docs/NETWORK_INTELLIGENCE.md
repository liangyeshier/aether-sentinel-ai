# Network Intelligence Plan

## Purpose

Network Intelligence covers DNS Optimization, Network Speed Test, ISP detection, region detection, latency analysis, and network stability scoring.

The feature must help users understand network bottlenecks without silently changing DNS settings or consuming large amounts of traffic.

## Scope

Phase 03 introduces architecture and UI reservation for:

- Current DNS configuration detection.
- China mainland ISP and region identification.
- Network speed testing.
- Latency, jitter, packet loss, download, and upload metrics.
- DNS provider benchmarking.
- Safe recommendation generation.

Real DNS changes and full-bandwidth tests are not enabled by default.

## Open Source Direction

### IP Region And ISP Detection

Candidate:

- `lionsoul2014/ip2region`

Reason:

- Offline IP-to-region lookup.
- Supports IPv4 and IPv6.
- Provides bindings for multiple languages, including C#.
- Suitable for reducing dependency on a single public geolocation API.

Usage rule:

- Treat IP region data as an estimate, not an absolute identity.
- Record provider, database version, confidence, and lookup source.
- Allow replacement with other licensed databases if accuracy or licensing requires it.

### Speed Test Engine

Candidate:

- `librespeed/speedtest`

Reason:

- Open source speed test system.
- Supports download, upload, ping, jitter, IP address, ISP, and distance features.
- Supports self-hosted infrastructure, which is important for stable China mainland testing.

Usage rule:

- Prefer self-hosted or explicitly trusted test nodes.
- Full-bandwidth tests must require user confirmation because they consume traffic.
- A lightweight latency-only test must remain available.
- Server selection must consider region, ISP, latency, stability, and test-node availability.

## Product Rules

Network tests must follow:

```text
Detect -> Explain -> Ask Permission -> Test -> Compare -> Recommend
```

DNS changes must follow:

```text
Detect Current DNS -> Benchmark Candidates -> Backup -> Ask Permission -> Apply -> Verify -> Rollback If Needed
```

## Low-overhead Requirements

- Do not run network speed tests automatically in the background.
- Do not run full-bandwidth tests during games or streaming sessions.
- Default to latency-only checks when the user has not granted explicit permission.
- Cache provider and region results with a short local TTL.
- Avoid polling public APIs repeatedly.
- Keep network checks cancellable.

## China Mainland Adaptation

The system should support:

- China Telecom.
- China Unicom.
- China Mobile.
- China Broadnet.
- Education networks where detectable.
- Province and city-level region display when confidence is acceptable.

Accuracy strategy:

- First use local adapter data and local IP database.
- Then optionally compare with trusted public IP intelligence providers.
- Degrade gracefully when provider data is unavailable.
- Show confidence instead of pretending exactness.

## Provider Candidate Policy

DNS and speed test providers must be stored in a provider registry.

Each provider record should include:

- Name.
- Region.
- Endpoint.
- Official documentation URL.
- License or usage notes.
- Verification status.
- Last validation date.

Providers must not be recommended solely because they are popular. The final recommendation must come from measured latency, stability, failure rate, and user region.

## Security And Privacy

- Do not upload hardware details for network tests.
- Do not persist public IP history unless history logging is enabled.
- Do not include private endpoints, server credentials, analytics keys, or production telemetry in the open repository.
- Redact public IP data from shareable reports by default.
