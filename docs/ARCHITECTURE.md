# AETHER SENTINEL AI Architecture

## Purpose

This document defines the foundational architecture for AETHER SENTINEL AI.

Phase 00 does not implement production source code. It establishes the architecture, module boundaries, and long-term engineering rules for future implementation.

## Product Definition

AETHER SENTINEL AI is an AI Performance Intelligence Agent.

It is designed to understand PC performance, identify bottlenecks, explain system issues, recommend safe optimizations, execute controlled optimization actions, and verify results.

## Architecture Principles

- Native performance first.
- Low memory usage.
- Low CPU usage.
- Native desktop experience.
- Long-term maintainability.
- Cross-platform core.
- Platform-specific adapters.
- Clear separation between analysis, execution, monitoring, and UI.

Avoid:

- Electron.
- WebView-first desktop architecture.
- Browser-runtime applications.
- Heavy frontend frameworks.
- Unsafe one-click optimization flows without explanation, backup, verification, and rollback.

## Technology Direction

- UI framework: Avalonia UI
- Core framework: .NET 8
- Primary target runtime: Windows 10 and Windows 11
- Future target runtime: macOS
- Development environment: macOS
- Developer tools: Codex, VS Code, JetBrains Rider, Git, GitHub

## Overall Architecture

```text
AETHER SENTINEL AI
        |
        v
Avalonia UI Layer
        |
        v
Core Intelligence Engine
        |
        v
Platform Adapter Layer
        |
        +-- Windows Adapter
        |
        +-- macOS Adapter
```

## Layer Responsibilities

### Avalonia UI Layer

Responsibilities:

- Present product navigation and user workflows.
- Display performance status, recommendations, monitoring data, and history.
- Trigger user-approved analysis and optimization actions.
- Render reusable components defined by the UI design system.

Rules:

- UI must not directly execute platform-specific optimization logic.
- UI must communicate with the core engine through stable application services.
- UI should remain responsive during scanning, monitoring, and optimization operations.

### Core Intelligence Engine

Responsibilities:

- Data models.
- Analysis logic.
- Configuration.
- Rule management.
- AI interface abstraction.
- Recommendation generation.
- Risk assessment.

Rules:

- Must not depend on operating-system-specific APIs.
- Must expose platform-neutral contracts.
- Must preserve explainability for analysis and recommendations.
- Must separate recommendation generation from optimization execution.

### Platform Adapter Layer

Responsibilities:

- Encapsulate platform-specific scanning, monitoring, and optimization capabilities.
- Provide Windows-specific and macOS-specific implementations behind stable interfaces.
- Protect the core engine from OS API differences.

Rules:

- Platform adapters may depend on OS-specific APIs.
- Platform adapters must return normalized data models.
- Platform adapters must report capability availability and permission requirements.

## Module Design

### Core Engine

Responsibilities:

- Define domain data models.
- Execute analysis logic.
- Manage configuration.
- Manage optimization rules.
- Define AI provider and model abstractions.
- Produce recommendations with risk and confidence metadata.

Required boundaries:

- No direct dependency on Windows, macOS, Linux, registry APIs, shell commands, WMI, or privileged services.
- No direct UI dependency.
- No irreversible system modification logic.

### Scanner Module

Responsibilities:

- Collect system information.
- Normalize platform-specific signals into unified models.
- Support future scanning for CPU, GPU, memory, storage, OS, processes, drivers, services, startup items, network state, DNS state, and game-related runtime state.

Output:

- Unified scan result model.
- Timestamped scan metadata.
- Capability and permission metadata.
- Confidence level for collected data.

### Optimization Engine

Responsibilities:

- Execute safe optimization actions.
- Validate preconditions.
- Store action plans.
- Enforce risk boundaries.
- Produce execution logs.

Every optimization action must support:

- Before: backup.
- During: execution log.
- After: verification.
- Rollback: restore previous state when supported.

Rules:

- No optimization action should run without a declared target, risk level, backup method, verification method, and rollback method.
- High-risk actions require explicit confirmation.
- Optimization recommendations and optimization execution must remain separate.

### Monitor Module

Responsibilities:

- Provide real-time and historical performance monitoring.
- Support future metrics including FPS, CPU, GPU, memory, temperature, latency, disk activity, network activity, and process behavior.

Rules:

- Monitoring must be lightweight.
- Sampling frequency must be configurable.
- Long-running monitoring must not degrade user performance.

### Network Intelligence Module

Responsibilities:

- Identify current DNS configuration.
- Identify network interface state.
- Estimate China mainland ISP and region when data is available.
- Benchmark DNS candidates.
- Run user-approved network speed tests.
- Measure latency, jitter, packet loss, download, and upload stability.

Rules:

- Must default to read-only detection.
- Must not change DNS settings without backup, explicit confirmation, verification, and rollback.
- Must not run full-bandwidth tests without user consent.
- Must support lightweight latency-only checks.
- Must keep provider registries replaceable so public, self-hosted, and region-specific nodes can be evaluated independently.

### AI Intelligence Layer

Responsibilities:

- Analyze system information.
- Explain performance issues in user-readable language.
- Generate optimization recommendations.
- Assess risk.
- Produce confidence scores and reasoning summaries.

Rules:

- AI output must be treated as advisory unless validated by deterministic rules.
- AI recommendations must include rationale and risk.
- AI layer must not directly execute optimization actions.

### UpdateManager

The architecture reserves an `UpdateManager` for future automatic updates.

Responsibilities:

- Version checking.
- Package verification.
- Download.
- Installation.
- Rollback.

Rules:

- Updates must be signed or otherwise verifiable.
- Failed updates must not leave the application in an unusable state.
- Rollback must be part of the update design.

## Future Product Extension

The architecture must allow future expansion into:

- AETHER SENTINEL: Performance intelligence.
- AETHER CREATOR: Creator workflow optimization.
- AETHER FLOW: Automation agent.
- AETHER VISION: AI visual intelligence.

Phase 00 does not implement these products. The architecture reserves namespace and module scalability only.

## Architectural Decision Records

Important decisions must be written into `docs/`.

Recommended future format:

```text
docs/decisions/ADR-0001-title.md
```

Each decision should include:

- Context.
- Decision.
- Alternatives considered.
- Consequences.
- Date.

## Reserved Workflow Locations

The repository reserves the following workflow files:

- `.github/workflows/build.yml`
- `.github/workflows/release.yml`
- `.github/workflows/update.yml`

Phase 00 reserves these locations only.
