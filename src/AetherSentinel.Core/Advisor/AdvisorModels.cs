namespace AetherSentinel.Core.Advisor;

public sealed record AdvisorReport(
    string Id,
    DateTimeOffset GeneratedAt,
    string Summary,
    IReadOnlyList<AdvisorFinding> Findings,
    IReadOnlyList<AdvisorRecommendation> Recommendations,
    bool PrivacyRedactionApplied);

public sealed record AdvisorFinding(
    string Title,
    string Detail,
    AdvisorSeverity Severity,
    string Source);

public sealed record AdvisorRecommendation(
    string Title,
    string Detail,
    AdvisorSeverity Severity,
    string NextVerification);

public sealed record AdvisorHistoryRecord(
    string Id,
    DateTimeOffset CreatedAt,
    string Summary,
    int FindingCount,
    int RecommendationCount,
    bool PrivacyRedactionApplied);

public enum AdvisorSeverity
{
    Info,
    Watch,
    Risk
}
