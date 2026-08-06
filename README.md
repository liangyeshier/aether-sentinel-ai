# AETHER SENTINEL AI

AETHER SENTINEL AI is an AI Performance Intelligence Agent created by Anson under AETHER AGENTIC Studio.

The product is designed as an intelligent desktop guardian that understands computer performance, identifies bottlenecks, explains problems, recommends safe optimizations, and verifies outcomes.

## Studio

AETHER AGENTIC Studio focuses on building AI-native software agents that improve human productivity, creativity, and digital experiences.

## Product Positioning

AETHER SENTINEL AI is not a simple FPS booster.

Traditional optimization software usually follows:

```text
Click button -> Apply tweaks
```

AETHER SENTINEL AI follows:

```text
Understand -> Analyze -> Explain -> Recommend -> Optimize -> Verify
```

## Phase

Current phase:

```text
Phase 10 - AI Advisor And History
```

The current prototype includes the product foundation, runnable desktop shell, clickable module previews, reserved DNS Optimization, Network Speed Test planning, core read-only intelligence contracts, first local read-only scanning, and a Windows-first upgrade roadmap aligned against BoosterX and Pavise. Optimization execution is still disabled.

## Technical Direction

- UI framework: Avalonia UI
- Core framework: .NET 8
- Architecture principle: Cross-platform core plus platform-specific adapters
- Primary target runtime: Windows 10 and Windows 11
- Future runtime target: macOS

## Run The Prototype

Phase 03 includes a runnable Avalonia desktop shell with clickable module pages, read-only capability previews, the first cross-platform core contracts, and local read-only scanning.

Requirements:

- .NET 8 SDK

Run:

```bash
dotnet restore AetherSentinel.sln
dotnet run --project src/AetherSentinel.UI/AetherSentinel.UI.csproj
```

Current prototype scope:

- Desktop shell.
- BoosterX-inspired dark performance dashboard.
- Clickable navigation modules.
- Simulated performance metrics.
- Simulated AI optimization queue.
- Read-only module previews for PC intelligence, game optimization, monitoring, optimization center, DNS optimization, network speed testing, AI advisor, history, and settings.
- Core models for read-only system snapshots, network quality, DNS candidates, and speed test results.
- Local read-only scan button for OS, memory, storage, process, network interface, and DNS data.
- Read-only performance score, optimization potential, and recommendation generation.
- Verified 360 Secure DNS provider registry entry.
- Competitive upgrade roadmap covering Network Intelligence, Game Session Intelligence, low-overhead monitoring, safe rule dry-runs, reversible optimization execution, Game Boost Mode, and toolkit modules.
- Windows-first implementation strategy with macOS kept as the current development and validation environment.
- User-triggered quick Network Speed Test with Ping/Jitter and DNS benchmark results.
- Expanded Windows-first read-only snapshot fields for startup items, active power plan, GPU name, and game process candidates.
- Local game library and read-only game session detection controls.
- User-triggered low-overhead performance monitor sample.
- Optimization Center Dry Run rule preview.
- Guarded safe optimization execution simulation with restore point records.
- Game Boost Mode preview plan generation.
- Windows Toolkit Center catalog.
- GPU vendor and driver-write safety intelligence.
- Local AI Advisor report generation and redacted history summaries.

No optimization is executed in this prototype.

## Documentation

Important product, engineering, and design decisions are maintained in `docs/`.

Core documents:

- `docs/ARCHITECTURE.md`
- `docs/BRAND_GUIDELINE.md`
- `docs/UI_DESIGN_SYSTEM.md`
- `docs/GITHUB_WORKFLOW.md`
- `docs/MILESTONES.md`
- `docs/DNS_PROVIDER_REGISTRY.md`
- `docs/NETWORK_INTELLIGENCE.md`
- `docs/COMPETITIVE_FEATURE_ROADMAP.md`
- `docs/WINDOWS_FIRST_UPGRADE_ROADMAP.md`
- `docs/PHASE-00-REPORT.md`
- `docs/PHASE-01-REPORT.md`
- `docs/PHASE-02-REPORT.md`
- `docs/PHASE-02-1-REPORT.md`
- `docs/PHASE-03-REPORT.md`
- `docs/PHASE-03-1-MACOS-TEST-REPORT.md`
- `docs/PHASE-03-2-ANALYSIS-REPORT.md`
- `docs/PHASE-03-3-COMPETITIVE-ROADMAP-REPORT.md`
- `docs/PHASE-03-4-NETWORK-INTELLIGENCE-REPORT.md`
- `docs/PHASE-03-5-WINDOWS-READONLY-ADAPTER-REPORT.md`
- `docs/PHASE-03-6-GAME-SESSION-REPORT.md`
- `docs/PHASE-04-LOW-OVERHEAD-MONITOR-REPORT.md`
- `docs/PHASE-05-OPTIMIZATION-DRY-RUN-REPORT.md`
- `docs/PHASE-06-SAFE-OPTIMIZATION-EXECUTION-REPORT.md`
- `docs/PHASE-07-GAME-BOOST-MODE-REPORT.md`
- `docs/PHASE-08-WINDOWS-TOOLKIT-CENTER-REPORT.md`
- `docs/PHASE-09-GPU-DRIVER-INTELLIGENCE-REPORT.md`
- `docs/PHASE-10-AI-ADVISOR-HISTORY-REPORT.md`

## Repository Governance

This repository uses structured documentation, conventional commit rules, issue templates, milestone planning, and phase completion reports to support long-term maintainability.

## Open Source Boundary

This project may be developed openly where practical.

Private server endpoints, API keys, signing certificates, update credentials, personal tokens, and production infrastructure settings must not be committed. Public examples belong in `.env.example`; real local values belong in ignored environment files.
