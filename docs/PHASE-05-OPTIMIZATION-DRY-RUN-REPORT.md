# Phase 05 Optimization Rule Engine Dry Run Report

## Completed

- Added optimization rule model.
- Added dry-run report and preview model.
- Added default optimization rule catalog.
- Added dry-run engine.
- Added Optimization Center action button.
- Added live dry-run preview result cards.

## Changed Files

- `src/AetherSentinel.Core/Optimization/OptimizationRuleModels.cs`
- `src/AetherSentinel.Core/Optimization/OptimizationRuleCatalog.cs`
- `src/AetherSentinel.Core/Optimization/OptimizationDryRunEngine.cs`
- `src/AetherSentinel.UI/MainWindow.axaml.cs`
- `README.md`
- `ROADMAP.md`
- `CHANGELOG.md`
- `VERSION`

## Rule Categories

- DNS.
- Startup items.
- Power plan.
- Background pressure.
- Temporary cleanup.
- Game focus.

## Safety

Dry Run does not execute:

- DNS changes.
- Startup changes.
- Power plan changes.
- Process changes.
- Cleanup deletion.
- Notification changes.

Every rule carries:

- Risk level.
- Required privilege.
- Detection condition.
- Backup method.
- Verification signal.
- Rollback method.
- User consent requirement.

## macOS Validation

Validation command:

```bash
dotnet build AetherSentinel.sln --no-restore
```

Result:

```text
Build succeeded.
0 warnings.
0 errors.
```

Temporary smoke test result:

```text
DryRun: 6 previews / eligible 2 / blocked 0
```

## Windows Inference

Expected Windows behavior:

- Windows scan data should make startup, power plan, DNS, background pressure, cleanup, and game focus rules more accurately eligible or needs-more-data.
- The engine itself is platform-neutral and should behave the same once Windows adapter data is present.

Windows validation is still required.

## Known Issues

- No real execution exists yet.
- Blocked state is reserved but not currently emitted by default rules.
- Rule eligibility is heuristic and will need calibration on Windows.

## Risk

- Users may confuse Dry Run with execution. UI copy must keep stating that no system change is applied.
- Future execution must not be enabled until backups, verification, and rollback are implemented.

## Next Planning

- Phase 06: Safe Optimization Execution.
