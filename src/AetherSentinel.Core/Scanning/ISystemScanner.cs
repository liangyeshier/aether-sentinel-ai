using AetherSentinel.Core.Performance;

namespace AetherSentinel.Core.Scanning;

public interface ISystemScanner
{
    ValueTask<SystemSnapshot> CaptureAsync(ScanRequest request, CancellationToken cancellationToken);
}

public sealed record ScanRequest(
    bool IncludeProcesses,
    bool IncludeNetwork,
    bool IncludeDns,
    PerformanceBudgetPolicy Budget);
