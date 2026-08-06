# Changelog

All notable changes to AETHER SENTINEL AI will be documented in this file.

This project follows phase-based development. Each completed phase must include a corresponding report in `docs/`.

## [0.12.0] - Phase 12 AETHER Glass And YuqiEngine Review

### Added

- AETHER Glass visual material direction for the Avalonia shell.
- YuqiEngine independent feature absorption review.
- Phase 12 completion report.

### Changed

- The main desktop shell now uses layered dark glass surfaces, refined panel borders, and gradient action materials while preserving 8 px radius and low-overhead rendering.
- Runtime visual direction now explicitly avoids WebView-based dependency growth and keeps Windows native polish as the target.

## [0.11.0] - Phase 11 Feature Loop Completion

### Added

- Dashboard AI Optimize full safety loop.
- Dashboard Analyze and View Report actions.
- PC Intelligence action panel.
- DNS Optimization benchmark and switch-preview action panel.
- Toolkit Center run-checks action panel.
- Settings local save and data-folder action panel.
- History JSON export action.
- Local settings file model.
- Phase 11 completion report.

### Changed

- Every left navigation page now exposes at least one executable local action or result-refresh path.
- AI Optimize now chains read-only scan, quick network diagnostics, game session analysis, Game Boost preview, optimization Dry Run, safe execution simulation, Advisor report generation, and redacted history persistence.
- Runtime artifacts are ignored by Git.

## [0.10.0] - Phase 10 AI Advisor And History

### Added

- Advisor report model.
- Advisor finding and recommendation models.
- Advisor history record model.
- Local Advisor report generator.
- AI Advisor page report action.
- Local redacted history summaries and History page cards.
- Phase 10 completion report.

### Changed

- AI Advisor now generates local template-based reports from scan, network, game, monitor, GPU, Dry Run, and execution simulation data.

## [0.9.0] - Phase 09 GPU And Driver Intelligence

### Added

- GPU intelligence report model.
- GPU vendor detection.
- GPU telemetry availability model.
- GPU insight model.
- GPU intelligence analyzer.
- PC Intelligence GPU intelligence live row.
- Phase 09 completion report.

### Changed

- GPU driver write actions are explicitly disabled by policy until Windows hardware validation exists.

## [0.8.0] - Phase 08 Windows Toolkit Center

### Added

- Toolkit item model.
- Toolkit catalog.
- Toolkit Center navigation item.
- Toolkit Center module page.
- Purpose, risk, availability, and revert path metadata for toolkit items.
- Phase 08 completion report.

### Changed

- The app now exposes a Windows-first Toolkit Center without enabling unsafe tool execution.

## [0.7.0] - Phase 07 Game Boost Mode

### Added

- Game Boost plan model.
- Game Boost action preview model.
- Game Boost planner.
- Balanced Game Boost plan generation.
- Game Optimization Boost Plan button and result cards.
- Phase 07 completion report.

### Changed

- Game Optimization can now generate a safe Game Boost preview plan while keeping real execution disabled.

## [0.6.0] - Phase 06 Safe Optimization Execution

### Added

- Optimization execution request and report models.
- Execution result and restore point models.
- Guarded optimization execution engine.
- Optimization Center safe simulation action and result cards.
- Phase 06 completion report.

### Changed

- Optimization Center can now simulate safe execution from a Dry Run report while keeping real system writes disabled.

## [0.5.0] - Phase 05 Optimization Rule Engine Dry Run

### Added

- Optimization rule model.
- Default optimization rule catalog.
- Dry-run report and preview models.
- Optimization dry-run engine.
- Optimization Center action button and live preview cards.
- Phase 05 completion report.

### Changed

- Optimization Center now previews safe rules without executing system changes.

## [0.4.0] - Phase 04 Low-overhead Performance Monitor

### Added

- Monitor request and snapshot models.
- Low-overhead monitor interface.
- Local single-sample monitor provider.
- AETHER self CPU and memory overhead measurement.
- Process count and top memory pressure sampling.
- Performance Monitor page action button and live result cards.
- Phase 04 completion report.

### Changed

- Performance Monitor now performs user-triggered sampling instead of remaining a static reservation page.

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
