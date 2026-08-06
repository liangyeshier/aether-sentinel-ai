# DNS Provider Registry

## Purpose

This registry tracks DNS providers that AETHER SENTINEL AI can evaluate for DNS Optimization.

Provider records must separate:

- Officially verified endpoints.
- Unverified candidates.
- Measured local performance.
- User-approved system changes.

Being listed in the registry does not mean the provider should be applied automatically.

## Safety Rules

- Never change DNS settings without user confirmation.
- Always back up original DNS configuration before applying changes.
- Always support rollback.
- Do not recommend a DNS provider only because it is listed here.
- Final recommendation must consider latency, jitter, failure rate, ISP, region, privacy, and user intent.

## Verified Providers

### 360 Secure DNS

Provider:

360

Status:

Verified provider record

Region:

China Mainland

Official documentation:

`https://sdns.360.net/dnsPublic.html`

Verified on:

2026-08-06

Plain DNS IPv4:

```text
101.226.4.6
218.30.118.6
```

Supported protocols recorded for future evaluation:

- Plain DNS
- DNS over HTTPS
- DNS over TLS

Recommended ISP tags:

- China Telecom
- China Mobile
- China Tietong
- China Unicom

Implementation status:

- Provider registry: implemented.
- UI display: implemented.
- Latency benchmark: not implemented.
- DNS apply action: not implemented.
- Backup and rollback: not implemented.

## Candidate Providers

The following providers are candidates only. Their official endpoints must be verified before being used in recommendations:

- AliDNS.
- DNSPod Public DNS.
- Cloudflare DNS.

## Future Provider Record Fields

Each provider should track:

- Name.
- Provider.
- Region.
- Endpoint addresses.
- Supported protocols.
- Official documentation URL.
- License or usage notes.
- Verification date.
- Recommended ISP tags.
- Last measured latency.
- Last measured jitter.
- Failure rate.
- Recommendation level.
