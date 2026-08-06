namespace AetherSentinel.Core.Gaming;

public sealed record GameBoostPlan(
    DateTimeOffset GeneratedAt,
    GameBoostMode Mode,
    GameBoostPlanState State,
    IReadOnlyList<GameBoostActionPreview> Actions,
    string Summary,
    GameSessionSafetyBoundary SafetyBoundary);

public sealed record GameBoostActionPreview(
    string Name,
    GameBoostActionCategory Category,
    GameBoostActionState State,
    string Reason,
    string SafetyNote);

public enum GameBoostMode
{
    Balanced,
    Competitive,
    Custom
}

public enum GameBoostPlanState
{
    Ready,
    NeedsGameSession,
    BlockedBySafety
}

public enum GameBoostActionCategory
{
    BackgroundPressure,
    ProcessPriority,
    IoPriority,
    PowerPlan,
    NotificationFocus,
    Restore
}

public enum GameBoostActionState
{
    PreviewOnly,
    EligibleForFutureExecution,
    Blocked
}
