using AetherSentinel.Core.Performance;
using AetherSentinel.Core.Scanning;

namespace AetherSentinel.Core.Monitoring;

public sealed record MonitorRequest(
    MonitorSamplingMode Mode,
    TimeSpan SampleWindow,
    int TopProcessCount,
    PerformanceBudgetPolicy Budget);

public sealed record MonitorSnapshot(
    DateTimeOffset CapturedAt,
    MonitorSamplingMode Mode,
    double AppCpuPercent,
    long AppMemoryMb,
    int ProcessCount,
    IReadOnlyList<ProcessSnapshot> TopMemoryProcesses,
    IReadOnlyList<MonitorWarning> Warnings,
    string Method);

public sealed record MonitorWarning(
    string Title,
    string Detail,
    MonitorWarningSeverity Severity);

public enum MonitorSamplingMode
{
    Off,
    Light,
    Active
}

public enum MonitorWarningSeverity
{
    Info,
    Watch,
    Risk
}
