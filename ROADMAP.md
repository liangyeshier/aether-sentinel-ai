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

- Real ISP and region lookup provider.
- User-approved speed test execution.
- CPU usage sampling.
- GPU load and temperature.

## Phase 03.3 - Competitive Feature Synthesis

Status: Completed

Scope:

- Review public BoosterX behavior as a commercial reference.
- Review Pavise as a public-source Windows game performance reference with a custom non-sale license.
- Remove LaoYing Toolkit from active planning until official source and license are confirmed.
- Define the combined AETHER feature upgrade roadmap.
- Preserve independent implementation and license safety boundaries.

Deliverable:

- `docs/COMPETITIVE_FEATURE_ROADMAP.md`
- `docs/WINDOWS_FIRST_UPGRADE_ROADMAP.md`

Platform decision:

- Windows is the primary runtime and feature target.
- macOS remains the current development, UI validation, core validation, and read-only test environment.

## Phase 03.4 - Network Intelligence Activation

Status: Completed

Scope:

- Activate Network Speed Test as a user-triggered module.
- Support latency-only checks.
- Benchmark current DNS, 360 Secure DNS, and verified DNS providers.
- Recommend DNS only from measured latency, jitter, failure rate, region, ISP, and user intent.

Current deliverables:

- Quick Network Speed Test button.
- Ping/Jitter measurement.
- DNS UDP lookup benchmark.
- 360 Secure DNS benchmark path.
- Live Network Speed Test result cards.
- Full bandwidth tests remain disabled.

Remaining:

- China mainland ISP and region lookup with confidence levels.
- Explicit full speed tests.

## Phase 03.5 - Windows Read-only Adapter Expansion

Status: Completed

Scope:

- Expand Windows read-only scanner paths.
- Add startup item, active power plan, GPU, and game process candidate context.
- Preserve macOS fallback behavior for development validation.

Current deliverables:

- Windows CPU name, GPU name, physical memory, startup items, and active power plan read paths.
- Startup item, power plan, and game process candidate snapshot models.
- PC Intelligence and Game Optimization live rows for Windows-first read-only fields.
- macOS fallback validation.

Remaining:

- Windows real-device validation.
- CPU usage sampling.
- Richer GPU and driver data.

## Phase 03.6 - Game Session Intelligence

Status: Completed

Scope:

- Add user-managed game library.
- Detect game sessions, foreground state, launchers, updaters, crash reporters, and anti-cheat roles.
- Produce read-only session intelligence before any optimization execution.
- Keep no-injection, no-memory-modification, and no-game-file-modification boundaries.

Current deliverables:

- Local game library model and persistence.
- Add Game button.
- Detect Session button.
- Read-only game session analyzer.
- Game, launcher, anti-cheat, and capture-tool candidate boundaries.

Remaining:

- Foreground window and fullscreen detection.
- Launcher-to-game-body learning.
- Store/library auto-scan.

## Phase 04 - Low-overhead Performance Monitoring

Status: Completed

Scope:

- Add on-demand realtime metrics with strict sampling budgets.
- Prefer native APIs and lightweight timers over always-on polling.
- Reduce or pause sampling while the app is hidden, idle, or during game sessions.
- Expose performance cost inside Settings so users can understand overhead.
- Explain top CPU, memory, disk, network, and background pressure sources.

Current deliverables:

- User-triggered single light sample.
- AETHER self CPU estimate.
- AETHER self memory measurement.
- Process count.
- Top memory pressure processes.
- Monitor warnings.

Remaining:

- System-wide CPU sampling.
- GPU load and temperature.
- Disk queue and network throughput.
- Continuous sampling controls.

## Phase 05 - Optimization Rule Engine Dry Run

Status: Completed

Scope:

- Define safe optimization rule schema.
- Support dry-run previews before any real change.
- Require risk level, backup method, verification signal, and rollback method for every rule.
- Keep all execution disabled until rules pass validation.

Current deliverables:

- Optimization rule model.
- Default rule catalog.
- Dry-run engine.
- Optimization Center action button.
- Live dry-run result cards.

Remaining:

- Execution layer.
- More precise Windows calibration.
- Blocked-state policy enforcement.

## Phase 06 - Safe Optimization Execution

Status: Completed

Scope:

- Enable only low-risk, reversible optimization rules.
- Start with DNS apply, startup item restore, temporary cleanup, power profile changes, notification review, and privacy telemetry review.
- Require backup, user confirmation, verification, and rollback for every write action.

Current deliverables:

- Execution request and report models.
- Execution result and restore point models.
- Guarded execution engine.
- Safe simulation action in Optimization Center.
- Result cards for simulated and blocked actions.

Remaining:

- Real Windows executors.
- Persistent execution history.
- Verified rollback on Windows.

## Phase 07 - Game Boost Mode

Status: Completed

Scope:

- Add game-session-level resource protection.
- Use no-injection process and system policy boundaries.
- Support protected process lists, user whitelists, anti-cheat safety boundaries, and session restore.
- Generate before/after session reports.

Current deliverables:

- Game Boost plan model.
- Game Boost planner.
- Balanced mode plan generation.
- Game Optimization Boost Plan button.
- Preview cards for background pressure, priority, I/O, power, focus, and restore.

Remaining:

- Real Windows session executor.
- Foreground/fullscreen detection.
- Competitive mode UI.
- Verified restore path.

## Phase 08 - Toolkit Layer

Status: Planned

Scope:

- Add practical utility modules for startup, services, power plans, DNS, network, storage cleanup, memory pressure, GPU inspection, and restore center.
- Keep tools grouped, reversible, and explanation-first.

## Phase 09 - AI Advisor And History

Status: Planned

Scope:

- Generate natural-language performance explanations.
- Compare historical scores and session results.
- Export local reports with private data redaction.

## Phase 10 - Release And Update Infrastructure

Status: Planned

Scope:

- Prepare GitHub Actions build and release packaging.
- Reserve update manifest, package verification, and rollback-aware UpdateManager.
- Keep private endpoints, signing credentials, and production secrets outside the open repository.

## Future Product Expansion

The architecture reserves space for future AETHER product lines:

- AETHER SENTINEL: Performance intelligence
- AETHER CREATOR: Creator workflow optimization
- AETHER FLOW: Automation agent
- AETHER VISION: AI visual intelligence

These products are not implemented in Phase 00.
