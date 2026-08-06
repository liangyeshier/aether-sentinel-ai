namespace AetherSentinel.Core.Monitoring;

public interface ILowOverheadMonitor
{
    ValueTask<MonitorSnapshot> CaptureOnceAsync(
        MonitorRequest request,
        CancellationToken cancellationToken);
}
