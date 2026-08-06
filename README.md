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
Phase 00 - Product Foundation
```

Phase 00 establishes the product foundation only. It does not include production feature implementation.

## Technical Direction

- UI framework: Avalonia UI
- Core framework: .NET 8
- Architecture principle: Cross-platform core plus platform-specific adapters
- Primary target runtime: Windows 10 and Windows 11
- Future runtime target: macOS

## Run The Prototype

Phase 02 includes a runnable Avalonia desktop shell with clickable module pages and simulated, read-only capability previews.

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
- Read-only module previews for PC intelligence, game optimization, monitoring, optimization center, AI advisor, history, and settings.

No real system scanning or optimization is executed in this prototype.

## Documentation

Important product, engineering, and design decisions are maintained in `docs/`.

Core documents:

- `docs/ARCHITECTURE.md`
- `docs/BRAND_GUIDELINE.md`
- `docs/UI_DESIGN_SYSTEM.md`
- `docs/GITHUB_WORKFLOW.md`
- `docs/MILESTONES.md`
- `docs/PHASE-00-REPORT.md`
- `docs/PHASE-01-REPORT.md`
- `docs/PHASE-02-REPORT.md`

## Repository Governance

This repository uses structured documentation, conventional commit rules, issue templates, milestone planning, and phase completion reports to support long-term maintainability.

## Open Source Boundary

This project may be developed openly where practical.

Private server endpoints, API keys, signing certificates, update credentials, personal tokens, and production infrastructure settings must not be committed. Public examples belong in `.env.example`; real local values belong in ignored environment files.
