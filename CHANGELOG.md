# Changelog

All notable changes to AETHER SENTINEL AI will be documented in this file.

This project follows phase-based development. Each completed phase must include a corresponding report in `docs/`.

## [0.3.10] - Phase 03.6 Game Library And Session Detection

### Added

- Game library entry model.
- Game session analysis model.
- Game-session safety boundary model.
- Read-only game session analyzer.
- Local game library persistence.
- Game Optimization page controls for adding games and detecting sessions.
- Phase 03.6 completion report.

### Changed

- Game Optimization now distinguishes library matches, game candidates, launcher candidates, and needs-confirmation states.

## [0.3.9] - Phase 03.5 Windows Read-only Adapter Expansion

### Added

- Startup item snapshot model.
- Active power plan snapshot model.
- Game process candidate snapshot model.
- Windows CPU, GPU, physical memory, startup item, and power plan read paths.
- Game-related process candidate detection.
- PC Intelligence and Game Optimization live rows for the new read-only fields.
- Phase 03.5 completion report.

### Changed

- System snapshots now carry Windows-first optimization context while preserving macOS fallback behavior.

## [0.3.8] - Phase 03.4 Network Intelligence Activation

### Added

- Quick Network Speed Test execution from the Network Speed Test module.
- Local network diagnostics provider.
- Ping/Jitter latency measurement.
- DNS UDP lookup benchmark measurement.
- 360 Secure DNS benchmark path.
- Live network diagnostics result cards.
- Phase 03.4 completion report.

### Changed

- Network Speed Test now has a visible action button.
- DNS Optimization can display benchmark results after a quick network test.

## [0.3.7] - Windows-first Upgrade Roadmap

### Added

- Windows-first upgrade roadmap focused on BoosterX commercial alignment and Pavise free/open feature direction.
- Platform strategy that treats Windows as the primary optimization target and macOS as the current development and validation environment.
- Pavise reuse boundary for free/open modules, license preservation, provenance tracking, and future commercial separation.

### Changed

- LaoYing Toolkit is removed from active planning until an official source repository and compatible license are confirmed.
- README and roadmap now reference the Windows-first upgrade strategy.

## [0.3.6] - Competitive Feature Roadmap

### Added

- Competitive feature roadmap combining BoosterX, Pavise, and LaoYing-style utility directions.
- License and reuse boundaries for public-source and public-reference products.
- Phase 03.4 through Phase 10 upgrade plan for network intelligence, game session intelligence, monitoring, dry-run rules, safe execution, game boost mode, toolkit modules, AI history, and update infrastructure.
- Roadmap alignment matrices for BoosterX, Pavise, and LaoYing-style capabilities.

### Changed

- README and main roadmap now reference the competitive synthesis roadmap.

## [0.3.5] - Phase 03.2 Read-only Performance Scoring

### Added

- Core performance analysis report model.
- Read-only performance analyzer.
- Overall score calculation from memory, storage, process, DNS, and network factors.
- Optimization potential classification.
- Read-only recommendation generation with risk, verification, and rollback metadata.
- Dashboard score updates from the latest scan analysis.
- Live analysis cards for Optimization Center and AI Advisor.

### Changed

- Optimization Queue now displays analysis recommendations after a read-only scan.

## [0.3.4] - macOS Read-only Scan Validation

### Added

- macOS read-only scan validation report.
- Windows inferred test matrix for current Phase 03.1 scanner behavior.

### Changed

- macOS DNS detection now falls back from `networksetup` service DNS to `scutil --dns` resolver DNS.

## [0.3.3] - Phase 03.1 Local Read-only Scan

### Added

- `AetherSentinel.Platforms` project for platform-specific adapters.
- Local read-only system adapter.
- Scan button integration with real local OS, memory, storage, process, network interface, and DNS reads.
- Live dashboard updates after a read-only scan.
- Live module cards for PC Intelligence, DNS Optimization, and Network Speed Test after scanning.

### Changed

- UI now distinguishes real read-only data from pending CPU/GPU/network speed adapters.

## [0.3.2] - Network Speed Navigation Visibility

### Changed

- Left navigation now scrolls when modules exceed the available sidebar height.
- Right-side phase preview now reflects Phase 03 Network Intelligence.

## [0.3.1] - 360 DNS Provider Registry

### Added

- Verified 360 Secure DNS provider record.
- DNS provider registry documentation.
- DNS resolver protocol metadata for Plain DNS, DNS over HTTPS, and DNS over TLS.
- Official documentation URL, verification date, and recommended ISP tags for DNS provider records.

### Changed

- DNS Optimization page now describes 360 Secure DNS as a verified provider registry entry.
- Unverified DNS providers no longer use placeholder endpoint addresses.

## [0.3.0] - Phase 03 Read-only System Intelligence Foundation

### Added

- `AetherSentinel.Core` project.
- Read-only system snapshot models.
- Platform system adapter boundary.
- Network quality, ISP region, DNS candidate, and speed test models.
- Network Speed Test left navigation module.
- Simplified Chinese and English Network Speed Test page copy.
- Network Intelligence planning document.

### Changed

- README and roadmap now describe Phase 03 as in progress.
- Architecture now includes a dedicated Network Intelligence module.

## [0.2.1] - Phase 02.1 DNS Optimization Reservation

### Added

- DNS Optimization left navigation module.
- Simplified Chinese and English DNS Optimization page copy.
- Read-only DNS capability cards for current DNS detection, latency benchmarking, secure DNS candidates, and backup/rollback requirements.

### Changed

- Roadmap now recognizes DNS Optimization as a dedicated product capability candidate.

## [0.0.0] - Phase 00 Foundation

### Added

- Product foundation documentation.
- Brand guideline.
- UI design system.
- Software architecture documentation.
- GitHub workflow documentation.
- Issue templates.
- Milestone documentation.
- Reserved CI, release, and update workflow locations.

## [0.1.0] - Phase 01 Desktop Shell

### Added

- Avalonia UI desktop project.
- Runnable dashboard prototype.
- BoosterX-inspired dark performance layout.
- Static navigation structure.
- Simulated system metric cards.
- Simulated AI Performance Core panel.
- Simulated optimization queue and Sentinel Feed.

### Changed

- Added Simplified Chinese and English UI switching.
- Set Simplified Chinese as the default prototype language.
- Reduced the AI Performance Core ring size and stroke weight.

## [0.2.0] - Phase 02 Module Activation

### Added

- Clickable left navigation.
- Module pages for PC Intelligence, Game Optimization, Performance Monitor, Optimization Center, AI Advisor, History, and Settings.
- Read-only capability cards for each module.
- Low-overhead planning notes in Performance Monitor and Settings pages.

### Changed

- Updated right-side phase preview copy to Phase 02.
- Preserved dashboard as the default landing page.
