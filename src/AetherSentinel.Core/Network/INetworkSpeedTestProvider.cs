namespace AetherSentinel.Core.Network;

public interface INetworkSpeedTestProvider
{
    ValueTask<NetworkSpeedTestResult> RunAsync(
        NetworkSpeedTestRequest request,
        CancellationToken cancellationToken);
}
