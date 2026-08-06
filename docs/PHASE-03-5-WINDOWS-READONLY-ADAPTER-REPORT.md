# Phase 03.5 Windows Read-only Adapter Expansion Report

## Completed

- Expanded `SystemSnapshot` with startup items, active power plan, and game process candidates.
- Added Windows CPU name detection through `Win32_Processor`.
- Added Windows GPU name detection through `Win32_VideoController`.
- Added Windows physical memory detection through `Win32_ComputerSystem` and `Win32_OperatingSystem`.
- Added Windows startup item detection through `Win32_StartupCommand`.
- Added Windows active power plan detection through `powercfg /getactivescheme`.
- Added read-only game-related process candidate detection.
- Updated PC Intelligence and Game Optimization module pages to display the new fields.
- Preserved macOS fallback behavior for development and validation.

## Changed Files

- `src/AetherSentinel.Core/Scanning/SystemSnapshot.cs`
- `src/AetherSentinel.Platforms/Scanning/LocalPlatformSystemAdapter.cs`
- `src/AetherSentinel.UI/MainWindow.axaml.cs`
- `README.md`
- `ROADMAP.md`
- `CHANGELOG.md`
- `VERSION`

## Read-only Data Added

- Startup item name, command, location, user, and estimated impact.
- Active power plan name, identifier, source, and high-performance candidate flag.
- Game process candidate name, PID, role, reason, and confidence.

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
OS: Darwin 25.3.0 Darwin Kernel Version 25.3.0: Wed Jan 28 20:56:42 PST 2026; root:xnu-12377.91.3~2/RELEASE_ARM64_T8142
CPU: Apple M5
GPU: Apple M5
Startup: 0
Power: Not applicable
Game candidates: 0
```

## Windows Inference

Expected Windows behavior:

- CPU, GPU, physical memory, startup items, active power plan, process list, DNS, and network interfaces should be collected without applying changes.
- Startup and power plan reads should not require administrator privileges.
- Game candidate detection is heuristic and must be validated against real Windows gaming environments.

Windows validation is still required.

## Known Issues

- CPU usage percentage remains pending.
- GPU load, temperature, and driver data are not implemented.
- Startup impact is heuristic only.
- Game process detection is a candidate classifier, not a final game-session detector.

## Risk

- PowerShell/CIM availability may vary across Windows editions and policy settings.
- Some process and startup data can be inaccessible under restricted accounts.
- Game candidate classification must avoid treating launchers or anti-cheat processes as the game body.

## Next Planning

- Phase 03.6: Game Library And Session Detection.
