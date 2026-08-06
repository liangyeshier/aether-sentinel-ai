# Phase 03.1 macOS Test Report

## Scope

This report validates the current Phase 03.1 read-only scanner on the developer macOS environment and provides inferred Windows expectations.

The report does not claim Windows runtime certification. Windows results are inferred from code paths and must be confirmed on a Windows 10 or Windows 11 device.

## Test Environment

Date:

2026-08-06

Host:

- macOS 26.3.1
- Darwin 25.3.0
- arm64
- .NET SDK 8.0.129
- Runtime identifier: `osx-arm64`

Repository commit:

`96ce0be feat(platform): add local read-only scanner`

## Commands

```bash
dotnet build AetherSentinel.sln --no-restore
dotnet run --project /tmp/aether-scan-smoke/ScanSmoke.csproj
```

## Build Result

```text
Build succeeded.
0 Warning(s)
0 Error(s)
```

## macOS Smoke Test Result

The scanner returned:

```text
OS=Darwin 25.3.0
Version=Unix 26.3.1
Architecture=Arm64
Device=ANSON-MacBook-Pro
CPU=Apple M5
MemoryMb=22480/32768
DriveCount=4
ProcessCount=5
Network=en0|Wireless80211
DnsCount=2
Dns=119.29.29.29,114.114.114.114
InsightCount=2
```

Validated:

- OS detection works.
- CPU name detection works through read-only `sysctl`.
- Memory total and used memory detection works through read-only `sysctl` and `vm_stat`.
- Storage detection works through .NET `DriveInfo`.
- Top process detection works through .NET `Process`.
- Active network interface detection works through .NET `NetworkInterface`.
- DNS detection works on macOS after fallback from `networksetup` to `scutil --dns`.
- Scanner completes without system modification.

## macOS DNS Finding

Initial `networksetup -getdnsservers Wi-Fi` returned:

```text
There aren't any DNS Servers set on Wi-Fi.
```

However, `scutil --dns` returned active resolver nameservers:

```text
119.29.29.29
114.114.114.114
```

Resolution:

- The macOS adapter now first checks service DNS through `networksetup`.
- If no service DNS is found, it falls back to resolver DNS through `scutil --dns`.

## Windows Inferred Test Matrix

| Capability | Current Windows Code Path | Inferred Result | Requires Windows Device |
|---|---|---|---|
| Build | .NET 8 cross-platform projects | Likely pass | Yes |
| Avalonia shell launch | Avalonia Desktop on .NET 8 | Likely pass | Yes |
| OS name/version | `RuntimeInformation`, `Environment.OSVersion` | Likely pass | Yes |
| CPU label | Falls back to process architecture | Pass but low detail | Yes |
| Memory total | `GC.GetGCMemoryInfo().TotalAvailableMemoryBytes` fallback | Likely pass, may be less precise | Yes |
| Memory used | `GC.GetTotalMemory` fallback | Pass but currently app-process memory only | Yes |
| Storage | .NET `DriveInfo` | Likely pass | Yes |
| Processes | .NET `Process.GetProcesses()` and `WorkingSet64` | Likely pass, permission-dependent | Yes |
| Network interface | .NET `NetworkInterface` | Likely pass | Yes |
| DNS servers | PowerShell `Get-DnsClientServerAddress` | Likely pass if PowerShell is available | Yes |
| GPU | Not implemented | Not available | Yes |
| Startup items | Not implemented | Not available | Yes |
| ISP/region | Not implemented | Not available | Yes |
| Network speed test | Not implemented | Not available | Yes |

## Windows Risk Notes

- The current Windows memory fallback is not equivalent to macOS physical memory usage. It may need a Windows-specific implementation.
- The current Windows CPU label is generic and should be replaced with a Windows-specific provider.
- DNS detection depends on PowerShell availability and command output shape.
- Process memory reads can fail for protected processes, but the adapter already ignores inaccessible processes.
- No Windows optimization actions exist, so the current scanner should remain read-only.

## Conclusion

macOS Phase 03.1 read-only scanning is functionally validated for the current scope.

Windows behavior is structurally likely to build and partially run because the project uses .NET 8 and isolated Windows code paths, but Windows must still be tested on a real Windows 10 or Windows 11 machine before marking the adapter as validated.
