namespace AetherSentinel.Core.Optimization;

public sealed record OptimizationExecutionRequest(
    OptimizationDryRunReport DryRunReport,
    OptimizationExecutionMode Mode,
    bool AllowSystemChanges,
    string UserConsentToken);

public sealed record OptimizationExecutionReport(
    DateTimeOffset ExecutedAt,
    OptimizationExecutionMode Mode,
    IReadOnlyList<OptimizationExecutionResult> Results,
    IReadOnlyList<OptimizationRestorePoint> RestorePoints,
    string Summary);

public sealed record OptimizationExecutionResult(
    string RuleId,
    string RuleName,
    OptimizationExecutionStatus Status,
    string Message,
    string VerificationResult,
    string RollbackState);

public sealed record OptimizationRestorePoint(
    string RuleId,
    string BackupMethod,
    string RollbackMethod,
    DateTimeOffset CreatedAt,
    string State);

public enum OptimizationExecutionMode
{
    Simulated,
    Apply
}

public enum OptimizationExecutionStatus
{
    Simulated,
    Blocked,
    Succeeded,
    Failed
}
