namespace AetherSentinel.Core.Network;

public interface INetworkDiagnosticsProvider
{
    ValueTask<NetworkDiagnosticsReport> RunQuickDiagnosticsAsync(
        NetworkDiagnosticsRequest request,
        CancellationToken cancellationToken);
}
