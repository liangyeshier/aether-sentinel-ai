namespace AetherSentinel.Core.Scanning;

public interface IPlatformSystemAdapter
{
    string PlatformName { get; }

    ValueTask<PlatformCapabilitySet> GetCapabilitiesAsync(CancellationToken cancellationToken);

    ValueTask<SystemSnapshot> CaptureReadOnlySnapshotAsync(
        ScanRequest request,
        CancellationToken cancellationToken);
}

public sealed record PlatformCapabilitySet(
    bool CanReadHardware,
    bool CanReadProcesses,
    bool CanReadStartupItems,
    bool CanReadNetworkInterfaces,
    bool CanReadDnsConfiguration,
    bool CanRunNetworkSpeedTest,
    IReadOnlyList<string> MissingPermissions);
