# Competitive Feature Roadmap

## Purpose

This document merges the useful product ideas found in BoosterX, Pavise, and LaoYing Toolkit into a safe AETHER SENTINEL AI upgrade roadmap.

The goal is feature synthesis, not code copying.

## Source Status

### BoosterX

Status:

Commercial reference product.

Source:

`https://boosterx.org/en/howitworks/`

Observed public positioning:

- System analysis before optimization.
- CPU or GPU bottleneck indication.
- Boost potential.
- Current optimization level.
- Privacy level.
- Future-proof slowdown prevention.
- Review-before-apply workflow.
- Backup and restore.

Reuse rule:

Use only public product behavior as competitive reference. Do not copy branding, wording, private logic, UI assets, or implementation.

### Pavise

Status:

Public source repository with a custom non-sale license.

Source:

`https://github.com/dulaiduwang003/Pavise-Game`

License source:

`https://github.com/dulaiduwang003/Pavise-Game/blob/main/LICENSE`

Observed product direction:

- Windows game performance protection.
- Local-only operation.
- No game-process injection.
- Reversible changes.
- Game target library and game-session recognition.
- Background process pressure reduction.
- CPU, I/O, scheduler, power plan, notification, service, and driver-level tuning.
- Anti-cheat and system-process safety boundaries.
- Per-session reports.
- Read-back verification after writes when possible.

License boundary:

The license allows free use, modification, learning, and free distribution, but prohibits selling the software or derivatives. AETHER SENTINEL AI must not copy Pavise source code, UI, name, icon, or derivative implementation unless a future legal review confirms a compatible path.

Allowed use:

- Product learning.
- Architecture comparison.
- Independent reimplementation of general ideas.
- Feature-level inspiration with separate design and code.

### LaoYing Toolkit

Status:

Public references found, but authoritative source repository and license are not confirmed.

Observed public references:

- `https://www.ckbf88.com/sys-nd/456.html`
- `https://search.bilibili.com/all?keyword=TOOLKIT`

Observed feature direction from public snippets:

- Game-process CPU scheduling.
- Core layout.
- Kernel isolation.
- NVIDIA DLSS model configuration.
- Global graphics presets.
- System optimization settings.
- Power plan management.
- Startup item management.
- Service optimization.
- NVIDIA preset tuning.

Reuse rule:

Treat LaoYing Toolkit only as a feature-category reference until an official source repository, license, and documentation are verified.

## Competitive Synthesis

BoosterX contributes:

- User-friendly diagnosis.
- Optimization potential scoring.
- Privacy and future slowdown categories.
- Review-before-apply and restore workflow.

Pavise contributes:

- Game-session protection.
- Background resource pressure control.
- No-injection safety boundary.
- Reversible session-level optimization.
- Session reports and verification.

LaoYing Toolkit contributes:

- Practical Windows tuning categories.
- CPU scheduling and core layout concepts.
- GPU preset and DLSS-focused user demand.
- Startup, service, and power-plan tool modules.

AETHER SENTINEL AI must combine these into:

```text
Detect -> Analyze -> Explain -> Preview -> Backup -> Apply -> Verify -> Report -> Rollback
```

## Product Direction

AETHER SENTINEL AI should not be a tweak launcher.

It should become a low-overhead AI performance guardian with:

- Read-only intelligence first.
- User-approved optimization only.
- Clear risk labels.
- Measured before/after results.
- Rollback for every applied rule.
- No game memory modification.
- No game file modification.
- No anti-cheat bypass behavior.
- Minimal background activity.

## New Functional Upgrade Roadmap

### Phase 03.3 - Competitive Feature Synthesis

Status:

Completed by this document.

Deliverables:

- BoosterX, Pavise, and LaoYing feature comparison.
- License and reuse boundary.
- Combined feature roadmap.
- Priority order for implementation.

### Phase 03.4 - Network Intelligence Activation

Goal:

Turn the reserved Network Speed Test and DNS Optimization modules into read-only, user-triggered tools.

Features:

- Network Speed Test button and results page.
- China mainland ISP detection.
- Region detection with confidence level.
- Latency-only test mode.
- Full speed test mode with explicit user confirmation.
- Download, upload, ping, jitter, and failure-rate metrics.
- DNS benchmark against current DNS, 360 Secure DNS, and other verified providers.
- Recommendation based on measured local result, not provider popularity.

Low-overhead rules:

- Never run speed tests automatically.
- Never run full-bandwidth tests in background.
- Cache ISP and region results.
- Keep all tests cancellable.

### Phase 03.5 - Game Session Intelligence

Goal:

Build safe game detection before any game optimization.

Features:

- User-managed game library.
- EXE, shortcut, Steam, Epic, GOG, Battle.net, Xbox, Riot, Ubisoft, WeGame detection candidates.
- Foreground window and full-screen state detection.
- Launcher-versus-game-body classification.
- Anti-cheat, launcher, updater, and crash reporter role detection.
- Session lifecycle state.
- Per-session read-only report.

Safety rules:

- No injection.
- No memory modification.
- No game file modification.
- No automatic process suppression.

### Phase 04 - Low-overhead Performance Monitor

Goal:

Create a native, on-demand monitor that can explain performance pressure with minimal cost.

Features:

- CPU usage.
- Memory pressure.
- Disk queue and I/O pressure.
- Network latency and throughput.
- Top background pressure sources.
- App overhead meter.
- Adaptive sampling.
- Pause or reduce sampling while hidden.

Platform scope:

- macOS read-only monitor first for local development validation.
- Windows native adapter as the release-critical target.

### Phase 05 - Optimization Rule Engine Dry Run

Goal:

Define every optimization as a reversible, inspectable rule.

Features:

- Rule catalog.
- Rule risk level.
- Affected system area.
- Backup plan.
- Verification signal.
- Rollback plan.
- Dry-run preview.
- User consent record.

Initial rule categories:

- Startup pressure.
- Background process pressure.
- DNS candidate switching.
- Power plan recommendation.
- Notification and game-focus settings.
- Temporary file cleanup.
- Service review.
- Privacy telemetry review.

### Phase 06 - Safe Optimization Execution

Goal:

Enable only low-risk, reversible optimizations.

Features:

- DNS apply with backup and rollback.
- Startup item disable/restore.
- Temporary file cleanup with size preview.
- Power profile changes with original value snapshot.
- Notification/game-focus changes.
- One-click restore center.

Blocked until:

- Rule schema is complete.
- Backup and rollback are implemented.
- Write actions pass Windows test matrix.

### Phase 07 - Game Boost Mode

Goal:

Add Pavise-inspired game-session protection with AETHER safety and explanation.

Features:

- Game session start and end detection.
- Foreground game priority recommendation.
- Background pressure reduction preview.
- User whitelist.
- Protected process list.
- Anti-cheat safe boundary.
- Session-level apply and automatic restore.
- Before/after session report.

Possible Windows-only execution areas:

- Process priority.
- I/O priority.
- CPU set or affinity policy where safe.
- Power plan.
- Game DVR and notification review.
- MMCSS and network throttling review.
- NVIDIA or AMD settings only through official supported interfaces.

Non-goals:

- No anti-cheat bypass.
- No memory injection.
- No driver tampering.
- No undocumented dangerous tweak by default.

### Phase 08 - Toolkit Layer

Goal:

Add LaoYing-style practical utility modules without turning the product into a cluttered toolbox.

Features:

- Startup manager.
- Service review.
- Power plan center.
- GPU setting inspector.
- DNS and network tools.
- Storage cleanup.
- Memory pressure inspection.
- System shortcut launcher.
- Restore and backup center.

Design rule:

Every tool must explain why it exists, what risk it has, and how to revert.

### Phase 09 - AI Advisor And History

Goal:

Make the product feel intelligent rather than mechanical.

Features:

- Natural-language performance explanation.
- Bottleneck summary.
- Optimization recommendation.
- Risk assessment.
- Historical score comparison.
- Before/after report.
- Exportable local report with private data redaction.

### Phase 10 - Release And Update Infrastructure

Goal:

Prepare public open-source releases while protecting private infrastructure.

Features:

- GitHub Actions build.
- Signed release packaging plan.
- Update manifest format.
- Package verification.
- Rollback-aware update manager.
- Private endpoint separation through environment configuration.

Open-source boundary:

- Public source can include client logic, public provider registries, and example configuration.
- Private server endpoints, signing credentials, telemetry keys, and production deployment secrets must stay out of the repository.

## BoosterX Alignment Matrix

| BoosterX Capability | AETHER Status | Planned Phase |
|---|---|---|
| System analysis before optimization | Partially implemented | Phase 03 |
| CPU/GPU bottleneck explanation | Planned | Phase 04 |
| Boost potential score | Partially implemented | Phase 03 |
| Current optimization level | Planned | Phase 05 |
| Privacy level | Planned | Phase 05 |
| Future slowdown prevention | Planned | Phase 05 |
| Review before apply | Planned | Phase 05 |
| Backup and restore | Planned | Phase 06 |

## Pavise Alignment Matrix

| Pavise-Inspired Capability | AETHER Status | Planned Phase |
|---|---|---|
| Game library | Planned | Phase 03.5 |
| Game session recognition | Planned | Phase 03.5 |
| No injection boundary | Required | Phase 03.5 |
| Background pressure analysis | Planned | Phase 04 |
| Resource policy preview | Planned | Phase 05 |
| Session-level restore | Planned | Phase 07 |
| Per-session report | Planned | Phase 07 |
| Anti-cheat safety boundary | Required | Phase 07 |

## LaoYing Alignment Matrix

| LaoYing-Style Capability | AETHER Status | Planned Phase |
|---|---|---|
| CPU scheduling tools | Planned | Phase 07 |
| Core layout inspection | Planned | Phase 04 |
| Kernel isolation review | Research only | Phase 08 |
| DLSS/NVIDIA settings | Research only | Phase 08 |
| Global graphics presets | Research only | Phase 08 |
| Power plan management | Planned | Phase 06 |
| Startup item management | Planned | Phase 06 |
| Service optimization | Planned | Phase 08 |

## Implementation Priority

1. Network Intelligence Activation.
2. Game Session Intelligence.
3. Low-overhead Performance Monitor.
4. Rule Engine Dry Run.
5. Safe Optimization Execution.
6. Game Boost Mode.
7. Toolkit Layer.
8. AI Advisor and History.
9. Release and Update Infrastructure.

## Risk Register

### Legal And License Risk

Pavise is public source but not commercially reusable as-is because of the no-sale custom license. LaoYing source and license are not confirmed. AETHER must use independent implementation.

### Safety Risk

CPU scheduling, service changes, driver settings, DNS switching, and power plans can harm user workflows. Every write action must have backup, verification, and rollback.

### Anti-cheat Risk

Game optimization must avoid injection, memory modification, game file changes, anti-cheat bypass, or behavior that could be interpreted as tampering.

### Performance Overhead Risk

Monitoring and scanning can become the problem they claim to solve. Default operation must stay read-only, on-demand, and low-frequency.

### China Network Accuracy Risk

ISP and region detection are estimates. The product must show confidence and source, and it must avoid pretending perfect precision.

## Decision

AETHER SENTINEL AI will align with BoosterX-level usability, Pavise-level game-session safety thinking, and LaoYing-style utility breadth, while keeping its own identity:

```text
AI-native, low-overhead, explainable, reversible, open where safe.
```
