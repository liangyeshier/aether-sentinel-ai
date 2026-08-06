# Roadmap

AETHER SENTINEL AI uses phase-based planning.

## Phase 00 - Product Foundation

Status: Completed

Scope:

- Product identity
- Brand system
- UI design system
- Architecture documentation
- GitHub workflow
- Issue templates
- Milestone framework
- Automatic update architecture reservation

## Phase 01 - Desktop Shell Preview

Status: Completed

Scope:

- Runnable Avalonia UI application shell.
- Dashboard visual prototype.
- Static simulated performance data.
- No production optimization features.
- No real system data collection.

## Phase 02 - Module Activation

Status: Completed

Scope:

- Left navigation becomes clickable.
- All main modules render dedicated pages.
- Module content remains read-only and simulated.
- Low-overhead operating rules are visible in Settings and Monitor views.
- No real system scanning, background service, or optimization execution is added.

## Phase 02.1 - DNS Optimization Reservation

Status: Completed

Scope:

- Add DNS Optimization as a dedicated left navigation module.
- Reserve read-only DNS detection, latency benchmarking, secure DNS provider comparison, and rollback requirements.
- Treat 360 Secure DNS as a candidate provider only after official public endpoints and usage terms are confirmed.
- Do not change system DNS settings in this phase.

## Phase 03 - Read-only System Intelligence

Status: In Progress

Scope:

- Build cross-platform scanner contracts in the core layer.
- Add Windows and macOS adapter boundaries for read-only system information.
- Collect CPU, GPU, memory, storage, OS, process, startup, network, DNS, ISP region, and speed test state without making system changes.
- Keep background sampling disabled by default to protect idle CPU and memory usage.

Current deliverables:

- Core project added to the solution.
- Read-only system snapshot models.
- Platform adapter interface boundary.
- `AetherSentinel.Platforms` project added to the solution.
- First local read-only adapter for OS, memory, storage, process, network interface, and DNS data.
- Scan button updates dashboard and live module cards.
- Read-only performance analyzer added.
- Overall score, optimization potential, score factors, and recommendations generated from scan data.
- Network quality, DNS, ISP region, and speed test models.
- Network Speed Test navigation reservation.
- Network Intelligence planning document.

Remaining:

- Windows read-only adapter implementation.
- macOS read-only adapter expansion for GPU, startup items, and richer CPU sampling.
- Real ISP and region lookup provider.
- User-approved speed test execution.

## Phase 04 - Low-overhead Performance Monitoring

Status: Planned

Scope:

- Add on-demand realtime metrics with strict sampling budgets.
- Prefer native APIs and lightweight timers over always-on polling.
- Reduce or pause sampling while the app is hidden, idle, or during game sessions.
- Expose performance cost inside Settings so users can understand overhead.

## Phase 05 - Optimization Rule Engine Dry Run

Status: Planned

Scope:

- Define safe optimization rule schema.
- Support dry-run previews before any real change.
- Require risk level, backup method, verification signal, and rollback method for every rule.
- Keep all execution disabled until rules pass validation.

## Future Product Expansion

The architecture reserves space for future AETHER product lines:

- AETHER SENTINEL: Performance intelligence
- AETHER CREATOR: Creator workflow optimization
- AETHER FLOW: Automation agent
- AETHER VISION: AI visual intelligence

These products are not implemented in Phase 00.
