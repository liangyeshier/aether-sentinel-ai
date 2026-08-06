using AetherSentinel.Core.Scanning;

namespace AetherSentinel.Core.Gaming;

public sealed record GameLibraryEntry(
    string Id,
    string DisplayName,
    string ExecutablePath,
    GameLibrarySource Source,
    DateTimeOffset AddedAt,
    bool IsEnabled);

public sealed record GameSessionAnalysis(
    DateTimeOffset AnalyzedAt,
    GameSessionState State,
    GameLibraryEntry? LibraryMatch,
    GameProcessCandidateSnapshot? PrimaryCandidate,
    IReadOnlyList<GameProcessCandidateSnapshot> RelatedCandidates,
    string Explanation,
    double Confidence,
    GameSessionSafetyBoundary SafetyBoundary);

public sealed record GameSessionSafetyBoundary(
    bool AllowsInjection,
    bool AllowsMemoryModification,
    bool AllowsGameFileModification,
    bool AllowsAntiCheatBypass,
    string Policy);

public enum GameLibrarySource
{
    ManualExe,
    Shortcut,
    Steam,
    Epic,
    BattleNet,
    Riot,
    WeGame,
    Xbox,
    Imported
}

public enum GameSessionState
{
    NotDetected,
    LauncherCandidate,
    GameCandidate,
    LibraryMatch,
    NeedsConfirmation
}
