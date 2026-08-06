using AetherSentinel.Core.Analysis;
using AetherSentinel.Core.Gaming;
using AetherSentinel.Core.Gpu;
using AetherSentinel.Core.Monitoring;
using AetherSentinel.Core.Network;
using AetherSentinel.Core.Optimization;
using AetherSentinel.Core.Scanning;

namespace AetherSentinel.Core.Advisor;

public interface IAdvisorReportGenerator
{
    AdvisorReport Generate(AdvisorReportContext context);
}

public sealed record AdvisorReportContext(
    SystemSnapshot? Snapshot,
    PerformanceAnalysisReport? PerformanceReport,
    NetworkDiagnosticsReport? NetworkReport,
    GameSessionAnalysis? GameSession,
    GameBoostPlan? GameBoostPlan,
    MonitorSnapshot? MonitorSnapshot,
    GpuIntelligenceReport? GpuReport,
    OptimizationDryRunReport? DryRunReport,
    OptimizationExecutionReport? ExecutionReport);

public sealed class AdvisorReportGenerator : IAdvisorReportGenerator
{
    public AdvisorReport Generate(AdvisorReportContext context)
    {
        var findings = new List<AdvisorFinding>();
        var recommendations = new List<AdvisorRecommendation>();

        AddPerformanceFindings(context, findings, recommendations);
        AddNetworkFindings(context, findings, recommendations);
        AddGameFindings(context, findings, recommendations);
        AddMonitorFindings(context, findings, recommendations);
        AddGpuFindings(context, findings, recommendations);
        AddOptimizationFindings(context, findings, recommendations);

        if (findings.Count == 0)
        {
            findings.Add(new AdvisorFinding(
                Title: "No local report data",
                Detail: "Run scan, network test, monitor sample, or optimization dry-run to create richer advice.",
                Severity: AdvisorSeverity.Info,
                Source: "Advisor"));
        }

        var summary = CreateSummary(context, findings, recommendations);
        return new AdvisorReport(
            Id: Guid.NewGuid().ToString("N"),
            GeneratedAt: DateTimeOffset.Now,
            Summary: summary,
            Findings: findings,
            Recommendations: recommendations,
            PrivacyRedactionApplied: true);
    }

    private static void AddPerformanceFindings(
        AdvisorReportContext context,
        List<AdvisorFinding> findings,
        List<AdvisorRecommendation> recommendations)
    {
        if (context.PerformanceReport is null)
        {
            recommendations.Add(new AdvisorRecommendation(
                Title: "Run read-only scan",
                Detail: "A scan is required before AETHER can explain bottlenecks.",
                Severity: AdvisorSeverity.Info,
                NextVerification: "Scan result should produce score factors."));
            return;
        }

        findings.Add(new AdvisorFinding(
            Title: "Performance score",
            Detail: $"Current score is {context.PerformanceReport.OverallScore}/100 with {context.PerformanceReport.OptimizationPotential} optimization potential.",
            Severity: context.PerformanceReport.OverallScore >= 85 ? AdvisorSeverity.Info : AdvisorSeverity.Watch,
            Source: "PerformanceAnalyzer"));

        foreach (var recommendation in context.PerformanceReport.Recommendations.Take(2))
        {
            recommendations.Add(new AdvisorRecommendation(
                Title: recommendation.Title,
                Detail: recommendation.Detail,
                Severity: recommendation.RiskLevel == RiskLevel.High ? AdvisorSeverity.Risk : AdvisorSeverity.Watch,
                NextVerification: recommendation.VerificationSignal));
        }
    }

    private static void AddNetworkFindings(
        AdvisorReportContext context,
        List<AdvisorFinding> findings,
        List<AdvisorRecommendation> recommendations)
    {
        if (context.NetworkReport is null)
        {
            return;
        }

        findings.Add(new AdvisorFinding(
            Title: "Network quick test",
            Detail: context.NetworkReport.Summary,
            Severity: context.NetworkReport.SpeedResult.QualityLevel == NetworkQualityLevel.Poor ? AdvisorSeverity.Risk : AdvisorSeverity.Info,
            Source: "NetworkDiagnostics"));

        recommendations.Add(new AdvisorRecommendation(
            Title: "Keep DNS benchmark local",
            Detail: "DNS recommendation should be based on measured latency, jitter, and failure rate, not popularity.",
            Severity: AdvisorSeverity.Info,
            NextVerification: "Repeat quick diagnostics before applying DNS changes."));
    }

    private static void AddGameFindings(
        AdvisorReportContext context,
        List<AdvisorFinding> findings,
        List<AdvisorRecommendation> recommendations)
    {
        if (context.GameSession is not null)
        {
            findings.Add(new AdvisorFinding(
                Title: "Game session",
                Detail: context.GameSession.Explanation,
                Severity: context.GameSession.State == GameSessionState.LibraryMatch ? AdvisorSeverity.Info : AdvisorSeverity.Watch,
                Source: "GameSessionAnalyzer"));
        }

        if (context.GameBoostPlan is not null)
        {
            recommendations.Add(new AdvisorRecommendation(
                Title: "Game Boost plan",
                Detail: context.GameBoostPlan.Summary,
                Severity: context.GameBoostPlan.State == GameBoostPlanState.Ready ? AdvisorSeverity.Info : AdvisorSeverity.Watch,
                NextVerification: "Confirm game session and restore path before real execution."));
        }
    }

    private static void AddMonitorFindings(
        AdvisorReportContext context,
        List<AdvisorFinding> findings,
        List<AdvisorRecommendation> recommendations)
    {
        if (context.MonitorSnapshot is null)
        {
            return;
        }

        findings.Add(new AdvisorFinding(
            Title: "AETHER overhead",
            Detail: $"AETHER CPU estimate {context.MonitorSnapshot.AppCpuPercent:0.00}%, memory {context.MonitorSnapshot.AppMemoryMb} MB.",
            Severity: context.MonitorSnapshot.AppCpuPercent <= 1 && context.MonitorSnapshot.AppMemoryMb <= 150 ? AdvisorSeverity.Info : AdvisorSeverity.Watch,
            Source: "LowOverheadMonitor"));
    }

    private static void AddGpuFindings(
        AdvisorReportContext context,
        List<AdvisorFinding> findings,
        List<AdvisorRecommendation> recommendations)
    {
        if (context.GpuReport is null)
        {
            return;
        }

        findings.Add(new AdvisorFinding(
            Title: "GPU intelligence",
            Detail: $"{context.GpuReport.Name} / {context.GpuReport.Vendor} / telemetry {context.GpuReport.TelemetryAvailability}.",
            Severity: context.GpuReport.DriverWriteActionsEnabled ? AdvisorSeverity.Risk : AdvisorSeverity.Info,
            Source: "GpuIntelligenceAnalyzer"));
    }

    private static void AddOptimizationFindings(
        AdvisorReportContext context,
        List<AdvisorFinding> findings,
        List<AdvisorRecommendation> recommendations)
    {
        if (context.DryRunReport is not null)
        {
            findings.Add(new AdvisorFinding(
                Title: "Optimization Dry Run",
                Detail: context.DryRunReport.Summary,
                Severity: AdvisorSeverity.Info,
                Source: "OptimizationDryRunEngine"));
        }

        if (context.ExecutionReport is not null)
        {
            recommendations.Add(new AdvisorRecommendation(
                Title: "Execution safety",
                Detail: context.ExecutionReport.Summary,
                Severity: AdvisorSeverity.Info,
                NextVerification: "Real execution remains blocked until Windows validation."));
        }
    }

    private static string CreateSummary(
        AdvisorReportContext context,
        IReadOnlyList<AdvisorFinding> findings,
        IReadOnlyList<AdvisorRecommendation> recommendations)
    {
        var score = context.PerformanceReport?.OverallScore.ToString() ?? "n/a";
        return $"Local advisor report generated with score {score}, {findings.Count} finding(s), and {recommendations.Count} recommendation(s). Private identifiers are redacted by default.";
    }
}
