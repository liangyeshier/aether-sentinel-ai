namespace AetherSentinel.Core.Optimization;

public interface IOptimizationExecutionEngine
{
    OptimizationExecutionReport Execute(OptimizationExecutionRequest request);
}

public sealed class OptimizationExecutionEngine : IOptimizationExecutionEngine
{
    public OptimizationExecutionReport Execute(OptimizationExecutionRequest request)
    {
        var results = new List<OptimizationExecutionResult>();
        var restorePoints = new List<OptimizationRestorePoint>();

        foreach (var preview in request.DryRunReport.Previews)
        {
            var result = ExecutePreview(preview, request);
            results.Add(result);

            if (result.Status is OptimizationExecutionStatus.Simulated or OptimizationExecutionStatus.Succeeded)
            {
                restorePoints.Add(new OptimizationRestorePoint(
                    RuleId: preview.Rule.Id,
                    BackupMethod: preview.Rule.BackupMethod,
                    RollbackMethod: preview.Rule.RollbackMethod,
                    CreatedAt: DateTimeOffset.Now,
                    State: result.Status == OptimizationExecutionStatus.Simulated
                        ? "Simulated restore point; no system state captured."
                        : "Restore point captured."));
            }
        }

        var blocked = results.Count(result => result.Status == OptimizationExecutionStatus.Blocked);
        var simulated = results.Count(result => result.Status == OptimizationExecutionStatus.Simulated);
        var succeeded = results.Count(result => result.Status == OptimizationExecutionStatus.Succeeded);

        return new OptimizationExecutionReport(
            ExecutedAt: DateTimeOffset.Now,
            Mode: request.Mode,
            Results: results,
            RestorePoints: restorePoints,
            Summary: $"{succeeded} succeeded, {simulated} simulated, {blocked} blocked. Real system writes require Windows validation and explicit consent.");
    }

    private static OptimizationExecutionResult ExecutePreview(
        OptimizationRulePreview preview,
        OptimizationExecutionRequest request)
    {
        if (preview.State != OptimizationRulePreviewState.Eligible)
        {
            return new OptimizationExecutionResult(
                RuleId: preview.Rule.Id,
                RuleName: preview.Rule.Name,
                Status: OptimizationExecutionStatus.Blocked,
                Message: $"Rule is not eligible: {preview.Reason}",
                VerificationResult: "Not executed.",
                RollbackState: "No rollback needed.");
        }

        if (string.IsNullOrWhiteSpace(request.UserConsentToken))
        {
            return new OptimizationExecutionResult(
                RuleId: preview.Rule.Id,
                RuleName: preview.Rule.Name,
                Status: OptimizationExecutionStatus.Blocked,
                Message: "Missing user consent token.",
                VerificationResult: "Not executed.",
                RollbackState: "No rollback needed.");
        }

        if (request.Mode == OptimizationExecutionMode.Simulated)
        {
            return new OptimizationExecutionResult(
                RuleId: preview.Rule.Id,
                RuleName: preview.Rule.Name,
                Status: OptimizationExecutionStatus.Simulated,
                Message: preview.Rule.Preview,
                VerificationResult: $"Would verify: {preview.Rule.VerificationSignal}",
                RollbackState: $"Would rollback through: {preview.Rule.RollbackMethod}");
        }

        if (!request.AllowSystemChanges)
        {
            return new OptimizationExecutionResult(
                RuleId: preview.Rule.Id,
                RuleName: preview.Rule.Name,
                Status: OptimizationExecutionStatus.Blocked,
                Message: "System changes are disabled by safety gate.",
                VerificationResult: "Not executed.",
                RollbackState: "No rollback needed.");
        }

        return new OptimizationExecutionResult(
            RuleId: preview.Rule.Id,
            RuleName: preview.Rule.Name,
            Status: OptimizationExecutionStatus.Blocked,
            Message: "Real apply executor is not enabled until Windows validation is complete.",
            VerificationResult: "Not executed.",
            RollbackState: "No rollback needed.");
    }
}
