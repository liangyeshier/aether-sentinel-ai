using AetherSentinel.Core.Scanning;

namespace AetherSentinel.Core.Analysis;

public sealed record PerformanceAnalysisReport(
    DateTimeOffset AnalyzedAt,
    int OverallScore,
    OptimizationPotentialLevel OptimizationPotential,
    IReadOnlyList<ScoreFactor> Factors,
    IReadOnlyList<OptimizationRecommendation> Recommendations);

public sealed record ScoreFactor(
    string Key,
    string Title,
    string Detail,
    int Score,
    ScoreSeverity Severity,
    string Source);

public sealed record OptimizationRecommendation(
    string Title,
    string Detail,
    RecommendationCategory Category,
    RiskLevel RiskLevel,
    string VerificationSignal,
    string RollbackRequirement);

public enum OptimizationPotentialLevel
{
    Low,
    Medium,
    High
}

public enum ScoreSeverity
{
    Good,
    Watch,
    Risk
}

public enum RecommendationCategory
{
    Memory,
    Storage,
    Process,
    Dns,
    Network,
    System
}

public enum RiskLevel
{
    ReadOnly,
    Low,
    Medium,
    High
}
