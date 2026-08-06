using AetherSentinel.Core.Scanning;

namespace AetherSentinel.Core.Gaming;

public interface IGameSessionAnalyzer
{
    GameSessionAnalysis Analyze(
        SystemSnapshot snapshot,
        IReadOnlyList<GameLibraryEntry> libraryEntries);
}

public sealed class GameSessionAnalyzer : IGameSessionAnalyzer
{
    private static readonly GameSessionSafetyBoundary DefaultSafetyBoundary = new(
        AllowsInjection: false,
        AllowsMemoryModification: false,
        AllowsGameFileModification: false,
        AllowsAntiCheatBypass: false,
        Policy: "Read-only game session detection only. No injection, no memory modification, no game file modification, and no anti-cheat bypass.");

    public GameSessionAnalysis Analyze(
        SystemSnapshot snapshot,
        IReadOnlyList<GameLibraryEntry> libraryEntries)
    {
        var enabledEntries = libraryEntries
            .Where(entry => entry.IsEnabled)
            .ToArray();
        var candidates = snapshot.GameProcessCandidates
            .OrderByDescending(candidate => candidate.Confidence)
            .ToArray();
        var match = FindLibraryMatch(enabledEntries, candidates);

        if (match.LibraryEntry is not null && match.Candidate is not null)
        {
            return new GameSessionAnalysis(
                AnalyzedAt: DateTimeOffset.Now,
                State: GameSessionState.LibraryMatch,
                LibraryMatch: match.LibraryEntry,
                PrimaryCandidate: match.Candidate,
                RelatedCandidates: candidates,
                Explanation: $"Matched running process '{match.Candidate.Name}' with library entry '{match.LibraryEntry.DisplayName}'.",
                Confidence: Math.Max(match.Candidate.Confidence, 0.8),
                SafetyBoundary: DefaultSafetyBoundary);
        }

        var gameCandidate = candidates.FirstOrDefault(candidate => candidate.Role == GameProcessRole.Game);
        if (gameCandidate is not null)
        {
            return new GameSessionAnalysis(
                AnalyzedAt: DateTimeOffset.Now,
                State: GameSessionState.GameCandidate,
                LibraryMatch: null,
                PrimaryCandidate: gameCandidate,
                RelatedCandidates: candidates,
                Explanation: $"Detected a possible game process: {gameCandidate.Name}. User confirmation is required before future optimization.",
                Confidence: gameCandidate.Confidence,
                SafetyBoundary: DefaultSafetyBoundary);
        }

        var launcherCandidate = candidates.FirstOrDefault(candidate => candidate.Role == GameProcessRole.Launcher);
        if (launcherCandidate is not null)
        {
            return new GameSessionAnalysis(
                AnalyzedAt: DateTimeOffset.Now,
                State: GameSessionState.LauncherCandidate,
                LibraryMatch: null,
                PrimaryCandidate: launcherCandidate,
                RelatedCandidates: candidates,
                Explanation: $"Detected a launcher candidate: {launcherCandidate.Name}. AETHER will not treat launchers as the game body automatically.",
                Confidence: launcherCandidate.Confidence,
                SafetyBoundary: DefaultSafetyBoundary);
        }

        return new GameSessionAnalysis(
            AnalyzedAt: DateTimeOffset.Now,
            State: enabledEntries.Length > 0 ? GameSessionState.NeedsConfirmation : GameSessionState.NotDetected,
            LibraryMatch: null,
            PrimaryCandidate: null,
            RelatedCandidates: candidates,
            Explanation: enabledEntries.Length > 0
                ? "Game library entries exist, but no matching running game process was detected."
                : "No enabled game library entry or running game candidate was detected.",
            Confidence: enabledEntries.Length > 0 ? 0.35 : 0,
            SafetyBoundary: DefaultSafetyBoundary);
    }

    private static (GameLibraryEntry? LibraryEntry, GameProcessCandidateSnapshot? Candidate) FindLibraryMatch(
        IReadOnlyList<GameLibraryEntry> entries,
        IReadOnlyList<GameProcessCandidateSnapshot> candidates)
    {
        foreach (var entry in entries)
        {
            var executableName = Path.GetFileNameWithoutExtension(entry.ExecutablePath);
            if (string.IsNullOrWhiteSpace(executableName))
            {
                continue;
            }

            var candidate = candidates.FirstOrDefault(value =>
                value.Name.Equals(executableName, StringComparison.OrdinalIgnoreCase) ||
                value.Name.Contains(executableName, StringComparison.OrdinalIgnoreCase) ||
                executableName.Contains(value.Name, StringComparison.OrdinalIgnoreCase));

            if (candidate is not null)
            {
                return (entry, candidate);
            }
        }

        return (null, null);
    }
}
