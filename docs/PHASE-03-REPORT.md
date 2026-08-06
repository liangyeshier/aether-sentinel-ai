# Phase 03 Progress Report

## Phase

Phase 03 - Read-only System Intelligence Foundation

## Status

In Progress

## Completed

- Added `AetherSentinel.Core` to the solution.
- Added `AetherSentinel.Platforms` to the solution.
- Added read-only system snapshot models.
- Added platform adapter boundary for future Windows and macOS implementations.
- Added first local read-only platform adapter.
- Connected the Scan button to real local read-only capture.
- Added live dashboard updates after scanning.
- Added live module cards for PC Intelligence, DNS Optimization, and Network Speed Test after scanning.
- Added performance budget policy for low-overhead defaults.
- Added network quality, ISP region, DNS candidate, and network speed test models.
- Added verified 360 Secure DNS provider registry metadata.
- Added DNS provider registry documentation.
- Added Network Speed Test to the left navigation.
- Added Simplified Chinese and English UI copy for Network Speed Test.
- Added Network Intelligence planning document.

## Changed Files

- `AetherSentinel.sln`
- `src/AetherSentinel.UI/AetherSentinel.UI.csproj`
- `src/AetherSentinel.UI/MainWindow.axaml`
- `src/AetherSentinel.UI/MainWindow.axaml.cs`
- `src/AetherSentinel.Core/AetherSentinel.Core.csproj`
- `src/AetherSentinel.Platforms/AetherSentinel.Platforms.csproj`
- `src/AetherSentinel.Platforms/Scanning/PlatformSystemScanner.cs`
- `src/AetherSentinel.Platforms/Scanning/LocalPlatformSystemAdapter.cs`
- `src/AetherSentinel.Core/Scanning/SystemSnapshot.cs`
- `src/AetherSentinel.Core/Scanning/ISystemScanner.cs`
- `src/AetherSentinel.Core/Scanning/IPlatformSystemAdapter.cs`
- `src/AetherSentinel.Core/Performance/PerformanceBudgetPolicy.cs`
- `src/AetherSentinel.Core/Network/NetworkSpeedTestModels.cs`
- `src/AetherSentinel.Core/Network/INetworkSpeedTestProvider.cs`
- `src/AetherSentinel.Core/Network/DnsOptimizationModels.cs`
- `src/AetherSentinel.Core/Network/NetworkProviderCatalog.cs`
- `docs/DNS_PROVIDER_REGISTRY.md`
- `docs/NETWORK_INTELLIGENCE.md`
- `docs/ARCHITECTURE.md`
- `docs/UI_DESIGN_SYSTEM.md`
- `docs/MILESTONES.md`
- `README.md`
- `ROADMAP.md`
- `CHANGELOG.md`
- `VERSION`

## Architecture Decisions

- Phase 03 starts with core contracts and data models before platform-specific implementation.
- Core must remain cross-platform and must not reference Avalonia, WMI, registry APIs, shell commands, `system_profiler`, or `networksetup`.
- Platform-specific command usage belongs in `AetherSentinel.Platforms`.
- The first local adapter may use macOS `sysctl`, `vm_stat`, and `networksetup` in read-only mode.
- macOS DNS detection may fall back to `scutil --dns` when service-level DNS is not configured.
- Network Speed Test is part of Network Intelligence and must be read-only until user-approved traffic tests are implemented.
- China mainland ISP and region detection should prefer replaceable offline data providers before any public API.
- Full-bandwidth speed tests must require explicit user consent because they consume traffic.
- 360 Secure DNS can be listed as a verified provider record, but recommendation and application still require local benchmark evidence and rollback support.

## Open Source Candidates

- `lionsoul2014/ip2region` for offline IP region and ISP lookup.
- `librespeed/speedtest` for self-hostable speed test infrastructure.

## Testing

Required verification:

- `dotnet build AetherSentinel.sln --no-restore`
- `docs/PHASE-03-1-MACOS-TEST-REPORT.md`

## Known Issues

- Windows adapter is not implemented yet.
- macOS adapter does not read GPU, startup items, or real CPU utilization yet.
- ISP and region detection is not implemented yet.
- Network speed testing is not implemented yet.
- 360 DNS is registered, but DNS switching is not implemented yet.
- Network Speed Test still displays read-only context and planning cards after scan.

## Risk

- Public IP location can be inaccurate; future UI must show confidence and data source.
- Full speed tests can consume bandwidth and affect games or streaming; they must be user-triggered and cancellable.
- DNS recommendations must be based on measured stability and region, not hardcoded provider preference.
