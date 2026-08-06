using AetherSentinel.Core.Analysis;

namespace AetherSentinel.Core.Optimization;

public static class OptimizationRuleCatalog
{
    public static IReadOnlyList<OptimizationRule> DefaultRules { get; } =
    [
        new(
            Id: "dns.switch.verified-provider",
            Name: "Switch to measured DNS candidate",
            Category: OptimizationRuleCategory.Dns,
            TargetPlatform: "Windows",
            RequiredPrivilege: "Administrator",
            RiskLevel: RiskLevel.Low,
            DetectionCondition: "A verified DNS candidate has lower measured latency and failure rate than the current resolver.",
            Preview: "Back up current DNS settings, apply the selected resolver, verify lookup stability, and keep rollback available.",
            BackupMethod: "Store adapter name and original DNS server list.",
            VerificationSignal: "DNS lookup succeeds and latency/failure rate do not regress.",
            RollbackMethod: "Restore the original DNS server list to the same adapter.",
            RequiresUserConsent: true),
        new(
            Id: "startup.disable-review",
            Name: "Disable reviewed startup item",
            Category: OptimizationRuleCategory.Startup,
            TargetPlatform: "Windows",
            RequiredPrivilege: "User",
            RiskLevel: RiskLevel.Low,
            DetectionCondition: "One or more startup items are marked medium or high impact.",
            Preview: "Disable only user-selected startup entries and keep their original command and location.",
            BackupMethod: "Persist startup item name, command, location, and user.",
            VerificationSignal: "Startup entry no longer runs automatically and can still be restored.",
            RollbackMethod: "Recreate or re-enable the original startup entry.",
            RequiresUserConsent: true),
        new(
            Id: "powerplan.session-high-performance",
            Name: "Switch game-session power plan",
            Category: OptimizationRuleCategory.PowerPlan,
            TargetPlatform: "Windows",
            RequiredPrivilege: "User",
            RiskLevel: RiskLevel.Low,
            DetectionCondition: "Active plan is not a high-performance candidate and a game session is detected.",
            Preview: "Switch to a user-approved performance plan only during the game session.",
            BackupMethod: "Store active power plan GUID before switching.",
            VerificationSignal: "Active power plan matches the selected performance plan.",
            RollbackMethod: "Restore the original active power plan GUID.",
            RequiresUserConsent: true),
        new(
            Id: "background.pressure-review",
            Name: "Review background pressure",
            Category: OptimizationRuleCategory.BackgroundPressure,
            TargetPlatform: "Windows",
            RequiredPrivilege: "User",
            RiskLevel: RiskLevel.Medium,
            DetectionCondition: "High-memory or known background pressure processes are active.",
            Preview: "Show candidate background processes for manual review; no process is closed automatically.",
            BackupMethod: "No write action in dry-run mode.",
            VerificationSignal: "User-approved action should reduce memory or CPU pressure.",
            RollbackMethod: "Manual relaunch or future session restore policy.",
            RequiresUserConsent: true),
        new(
            Id: "cleanup.temp-preview",
            Name: "Preview temporary cleanup",
            Category: OptimizationRuleCategory.Cleanup,
            TargetPlatform: "Windows",
            RequiredPrivilege: "User",
            RiskLevel: RiskLevel.Low,
            DetectionCondition: "Storage pressure or cleanup request is present.",
            Preview: "Estimate removable temporary files before deleting anything.",
            BackupMethod: "Deletion list and excluded paths are recorded before execution.",
            VerificationSignal: "Free space increases and excluded paths remain untouched.",
            RollbackMethod: "Only reversible or low-risk temporary locations are eligible.",
            RequiresUserConsent: true),
        new(
            Id: "gamefocus.notifications",
            Name: "Enable game focus notification policy",
            Category: OptimizationRuleCategory.GameFocus,
            TargetPlatform: "Windows",
            RequiredPrivilege: "User",
            RiskLevel: RiskLevel.Low,
            DetectionCondition: "A game session is detected and notifications may interrupt the session.",
            Preview: "Enable a reversible focus policy for the duration of the game session.",
            BackupMethod: "Store original notification/focus state.",
            VerificationSignal: "Focus policy state is active during session.",
            RollbackMethod: "Restore original notification/focus state after session.",
            RequiresUserConsent: true)
    ];
}
