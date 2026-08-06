# Windows-first Upgrade Roadmap

## Purpose

This document defines the main upgrade path for AETHER SENTINEL AI after the Phase 03 foundation work.

The product direction is:

```text
Windows-first performance intelligence and game optimization.
macOS-supported development, UI validation, core logic validation, and read-only testing.
```

## Product Alignment

### BoosterX

Role:

Commercial benchmark.

Use:

- Product experience reference.
- User workflow reference.
- Optimization category reference.
- Dashboard and decision-flow reference.

Do not use:

- Branding.
- Copywriting.
- Private implementation.
- Proprietary UI assets.
- Any behavior inferred from private internals.

Target AETHER alignment:

- Scan first.
- Explain bottlenecks.
- Show boost potential.
- Show privacy and optimization level.
- Review before applying.
- Backup before changes.
- Verify after changes.
- Restore when needed.

### Pavise

Role:

Primary open-source/free-function reference for Windows game performance features.

Source:

`https://github.com/dulaiduwang003/Pavise-Game`

License:

Custom Pavise License.

Practical interpretation for AETHER:

- Pavise-related feature code can be studied and, where appropriate, reused only for free and open distribution.
- Any reused or derivative Pavise code must preserve license text, copyright notices, author information, and modification notices.
- Any reused or derivative Pavise code must not be sold.
- AETHER must not use the Pavise name, icon, or visual identity.
- Future paid/proprietary AETHER modules must not depend on copied Pavise code unless written permission or a compatible legal path exists.

Engineering policy:

- Prefer independent reimplementation of concepts when the feature may later become part of a commercial AETHER product.
- If direct code reuse is chosen for a free-only module, isolate it in a clearly marked compatibility area and document provenance.
- Keep copied/derived Pavise work separated from private update infrastructure, cloud services, and paid features.

### LaoYing Toolkit

Role:

Removed from the active roadmap.

Reason:

The current plan no longer depends on LaoYing Toolkit because its authoritative source and license are not confirmed.

## Platform Strategy

### Windows

Windows is the primary runtime and feature target.

Windows receives:

- Real hardware and OS scanning.
- Process and startup analysis.
- DNS benchmarking and DNS apply/rollback.
- Game detection.
- Game-session optimization.
- Background pressure control.
- Power plan control.
- Service review.
- Driver/GPU setting inspection where safe.
- Full release validation.

### macOS

macOS is a development and validation platform for now.

macOS receives:

- Avalonia UI validation.
- Core model validation.
- Read-only system scan validation where APIs are available.
- Network speed and DNS benchmark validation.
- Documentation and architecture testing.
- Build verification.

macOS does not define the first production optimization target.

## Core Product Principle

Every feature should follow:

```text
Detect -> Analyze -> Explain -> Preview -> Backup -> Apply -> Verify -> Report -> Rollback
```

No write action may bypass:

- User confirmation.
- Risk explanation.
- Backup plan.
- Verification signal.
- Rollback path.

## Phase Plan

### Phase 03.4 - Network Intelligence Activation

Goal:

Make Network Speed Test and DNS Optimization visible and useful.

Windows priority:

- Detect current DNS.
- Detect active network adapter.
- Detect public IP.
- Detect China mainland ISP and region.
- Run latency-only test.
- Run user-approved full speed test.
- Benchmark current DNS against 360 Secure DNS and other verified DNS providers.
- Score DNS candidates by latency, jitter, failure rate, ISP, and region.

macOS validation:

- Confirm UI flow.
- Confirm provider registry logic.
- Confirm latency-only and network model behavior.
- Avoid system DNS write actions.

Deliverables:

- Network Speed Test button.
- Network Speed Test page with results.
- ISP and region display with confidence.
- DNS candidate benchmark.
- Recommendation result without applying changes.

### Phase 03.5 - Windows Read-only Adapter Expansion

Goal:

Collect Windows-specific system data before optimization.

Features:

- CPU model, core count, logical processor count.
- Memory pressure.
- Disk usage and basic disk activity.
- GPU name and vendor where safely available.
- Windows version and build.
- Running processes.
- Startup entries.
- Active power plan.
- Network adapters.
- DNS servers.
- Basic game process candidates.

Deliverables:

- Windows adapter implementation.
- macOS fallback kept working.
- Scan result parity table.

### Phase 03.6 - Game Library And Session Detection

Goal:

Recognize games safely before attempting to optimize them.

Features:

- Add game manually by EXE.
- Add game by shortcut.
- Scan common launchers and libraries.
- Distinguish launcher, updater, crash reporter, anti-cheat, and game body.
- Track game session start, foreground state, fullscreen state, and exit.
- Keep session alive when user switches to desktop.

Windows priority:

- Steam.
- Epic Games.
- Battle.net.
- Riot.
- WeGame.
- Xbox/Microsoft Store where safe.

Safety boundary:

- No injection.
- No memory modification.
- No game file modification.
- No anti-cheat bypass.

### Phase 04 - Low-overhead Performance Monitor

Goal:

Explain what is consuming system resources without becoming a resource problem itself.

Features:

- CPU usage.
- Memory pressure.
- Disk queue or I/O pressure.
- Network latency and throughput.
- Top background pressure processes.
- App self-overhead display.
- Sampling mode: off, light, active.
- Auto-throttle when hidden.

Windows priority:

- Native API sampling.
- Event-based or low-frequency sampling where possible.
- No always-on heavy polling.

macOS validation:

- UI and core sampling budget behavior.
- Basic CPU, memory, disk, network read-only data.

### Phase 05 - Optimization Rule Engine Dry Run

Goal:

Represent every optimization as a rule before any real system change.

Rule fields:

- Rule ID.
- Name.
- Category.
- Target platform.
- Required privilege.
- Risk level.
- Detection condition.
- Preview text.
- Backup method.
- Apply method.
- Verification method.
- Rollback method.
- User consent requirement.

Initial categories:

- DNS.
- Startup items.
- Power plan.
- Background pressure.
- Notifications.
- Game focus.
- Temporary cleanup.
- Services.
- Privacy telemetry.

Deliverables:

- Rule schema.
- Rule catalog.
- Dry-run page.
- Change preview.
- Risk badges.

### Phase 06 - Safe Optimization Execution

Goal:

Enable only low-risk, reversible optimizations first.

First executable rules:

- DNS switch with backup and rollback.
- Startup item disable and restore.
- Temporary cleanup with preview.
- Power plan switch with original plan snapshot.
- Game focus notification settings where reversible.

Blocked rules:

- Aggressive service disabling.
- Driver setting writes.
- Anti-cheat-related changes.
- Kernel isolation.
- Undocumented registry changes.

Deliverables:

- Apply flow.
- Restore center.
- Action log.
- Verification result.
- Rollback button.

### Phase 07 - Game Boost Mode

Goal:

Build a safe game-session optimization mode inspired by Pavise while preserving AETHER's explainable workflow.

Features:

- Game session protection profile.
- Background pressure preview.
- User whitelist.
- System protected list.
- Anti-cheat protected list.
- Launcher cleanup recommendation.
- Process priority policy.
- I/O priority policy where safe.
- CPU set or affinity recommendation where safe.
- Power plan during game session.
- Notification/game focus during session.
- Automatic restore when game exits.
- Crash recovery restore on next launch.
- Session report.

Modes:

- Balanced.
- Competitive.
- Custom.

Execution rule:

Balanced must be conservative. Competitive may be stronger but must explain compatibility risk before applying.

### Phase 08 - Windows Toolkit Center

Goal:

Provide practical Windows optimization tools without cluttering the product.

Modules:

- Startup Manager.
- Service Review.
- Power Plan Center.
- DNS Center.
- Network Test Center.
- Storage Cleanup.
- Memory Pressure Inspector.
- GPU Inspector.
- Restore Center.
- System Shortcuts.

Design rule:

Each tool must show:

- What it does.
- Why it matters.
- Risk level.
- Original value.
- Revert path.

### Phase 09 - GPU And Driver Intelligence

Goal:

Inspect and explain GPU-side constraints before writing driver settings.

Features:

- NVIDIA/AMD vendor detection.
- Driver version detection.
- GPU load and temperature where available.
- Power limit and thermal limit explanation where supported.
- Graphics preset review.
- DLSS-related capability notes where safely detectable.

Write actions:

Driver setting writes are not enabled until official APIs, hardware validation, and rollback behavior are confirmed.

### Phase 10 - AI Advisor And History

Goal:

Make the product explain results like an intelligent performance assistant.

Features:

- Bottleneck explanation.
- Optimization recommendation.
- Risk assessment.
- Before/after comparison.
- Game session report.
- Network report.
- Historical score trend.
- Exportable local report.
- Privacy redaction.

### Phase 11 - Release, Update, And Open-source Boundary

Goal:

Prepare public releases without exposing private infrastructure.

Features:

- GitHub Actions build.
- Windows package build.
- Code signing plan.
- Update manifest.
- Package verification.
- Update rollback.
- Public example configuration.
- Private endpoint separation.

Open-source boundary:

- Client code can be public.
- Public DNS provider registry can be public.
- Public speed-test node metadata can be public if licensed.
- API keys, signing certificates, private update endpoints, telemetry keys, and production server credentials must stay private.

## Windows-first Priority Order

1. Network Speed Test button and read-only result page.
2. DNS benchmark with 360 Secure DNS candidate scoring.
3. Windows read-only adapter expansion.
4. Game library.
5. Game session detection.
6. Low-overhead monitor.
7. Rule dry-run engine.
8. DNS apply with backup and rollback.
9. Startup and power-plan optimization.
10. Game Boost Mode balanced profile.
11. Game Boost Mode competitive profile.
12. Restore center and history.
13. AI Advisor report generation.
14. Windows release packaging.

## Technical Architecture Direction

```text
AETHER SENTINEL AI
  |
  +-- Avalonia UI
  |
  +-- Core Intelligence Engine
  |     |
  |     +-- Analysis
  |     +-- Rule Engine
  |     +-- Network Intelligence
  |     +-- Game Session Intelligence
  |     +-- Report Engine
  |
  +-- Platform Adapter Layer
        |
        +-- Windows Adapter
        |     |
        |     +-- Scanner
        |     +-- Monitor
        |     +-- DNS
        |     +-- Game Detection
        |     +-- Optimization Executor
        |
        +-- macOS Adapter
              |
              +-- Read-only Scanner
              +-- UI/Core Validation
```

## Pavise Reuse Boundary

Allowed for free/open AETHER modules:

- Study source.
- Reference behavior.
- Reuse compatible snippets only with full license compliance.
- Preserve copyright and license notices.
- Mark modifications.
- Keep distribution free.

Preferred for long-term AETHER:

- Reimplement the same feature category independently.
- Keep AETHER naming and UI distinct.
- Keep any Pavise-derived code traceable and isolated.

Do not:

- Sell Pavise-derived code.
- Hide Pavise-derived code inside paid modules.
- Use Pavise name, icon, or brand identity.
- Mix copied code with private server or paid update infrastructure.

## Definition Of Done

A Windows optimization feature is not done until:

- It has read-only detection.
- It has a dry-run preview.
- It has a risk label.
- It has user confirmation.
- It has backup.
- It has verification.
- It has rollback.
- It has a history record.
- It has Windows test notes.
- It has macOS fallback behavior if applicable.

## Decision

AETHER SENTINEL AI will use BoosterX as the commercial user-experience benchmark and Pavise as the main free/open Windows game-performance reference.

The active direction is:

```text
Windows-first, Pavise-compatible where free, BoosterX-aligned in experience, AETHER-native in architecture.
```
