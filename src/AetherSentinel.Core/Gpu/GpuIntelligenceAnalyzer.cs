using AetherSentinel.Core.Scanning;

namespace AetherSentinel.Core.Gpu;

public interface IGpuIntelligenceAnalyzer
{
    GpuIntelligenceReport Analyze(SystemSnapshot snapshot);
}

public sealed class GpuIntelligenceAnalyzer : IGpuIntelligenceAnalyzer
{
    public GpuIntelligenceReport Analyze(SystemSnapshot snapshot)
    {
        var name = string.IsNullOrWhiteSpace(snapshot.Hardware.GpuName)
            ? "Unknown GPU"
            : snapshot.Hardware.GpuName;
        var vendor = DetectVendor(name);
        var insights = CreateInsights(name, vendor);

        return new GpuIntelligenceReport(
            AnalyzedAt: DateTimeOffset.Now,
            Name: name,
            Vendor: vendor,
            DriverVersion: "Not collected",
            TelemetryAvailability: string.Equals(name, "Read-only adapter pending", StringComparison.OrdinalIgnoreCase)
                ? GpuTelemetryAvailability.Unknown
                : GpuTelemetryAvailability.NameOnly,
            DriverWriteActionsEnabled: false,
            Insights: insights);
    }

    private static GpuVendor DetectVendor(string name)
    {
        if (name.Contains("NVIDIA", StringComparison.OrdinalIgnoreCase) ||
            name.Contains("GeForce", StringComparison.OrdinalIgnoreCase) ||
            name.Contains("RTX", StringComparison.OrdinalIgnoreCase) ||
            name.Contains("GTX", StringComparison.OrdinalIgnoreCase))
        {
            return GpuVendor.Nvidia;
        }

        if (name.Contains("AMD", StringComparison.OrdinalIgnoreCase) ||
            name.Contains("Radeon", StringComparison.OrdinalIgnoreCase))
        {
            return GpuVendor.Amd;
        }

        if (name.Contains("Intel", StringComparison.OrdinalIgnoreCase) ||
            name.Contains("Arc", StringComparison.OrdinalIgnoreCase))
        {
            return GpuVendor.Intel;
        }

        if (name.Contains("Apple", StringComparison.OrdinalIgnoreCase))
        {
            return GpuVendor.Apple;
        }

        if (name.Contains("Microsoft Basic", StringComparison.OrdinalIgnoreCase))
        {
            return GpuVendor.MicrosoftBasic;
        }

        return GpuVendor.Unknown;
    }

    private static IReadOnlyList<GpuInsight> CreateInsights(string name, GpuVendor vendor)
    {
        var insights = new List<GpuInsight>
        {
            new(
                Title: "GPU identity",
                Detail: $"Detected GPU name: {name}.",
                Severity: GpuInsightSeverity.Info),
            new(
                Title: "Driver writes disabled",
                Detail: "AETHER does not write GPU driver settings until official APIs, hardware validation, backup, verification, and rollback are implemented.",
                Severity: GpuInsightSeverity.Info)
        };

        if (vendor is GpuVendor.Nvidia or GpuVendor.Amd)
        {
            insights.Add(new GpuInsight(
                Title: "Driver intelligence candidate",
                Detail: $"{vendor} driver inspection can be expanded on Windows through official or documented APIs before any tuning.",
                Severity: GpuInsightSeverity.Watch));
        }

        if (vendor == GpuVendor.MicrosoftBasic)
        {
            insights.Add(new GpuInsight(
                Title: "Driver risk",
                Detail: "Microsoft Basic Display Adapter may indicate missing vendor drivers.",
                Severity: GpuInsightSeverity.Risk));
        }

        return insights;
    }
}
