using AetherSentinel.Core.Gaming;
using AetherSentinel.Core.Network;
using AetherSentinel.Core.Scanning;

namespace AetherSentinel.Core.Optimization;

public interface IOptimizationDryRunEngine
{
    OptimizationDryRunReport Generate(
        SystemSnapshot? snapshot,
        NetworkDiagnosticsReport? networkReport,
        GameSessionAnalysis? gameSession);
}

public sealed class OptimizationDryRunEngine : IOptimizationDryRunEngine
{
    public OptimizationDryRunReport Generate(
        SystemSnapshot? snapshot,
        NetworkDiagnosticsReport? networkReport,
        GameSessionAnalysis? gameSession)
    {
        var previews = OptimizationRuleCatalog.DefaultRules
            .Select(rule => PreviewRule(rule, snapshot, networkReport, gameSession))
            .ToArray();
        var eligible = previews.Count(preview => preview.State == OptimizationRulePreviewState.Eligible);
        var blocked = previews.Count(preview => preview.State == OptimizationRulePreviewState.Blocked);

        return new OptimizationDryRunReport(
            GeneratedAt: DateTimeOffset.Now,
            Previews: previews,
            EligibleCount: eligible,
            BlockedCount: blocked,
            Summary: $"{eligible} rule(s) eligible for future user-approved execution; {blocked} blocked by missing backup, missing data, or safety boundaries.");
    }

    private static OptimizationRulePreview PreviewRule(
        OptimizationRule rule,
        SystemSnapshot? snapshot,
        NetworkDiagnosticsReport? networkReport,
        GameSessionAnalysis? gameSession)
    {
        if (snapshot is null)
        {
            return NeedsMoreData(rule, "Run a read-only scan before previewing optimization rules.");
        }

        return rule.Category switch
        {
            OptimizationRuleCategory.Dns => PreviewDns(rule, networkReport),
            OptimizationRuleCategory.Startup => PreviewStartup(rule, snapshot),
            OptimizationRuleCategory.PowerPlan => PreviewPowerPlan(rule, snapshot, gameSession),
            OptimizationRuleCategory.BackgroundPressure => PreviewBackgroundPressure(rule, snapshot),
            OptimizationRuleCategory.Cleanup => PreviewCleanup(rule, snapshot),
            OptimizationRuleCategory.GameFocus => PreviewGameFocus(rule, gameSession),
            _ => NeedsMoreData(rule, "This rule category needs more implementation context.")
        };
    }

    private static OptimizationRulePreview PreviewDns(
        OptimizationRule rule,
        NetworkDiagnosticsReport? networkReport)
    {
        var bestDns = networkReport?.DnsBenchmarkResults
            .Where(result => result.FailureRatePercent < 25)
            .OrderBy(result => result.AverageLatencyMs)
            .FirstOrDefault();

        return bestDns is null
            ? NeedsMoreData(rule, "Run quick network diagnostics before DNS switching can be previewed.")
            : Eligible(rule, $"Best measured candidate is {bestDns.Resolver.Name} at {bestDns.AverageLatencyMs:0.0} ms.", "Potential latency and resolver stability improvement.");
    }

    private static OptimizationRulePreview PreviewStartup(
        OptimizationRule rule,
        SystemSnapshot snapshot)
    {
        var reviewCount = snapshot.StartupItems.Count(item => item.ImpactLevel is StartupImpactLevel.Medium or StartupImpactLevel.High);
        return reviewCount <= 0
            ? NeedsMoreData(rule, "No medium or high impact startup item was detected.")
            : Eligible(rule, $"{reviewCount} startup item(s) need review.", "Lower startup pressure and reduce background launch load.");
    }

    private static OptimizationRulePreview PreviewPowerPlan(
        OptimizationRule rule,
        SystemSnapshot snapshot,
        GameSessionAnalysis? gameSession)
    {
        if (gameSession?.State is not (GameSessionState.GameCandidate or GameSessionState.LibraryMatch))
        {
            return NeedsMoreData(rule, "A game session candidate is required before session power plan switching.");
        }

        return snapshot.PowerPlan.IsHighPerformanceCandidate
            ? NeedsMoreData(rule, "Current power plan already appears to be performance-oriented.")
            : Eligible(rule, $"Current power plan is {snapshot.PowerPlan.Name}.", "Improve game-session power behavior while preserving rollback.");
    }

    private static OptimizationRulePreview PreviewBackgroundPressure(
        OptimizationRule rule,
        SystemSnapshot snapshot)
    {
        var highPressure = snapshot.TopProcesses.Count(process => process.ImpactLevel == ProcessImpactLevel.High);
        return highPressure <= 0
            ? NeedsMoreData(rule, "No high-impact background pressure process was detected.")
            : Eligible(rule, $"{highPressure} high-impact process(es) found.", "Manual review may free memory or reduce background contention.");
    }

    private static OptimizationRulePreview PreviewCleanup(
        OptimizationRule rule,
        SystemSnapshot snapshot)
    {
        var pressuredDrive = snapshot.Hardware.Storage.FirstOrDefault(storage => storage.ActivePercent >= 80);
        return pressuredDrive is null
            ? NeedsMoreData(rule, "No storage pressure was detected.")
            : Eligible(rule, $"{pressuredDrive.Name} usage is {pressuredDrive.ActivePercent:0}%.", "Free space may improve game updates, shader caches, and creator workloads.");
    }

    private static OptimizationRulePreview PreviewGameFocus(
        OptimizationRule rule,
        GameSessionAnalysis? gameSession)
    {
        return gameSession?.State is GameSessionState.GameCandidate or GameSessionState.LibraryMatch
            ? Eligible(rule, gameSession.Explanation, "Reduce interruption risk during game sessions.")
            : NeedsMoreData(rule, "A confirmed game session candidate is required.");
    }

    private static OptimizationRulePreview Eligible(
        OptimizationRule rule,
        string reason,
        string expectedImpact)
    {
        return new OptimizationRulePreview(rule, OptimizationRulePreviewState.Eligible, reason, expectedImpact);
    }

    private static OptimizationRulePreview NeedsMoreData(
        OptimizationRule rule,
        string reason)
    {
        return new OptimizationRulePreview(rule, OptimizationRulePreviewState.NeedsMoreData, reason, "No action will be executed.");
    }
}
