namespace AetherSentinel.Core.Scanning;

public sealed record SystemSnapshot(
    DateTimeOffset CapturedAt,
    OperatingSystemSnapshot OperatingSystem,
    HardwareSnapshot Hardware,
    IReadOnlyList<ProcessSnapshot> TopProcesses,
    NetworkSnapshot Network,
    IReadOnlyList<SystemInsight> Insights);

public sealed record OperatingSystemSnapshot(
    string Name,
    string Version,
    string Architecture,
    string DeviceName);

public sealed record HardwareSnapshot(
    string CpuName,
    string GpuName,
    long MemoryTotalMb,
    long MemoryUsedMb,
    IReadOnlyList<StorageSnapshot> Storage);

public sealed record StorageSnapshot(
    string Name,
    long TotalGb,
    long FreeGb,
    double ActivePercent);

public sealed record ProcessSnapshot(
    string Name,
    int ProcessId,
    double CpuPercent,
    long MemoryMb,
    ProcessImpactLevel ImpactLevel);

public sealed record NetworkSnapshot(
    string PrimaryInterfaceName,
    string ConnectionType,
    IReadOnlyList<string> CurrentDnsServers,
    IspRegionInfo IspRegion,
    NetworkQualitySnapshot Quality);

public sealed record IspRegionInfo(
    string Country,
    string Region,
    string City,
    string Isp,
    string Source,
    double Confidence);

public sealed record NetworkQualitySnapshot(
    double LatencyMs,
    double JitterMs,
    double PacketLossPercent,
    NetworkQualityLevel QualityLevel);

public sealed record SystemInsight(
    string Title,
    string Detail,
    InsightSeverity Severity,
    string Source);

public enum ProcessImpactLevel
{
    Low,
    Medium,
    High
}

public enum NetworkQualityLevel
{
    Unknown,
    Good,
    Watch,
    Poor
}

public enum InsightSeverity
{
    Info,
    Warning,
    Critical
}
