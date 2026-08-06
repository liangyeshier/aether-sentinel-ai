namespace AetherSentinel.Core.Gaming;

public interface IGameBoostPlanner
{
    GameBoostPlan CreatePlan(GameSessionAnalysis? session, GameBoostMode mode);
}

public sealed class GameBoostPlanner : IGameBoostPlanner
{
    public GameBoostPlan CreatePlan(GameSessionAnalysis? session, GameBoostMode mode)
    {
        var safety = session?.SafetyBoundary ?? new GameSessionSafetyBoundary(
            AllowsInjection: false,
            AllowsMemoryModification: false,
            AllowsGameFileModification: false,
            AllowsAntiCheatBypass: false,
            Policy: "Game Boost Mode requires a read-only game session candidate before any future execution.");

        if (session?.State is not (GameSessionState.GameCandidate or GameSessionState.LibraryMatch))
        {
            return new GameBoostPlan(
                GeneratedAt: DateTimeOffset.Now,
                Mode: mode,
                State: GameBoostPlanState.NeedsGameSession,
                Actions:
                [
                    new GameBoostActionPreview(
                        Name: "Detect game session",
                        Category: GameBoostActionCategory.Restore,
                        State: GameBoostActionState.Blocked,
                        Reason: "No confirmed game session candidate is available.",
                        SafetyNote: safety.Policy)
                ],
                Summary: "Game Boost plan needs a game candidate or library match.",
                SafetyBoundary: safety);
        }

        var actions = new List<GameBoostActionPreview>
        {
            new(
                Name: "Review background pressure",
                Category: GameBoostActionCategory.BackgroundPressure,
                State: GameBoostActionState.PreviewOnly,
                Reason: "Show background processes that may contend with the game.",
                SafetyNote: "No process will be closed automatically."),
            new(
                Name: "Prepare game priority policy",
                Category: GameBoostActionCategory.ProcessPriority,
                State: GameBoostActionState.EligibleForFutureExecution,
                Reason: "Future Windows executor may raise the confirmed game process priority within safe limits.",
                SafetyNote: "Launchers and anti-cheat processes remain protected."),
            new(
                Name: "Prepare I/O priority policy",
                Category: GameBoostActionCategory.IoPriority,
                State: mode == GameBoostMode.Competitive ? GameBoostActionState.EligibleForFutureExecution : GameBoostActionState.PreviewOnly,
                Reason: "Future Windows executor may reduce background I/O pressure during a confirmed session.",
                SafetyNote: "No disk or game file modification is allowed."),
            new(
                Name: "Prepare session power plan",
                Category: GameBoostActionCategory.PowerPlan,
                State: GameBoostActionState.EligibleForFutureExecution,
                Reason: "Power policy can be scoped to the game session with original plan restore.",
                SafetyNote: "Original power plan must be restored when session ends."),
            new(
                Name: "Prepare notification focus",
                Category: GameBoostActionCategory.NotificationFocus,
                State: GameBoostActionState.EligibleForFutureExecution,
                Reason: "Reduce interruption risk during gameplay.",
                SafetyNote: "Original focus state must be restored after session."),
            new(
                Name: "Prepare session restore",
                Category: GameBoostActionCategory.Restore,
                State: GameBoostActionState.PreviewOnly,
                Reason: "Every session-level change needs an exit and crash-recovery restore path.",
                SafetyNote: "Restore is mandatory before real Game Boost execution.")
        };

        return new GameBoostPlan(
            GeneratedAt: DateTimeOffset.Now,
            Mode: mode,
            State: GameBoostPlanState.Ready,
            Actions: actions,
            Summary: $"{mode} Game Boost plan generated for {session.PrimaryCandidate?.Name ?? session.LibraryMatch?.DisplayName}. Real execution remains disabled.",
            SafetyBoundary: safety);
    }
}
