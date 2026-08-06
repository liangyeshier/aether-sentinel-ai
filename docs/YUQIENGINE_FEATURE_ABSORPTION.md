# YuqiEngine Independent Feature Absorption

## Source

Repository reviewed:

- `https://github.com/ZYY817/YuqiEngine`
- Reviewed commit: `440f76f12d528b43406b63abbc8bba48925fbdb1`

## License And Boundary

The repository currently contains documentation, release references, and visual assets rather than a reusable open-source codebase.

AETHER SENTINEL AI must not copy YuqiEngine code, assets, UI layout, package contents, or wording.

Acceptable use:

- Product pattern study.
- Independent architecture planning.
- Independently implemented feature equivalents.

## Absorbable Product Patterns

### Status Taxonomy

AETHER should adopt a richer execution state model:

- Confirmed
- Reading
- Needs Confirmation
- Unavailable
- Applied
- Restored
- Partially Completed
- Blocked
- Simulated

Why it matters:

- Prevents false success states.
- Makes Windows permission and policy failures explainable.
- Helps users understand whether a feature was read, previewed, applied, verified, or restored.

### Backup And Restore First

Every Windows write action should require:

- Original state capture.
- Restore point or rule-level backup.
- Execution log.
- Verification signal.
- Rollback path.

Priority modules:

- DNS settings.
- Startup items.
- Power plans.
- Service startup modes.
- Game-session focus policy.
- Cleanup deletion lists.

### Problem Package

AETHER should add a local diagnostic package generator for Windows testing:

- App version.
- Windows version.
- Hardware summary.
- Recent scan summaries.
- Recent action logs.
- Error logs.
- Redacted local settings.

Privacy rule:

- Generate locally only.
- Never upload automatically.
- Redact usernames, device names, paths, IP addresses, tokens, and secrets where possible.

### Guarded Persistent Assistant

AETHER can plan a future Sentinel Guard helper, but it must stay narrow:

- Maintain only actions the user explicitly applied.
- Operate only when restore rules exist.
- Avoid high-frequency monitoring.
- Never apply all optimizations automatically.
- Back off during game sessions or low battery.

### Windows Module Pool

YuqiEngine suggests a useful Windows-first capability map:

- Game optimization center.
- CPU tuning.
- Graphics and display.
- Memory pressure.
- Audio optimization.
- Network testing.
- Peripheral optimization.
- Privacy controls.
- System services.
- Task scheduler.
- System cleanup.
- Startup manager.
- Power plan editor.
- Process priority and affinity tools.

AETHER should implement these through the existing model:

```text
Detect -> Explain -> Preview -> Backup -> Apply -> Verify -> Restore
```

## Not Adopted

- WebView2 runtime direction.
- Large offline browser-runtime package structure.
- Direct UI imitation.
- Direct asset usage.
- Any code copying.

## AETHER Implementation Direction

Phase candidates after Windows validation:

1. Add universal operation status model.
2. Add local diagnostic package generator.
3. Add Windows backup provider interfaces.
4. Add DNS real apply behind admin and rollback gates.
5. Add startup and power-plan real apply behind restore gates.
6. Add Sentinel Guard helper as opt-in and low-frequency only.
