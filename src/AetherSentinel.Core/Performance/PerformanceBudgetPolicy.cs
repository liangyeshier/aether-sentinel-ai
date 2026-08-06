namespace AetherSentinel.Core.Performance;

public sealed record PerformanceBudgetPolicy(
    int MaxCpuPercentWhileIdle,
    int MaxMemoryMb,
    TimeSpan MinimumForegroundSamplingInterval,
    TimeSpan MinimumBackgroundSamplingInterval,
    bool DisablePersistentPolling)
{
    public static PerformanceBudgetPolicy DefaultLowOverhead { get; } = new(
        MaxCpuPercentWhileIdle: 1,
        MaxMemoryMb: 150,
        MinimumForegroundSamplingInterval: TimeSpan.FromSeconds(2),
        MinimumBackgroundSamplingInterval: TimeSpan.FromSeconds(15),
        DisablePersistentPolling: true);
}
