using AetherSentinel.Core.Scanning;

namespace AetherSentinel.Core.Analysis;

public interface IPerformanceAnalyzer
{
    PerformanceAnalysisReport Analyze(SystemSnapshot snapshot);
}

public sealed class PerformanceAnalyzer : IPerformanceAnalyzer
{
    public PerformanceAnalysisReport Analyze(SystemSnapshot snapshot)
    {
        var factors = new List<ScoreFactor>
        {
            AnalyzeMemory(snapshot.Hardware),
            AnalyzeStorage(snapshot.Hardware),
            AnalyzeProcesses(snapshot.TopProcesses),
            AnalyzeDns(snapshot.Network),
            AnalyzeNetwork(snapshot.Network)
        };

        var recommendations = CreateRecommendations(factors, snapshot);
        var overallScore = CalculateOverallScore(factors);

        return new PerformanceAnalysisReport(
            AnalyzedAt: DateTimeOffset.Now,
            OverallScore: overallScore,
            OptimizationPotential: CalculateOptimizationPotential(overallScore, factors),
            Factors: factors,
            Recommendations: recommendations);
    }

    private static ScoreFactor AnalyzeMemory(HardwareSnapshot hardware)
    {
        if (hardware.MemoryTotalMb <= 0)
        {
            return new ScoreFactor(
                Key: "memory",
                Title: "Memory",
                Detail: "Memory usage is unavailable.",
                Score: 70,
                Severity: ScoreSeverity.Watch,
                Source: "HardwareSnapshot");
        }

        var percent = (double)hardware.MemoryUsedMb / hardware.MemoryTotalMb * 100;
        return percent switch
        {
            >= 90 => CreateFactor("memory", "Memory", $"Memory pressure is high at {percent:0}%.", 45, ScoreSeverity.Risk, "HardwareSnapshot"),
            >= 75 => CreateFactor("memory", "Memory", $"Memory usage is elevated at {percent:0}%.", 68, ScoreSeverity.Watch, "HardwareSnapshot"),
            _ => CreateFactor("memory", "Memory", $"Memory usage is healthy at {percent:0}%.", 92, ScoreSeverity.Good, "HardwareSnapshot")
        };
    }

    private static ScoreFactor AnalyzeStorage(HardwareSnapshot hardware)
    {
        var primaryStorage = hardware.Storage.FirstOrDefault();
        if (primaryStorage is null)
        {
            return CreateFactor("storage", "Storage", "Storage information is unavailable.", 70, ScoreSeverity.Watch, "DriveInfo");
        }

        return primaryStorage.ActivePercent switch
        {
            >= 90 => CreateFactor("storage", "Storage", $"Primary storage usage is high at {primaryStorage.ActivePercent:0}%.", 42, ScoreSeverity.Risk, "DriveInfo"),
            >= 80 => CreateFactor("storage", "Storage", $"Primary storage usage needs attention at {primaryStorage.ActivePercent:0}%.", 70, ScoreSeverity.Watch, "DriveInfo"),
            _ => CreateFactor("storage", "Storage", $"Primary storage has {primaryStorage.FreeGb} GB free.", 94, ScoreSeverity.Good, "DriveInfo")
        };
    }

    private static ScoreFactor AnalyzeProcesses(IReadOnlyList<ProcessSnapshot> processes)
    {
        var highImpactCount = processes.Count(process => process.ImpactLevel == ProcessImpactLevel.High);
        if (highImpactCount >= 3)
        {
            return CreateFactor("process", "Background Load", $"{highImpactCount} high-memory processes are active.", 58, ScoreSeverity.Watch, "Process.WorkingSet64");
        }

        if (highImpactCount > 0)
        {
            var noun = highImpactCount == 1 ? "process is" : "processes are";
            return CreateFactor("process", "Background Load", $"{highImpactCount} high-memory {noun} active.", 76, ScoreSeverity.Watch, "Process.WorkingSet64");
        }

        return CreateFactor("process", "Background Load", "No high-memory process pressure was detected.", 92, ScoreSeverity.Good, "Process.WorkingSet64");
    }

    private static ScoreFactor AnalyzeDns(NetworkSnapshot network)
    {
        if (network.CurrentDnsServers.Count == 0)
        {
            return CreateFactor("dns", "DNS", "No active DNS resolver was detected.", 62, ScoreSeverity.Watch, "NetworkSnapshot");
        }

        return CreateFactor("dns", "DNS", $"Detected {network.CurrentDnsServers.Count} active DNS resolver(s).", 86, ScoreSeverity.Good, "NetworkSnapshot");
    }

    private static ScoreFactor AnalyzeNetwork(NetworkSnapshot network)
    {
        if (network.PrimaryInterfaceName is "Unknown" or "Not scanned")
        {
            return CreateFactor("network", "Network", "Active network interface was not detected.", 65, ScoreSeverity.Watch, "NetworkInterface");
        }

        return CreateFactor("network", "Network", $"Active interface: {network.PrimaryInterfaceName} / {network.ConnectionType}.", 88, ScoreSeverity.Good, "NetworkInterface");
    }

    private static IReadOnlyList<OptimizationRecommendation> CreateRecommendations(
        IReadOnlyList<ScoreFactor> factors,
        SystemSnapshot snapshot)
    {
        var recommendations = new List<OptimizationRecommendation>();

        foreach (var factor in factors.Where(factor => factor.Severity != ScoreSeverity.Good))
        {
            recommendations.Add(factor.Key switch
            {
                "memory" => new OptimizationRecommendation(
                    Title: "Review memory pressure",
                    Detail: "Check high-memory background processes before gaming or creator workloads.",
                    Category: RecommendationCategory.Memory,
                    RiskLevel: RiskLevel.ReadOnly,
                    VerificationSignal: "Memory usage should decrease after user-approved action.",
                    RollbackRequirement: "No system change in read-only mode."),
                "storage" => new OptimizationRecommendation(
                    Title: "Review storage pressure",
                    Detail: "Inspect free space before large game updates, cache generation, or video exports.",
                    Category: RecommendationCategory.Storage,
                    RiskLevel: RiskLevel.ReadOnly,
                    VerificationSignal: "Free space and storage pressure should improve after cleanup.",
                    RollbackRequirement: "Future cleanup requires explicit file list and restore policy."),
                "process" => new OptimizationRecommendation(
                    Title: "Review background load",
                    Detail: "High-memory processes may reduce available headroom for games and creative tools.",
                    Category: RecommendationCategory.Process,
                    RiskLevel: RiskLevel.ReadOnly,
                    VerificationSignal: "Top process list should show lower memory pressure after user action.",
                    RollbackRequirement: "No process will be closed automatically."),
                "dns" => new OptimizationRecommendation(
                    Title: "Benchmark DNS",
                    Detail: "Compare the current resolver with verified DNS providers such as 360 Secure DNS before recommending a change.",
                    Category: RecommendationCategory.Dns,
                    RiskLevel: RiskLevel.ReadOnly,
                    VerificationSignal: "DNS latency, jitter, and failure rate should be measured first.",
                    RollbackRequirement: "Future DNS switching must back up original resolver settings."),
                _ => new OptimizationRecommendation(
                    Title: "Review network state",
                    Detail: "Network interface data should be reviewed before latency-sensitive workloads.",
                    Category: RecommendationCategory.Network,
                    RiskLevel: RiskLevel.ReadOnly,
                    VerificationSignal: "Network interface should remain stable.",
                    RollbackRequirement: "No network change in read-only mode.")
            });
        }

        if (recommendations.Count == 0)
        {
            recommendations.Add(new OptimizationRecommendation(
                Title: "System baseline looks healthy",
                Detail: "No immediate high-risk pressure was detected from the current read-only scan.",
                Category: RecommendationCategory.System,
                RiskLevel: RiskLevel.ReadOnly,
                VerificationSignal: "Continue comparing future scans against this baseline.",
                RollbackRequirement: "No system change in read-only mode."));
        }

        if (snapshot.Network.CurrentDnsServers.Count > 0)
        {
            recommendations.Add(new OptimizationRecommendation(
                Title: "Prepare DNS benchmark",
                Detail: "Current DNS is detected. Future Phase 03.3 can compare it against 360 Secure DNS without applying changes.",
                Category: RecommendationCategory.Dns,
                RiskLevel: RiskLevel.ReadOnly,
                VerificationSignal: "Compare average lookup latency, jitter, and failure rate.",
                RollbackRequirement: "DNS apply remains disabled until backup and rollback exist."));
        }

        return recommendations.Take(4).ToArray();
    }

    private static int CalculateOverallScore(IReadOnlyList<ScoreFactor> factors)
    {
        if (factors.Count == 0)
        {
            return 0;
        }

        return Math.Clamp((int)Math.Round(factors.Average(factor => factor.Score)), 0, 100);
    }

    private static OptimizationPotentialLevel CalculateOptimizationPotential(
        int overallScore,
        IReadOnlyList<ScoreFactor> factors)
    {
        if (overallScore < 70 || factors.Any(factor => factor.Severity == ScoreSeverity.Risk))
        {
            return OptimizationPotentialLevel.High;
        }

        if (overallScore < 85 || factors.Any(factor => factor.Severity == ScoreSeverity.Watch))
        {
            return OptimizationPotentialLevel.Medium;
        }

        return OptimizationPotentialLevel.Low;
    }

    private static ScoreFactor CreateFactor(
        string key,
        string title,
        string detail,
        int score,
        ScoreSeverity severity,
        string source)
    {
        return new ScoreFactor(key, title, detail, score, severity, source);
    }
}
