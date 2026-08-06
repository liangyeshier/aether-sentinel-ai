namespace AetherSentinel.Core.Gpu;

public sealed record GpuIntelligenceReport(
    DateTimeOffset AnalyzedAt,
    string Name,
    GpuVendor Vendor,
    string DriverVersion,
    GpuTelemetryAvailability TelemetryAvailability,
    bool DriverWriteActionsEnabled,
    IReadOnlyList<GpuInsight> Insights);

public sealed record GpuInsight(
    string Title,
    string Detail,
    GpuInsightSeverity Severity);

public enum GpuVendor
{
    Unknown,
    Nvidia,
    Amd,
    Intel,
    Apple,
    MicrosoftBasic
}

public enum GpuTelemetryAvailability
{
    Unknown,
    NameOnly,
    DriverOnly,
    Partial,
    Full
}

public enum GpuInsightSeverity
{
    Info,
    Watch,
    Risk
}
