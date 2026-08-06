using AetherSentinel.Core.Analysis;

namespace AetherSentinel.Core.Optimization;

public sealed record OptimizationRule(
    string Id,
    string Name,
    OptimizationRuleCategory Category,
    string TargetPlatform,
    string RequiredPrivilege,
    RiskLevel RiskLevel,
    string DetectionCondition,
    string Preview,
    string BackupMethod,
    string VerificationSignal,
    string RollbackMethod,
    bool RequiresUserConsent);

public sealed record OptimizationDryRunReport(
    DateTimeOffset GeneratedAt,
    IReadOnlyList<OptimizationRulePreview> Previews,
    int EligibleCount,
    int BlockedCount,
    string Summary);

public sealed record OptimizationRulePreview(
    OptimizationRule Rule,
    OptimizationRulePreviewState State,
    string Reason,
    string ExpectedImpact);

public enum OptimizationRuleCategory
{
    Dns,
    Startup,
    PowerPlan,
    BackgroundPressure,
    GameFocus,
    Cleanup,
    Privacy
}

public enum OptimizationRulePreviewState
{
    Eligible,
    Blocked,
    NeedsMoreData
}
