using AetherSentinel.Core.Scanning;

namespace AetherSentinel.Platforms.Scanning;

public sealed class PlatformSystemScanner(IPlatformSystemAdapter adapter) : ISystemScanner
{
    public ValueTask<SystemSnapshot> CaptureAsync(ScanRequest request, CancellationToken cancellationToken)
    {
        return adapter.CaptureReadOnlySnapshotAsync(request, cancellationToken);
    }
}
