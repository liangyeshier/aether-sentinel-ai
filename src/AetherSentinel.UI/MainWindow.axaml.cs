using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading;
using AetherSentinel.Core.Analysis;
using AetherSentinel.Core.Gaming;
using AetherSentinel.Core.Gpu;
using AetherSentinel.Core.Monitoring;
using AetherSentinel.Core.Network;
using AetherSentinel.Core.Optimization;
using AetherSentinel.Core.Performance;
using AetherSentinel.Core.Scanning;
using AetherSentinel.Core.Toolkit;
using AetherSentinel.Platforms.Network;
using AetherSentinel.Platforms.Monitoring;
using AetherSentinel.Platforms.Scanning;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Interactivity;
using Avalonia.Layout;
using Avalonia.Media;
using Avalonia.Platform.Storage;

namespace AetherSentinel.UI;

public partial class MainWindow : Window
{
    private readonly ISystemScanner _systemScanner = new PlatformSystemScanner(new LocalPlatformSystemAdapter());
    private readonly IPerformanceAnalyzer _performanceAnalyzer = new PerformanceAnalyzer();
    private readonly IGpuIntelligenceAnalyzer _gpuIntelligenceAnalyzer = new GpuIntelligenceAnalyzer();
    private readonly INetworkDiagnosticsProvider _networkDiagnosticsProvider = new LocalNetworkDiagnosticsProvider();
    private readonly IGameSessionAnalyzer _gameSessionAnalyzer = new GameSessionAnalyzer();
    private readonly IGameBoostPlanner _gameBoostPlanner = new GameBoostPlanner();
    private readonly ILowOverheadMonitor _lowOverheadMonitor = new LocalLowOverheadMonitor();
    private readonly IOptimizationDryRunEngine _optimizationDryRunEngine = new OptimizationDryRunEngine();
    private readonly IOptimizationExecutionEngine _optimizationExecutionEngine = new OptimizationExecutionEngine();
    private readonly List<GameLibraryEntry> _gameLibrary = LoadGameLibrary().ToList();
    private SystemSnapshot? _lastSnapshot;
    private PerformanceAnalysisReport? _lastReport;
    private NetworkDiagnosticsReport? _lastNetworkDiagnostics;
    private GameSessionAnalysis? _lastGameSessionAnalysis;
    private GameBoostPlan? _lastGameBoostPlan;
    private MonitorSnapshot? _lastMonitorSnapshot;
    private OptimizationDryRunReport? _lastDryRunReport;
    private OptimizationExecutionReport? _lastExecutionReport;
    private GpuIntelligenceReport? _lastGpuReport;
    private TextBlock? _networkSpeedStatusText;
    private TextBlock? _gameSessionStatusText;
    private TextBlock? _monitorStatusText;
    private TextBlock? _dryRunStatusText;
    private string _currentLanguage = "zh-CN";
    private string _currentPage = "dashboard";

    public MainWindow()
    {
        InitializeComponent();
        SetLanguage("zh-CN");
    }

    private void OnChineseClicked(object? sender, RoutedEventArgs e)
    {
        SetLanguage("zh-CN");
    }

    private void OnEnglishClicked(object? sender, RoutedEventArgs e)
    {
        SetLanguage("en-US");
    }

    private void OnNavigateClicked(object? sender, RoutedEventArgs e)
    {
        if (sender is Button { Tag: string page })
        {
            NavigateTo(page);
        }
    }

    private async void OnScanClicked(object? sender, RoutedEventArgs e)
    {
        ScanButton.IsEnabled = false;
        ScanButton.Content = IsZh ? "扫描中" : "Scanning";
        CurrentStateBodyTitleText.Text = IsZh ? "正在执行只读扫描" : "Running read-only scan";
        CurrentStateBodyText.Text = IsZh
            ? "正在读取本机系统、存储、进程、网络接口和 DNS 配置。"
            : "Reading local OS, storage, process, network interface, and DNS configuration.";

        try
        {
            _lastSnapshot = await _systemScanner.CaptureAsync(
                new ScanRequest(
                    IncludeProcesses: true,
                    IncludeNetwork: true,
                    IncludeDns: true,
                    Budget: PerformanceBudgetPolicy.DefaultLowOverhead),
                CancellationToken.None);

            _lastReport = _performanceAnalyzer.Analyze(_lastSnapshot);
            _lastGpuReport = _gpuIntelligenceAnalyzer.Analyze(_lastSnapshot);
            ApplySnapshot(_lastSnapshot);
            NavigateTo(_currentPage);
        }
        catch (Exception exception)
        {
            CurrentStateBodyTitleText.Text = IsZh ? "扫描失败" : "Scan failed";
            CurrentStateBodyText.Text = exception.Message;
        }
        finally
        {
            ScanButton.IsEnabled = true;
            ScanButton.Content = IsZh ? "扫描" : "Scan";
        }
    }

    private async void OnNetworkSpeedTestClicked(object? sender, RoutedEventArgs e)
    {
        if (sender is Button startButton)
        {
            startButton.IsEnabled = false;
            startButton.Content = IsZh ? "测试中" : "Testing";
        }

        if (_networkSpeedStatusText is not null)
        {
            _networkSpeedStatusText.Text = IsZh
                ? "正在执行轻量 Ping/Jitter 与 DNS 基准测试，不进行下载测速。"
                : "Running quick Ping/Jitter and DNS benchmark without download traffic.";
        }

        try
        {
            _lastNetworkDiagnostics = await _networkDiagnosticsProvider.RunQuickDiagnosticsAsync(
                CreateNetworkDiagnosticsRequest(),
                CancellationToken.None);

            CurrentStateBodyTitleText.Text = IsZh ? "网络测速完成" : "Network test complete";
            CurrentStateBodyText.Text = FormatNetworkDiagnosticsSummary(_lastNetworkDiagnostics);
            NavigateTo("speed");
        }
        catch (Exception exception)
        {
            if (_networkSpeedStatusText is not null)
            {
                _networkSpeedStatusText.Text = exception.Message;
            }

            CurrentStateBodyTitleText.Text = IsZh ? "网络测速失败" : "Network test failed";
            CurrentStateBodyText.Text = exception.Message;
        }
        finally
        {
            if (sender is Button finalButton)
            {
                finalButton.IsEnabled = true;
                finalButton.Content = IsZh ? "开始轻量测速" : "Run Quick Test";
            }
        }
    }

    private async void OnAddGameClicked(object? sender, RoutedEventArgs e)
    {
        var files = await StorageProvider.OpenFilePickerAsync(new FilePickerOpenOptions
        {
            Title = IsZh ? "选择游戏 EXE" : "Select Game EXE",
            AllowMultiple = false,
            FileTypeFilter =
            [
                new FilePickerFileType(IsZh ? "可执行文件" : "Executable")
                {
                    Patterns = OperatingSystem.IsWindows() ? ["*.exe"] : ["*"]
                }
            ]
        });

        var file = files.FirstOrDefault();
        if (file is null)
        {
            return;
        }

        var path = file.Path.LocalPath;
        var entry = new GameLibraryEntry(
            Id: Guid.NewGuid().ToString("N"),
            DisplayName: Path.GetFileNameWithoutExtension(path),
            ExecutablePath: path,
            Source: GameLibrarySource.ManualExe,
            AddedAt: DateTimeOffset.Now,
            IsEnabled: true);

        _gameLibrary.Add(entry);
        SaveGameLibrary(_gameLibrary);

        _gameSessionStatusText?.SetValue(
            TextBlock.TextProperty,
            IsZh ? $"已添加游戏：{entry.DisplayName}" : $"Added game: {entry.DisplayName}");

        NavigateTo("game");
    }

    private void OnAnalyzeGameSessionClicked(object? sender, RoutedEventArgs e)
    {
        if (_lastSnapshot is null)
        {
            _gameSessionStatusText?.SetValue(
                TextBlock.TextProperty,
                IsZh ? "请先执行只读扫描，再识别游戏会话。" : "Run a read-only scan before detecting game sessions.");
            return;
        }

        _lastGameSessionAnalysis = _gameSessionAnalyzer.Analyze(_lastSnapshot, _gameLibrary);
        GameSessionBodyTitleText.Text = TranslateGameSessionState(_lastGameSessionAnalysis.State);
        GameSessionBodyText.Text = IsZh
            ? TranslateGameSessionExplanation(_lastGameSessionAnalysis)
            : _lastGameSessionAnalysis.Explanation;

        NavigateTo("game");
    }

    private void OnGenerateGameBoostPlanClicked(object? sender, RoutedEventArgs e)
    {
        _lastGameSessionAnalysis ??= _lastSnapshot is null
            ? null
            : _gameSessionAnalyzer.Analyze(_lastSnapshot, _gameLibrary);

        _lastGameBoostPlan = _gameBoostPlanner.CreatePlan(
            _lastGameSessionAnalysis,
            GameBoostMode.Balanced);

        CurrentStateBodyTitleText.Text = IsZh ? "Game Boost 方案已生成" : "Game Boost plan generated";
        CurrentStateBodyText.Text = IsZh
            ? $"生成 {_lastGameBoostPlan.Actions.Count} 项预览动作；真实执行仍禁用。"
            : $"{_lastGameBoostPlan.Actions.Count} preview actions generated; real execution remains disabled.";
        NavigateTo("game");
    }

    private async void OnMonitorSampleClicked(object? sender, RoutedEventArgs e)
    {
        if (sender is Button sampleButton)
        {
            sampleButton.IsEnabled = false;
            sampleButton.Content = IsZh ? "采样中" : "Sampling";
        }

        if (_monitorStatusText is not null)
        {
            _monitorStatusText.Text = IsZh
                ? "正在执行一次轻量采样，不启动常驻后台监控。"
                : "Capturing one light sample without persistent background monitoring.";
        }

        try
        {
            _lastMonitorSnapshot = await _lowOverheadMonitor.CaptureOnceAsync(
                new MonitorRequest(
                    Mode: MonitorSamplingMode.Light,
                    SampleWindow: TimeSpan.FromMilliseconds(350),
                    TopProcessCount: 5,
                    Budget: PerformanceBudgetPolicy.DefaultLowOverhead),
                CancellationToken.None);

            CurrentStateBodyTitleText.Text = IsZh ? "性能采样完成" : "Performance sample complete";
            CurrentStateBodyText.Text = FormatMonitorSummary(_lastMonitorSnapshot);
            NavigateTo("monitor");
        }
        catch (Exception exception)
        {
            if (_monitorStatusText is not null)
            {
                _monitorStatusText.Text = exception.Message;
            }
        }
        finally
        {
            if (sender is Button finalButton)
            {
                finalButton.IsEnabled = true;
                finalButton.Content = IsZh ? "采样一次" : "Sample Once";
            }
        }
    }

    private void OnGenerateDryRunClicked(object? sender, RoutedEventArgs e)
    {
        _lastDryRunReport = _optimizationDryRunEngine.Generate(
            _lastSnapshot,
            _lastNetworkDiagnostics,
            _lastGameSessionAnalysis);

        CurrentStateBodyTitleText.Text = IsZh ? "Dry Run 已生成" : "Dry Run generated";
        CurrentStateBodyText.Text = FormatDryRunSummary(_lastDryRunReport);
        NavigateTo("optimization");
    }

    private void OnSimulateExecutionClicked(object? sender, RoutedEventArgs e)
    {
        _lastDryRunReport ??= _optimizationDryRunEngine.Generate(
            _lastSnapshot,
            _lastNetworkDiagnostics,
            _lastGameSessionAnalysis);

        _lastExecutionReport = _optimizationExecutionEngine.Execute(
            new OptimizationExecutionRequest(
                DryRunReport: _lastDryRunReport,
                Mode: OptimizationExecutionMode.Simulated,
                AllowSystemChanges: false,
                UserConsentToken: "local-ui-simulated-consent"));

        CurrentStateBodyTitleText.Text = IsZh ? "安全执行模拟完成" : "Safe execution simulation complete";
        CurrentStateBodyText.Text = FormatExecutionSummary(_lastExecutionReport);
        NavigateTo("optimization");
    }

    private void SetLanguage(string language)
    {
        _currentLanguage = language;
        var zh = language == "zh-CN";

        BrandSubtitleText.Text = zh ? "性能智能体" : "Performance Intelligence";

        NavDashboardButton.Content = zh ? "仪表盘" : "Dashboard";
        NavPcIntelligenceButton.Content = zh ? "电脑智能分析" : "PC Intelligence";
        NavGameOptimizationButton.Content = zh ? "游戏优化" : "Game Optimization";
        NavPerformanceMonitorButton.Content = zh ? "性能监控" : "Performance Monitor";
        NavOptimizationCenterButton.Content = zh ? "优化中心" : "Optimization Center";
        NavToolkitButton.Content = zh ? "工具中心" : "Toolkit Center";
        NavDnsOptimizationButton.Content = zh ? "DNS 优化" : "DNS Optimization";
        NavNetworkSpeedButton.Content = zh ? "网络测速" : "Network Speed Test";
        NavAiAdvisorButton.Content = zh ? "AI 顾问" : "AI Advisor";
        NavHistoryButton.Content = zh ? "历史记录" : "History";
        NavSettingsButton.Content = zh ? "设置" : "Settings";

        SystemGuardTitleText.Text = zh ? "系统守护" : "SYSTEM GUARD";
        LocalScanReadyText.Text = zh ? "本地扫描就绪" : "Local scan ready";

        PageTitleText.Text = zh ? "仪表盘" : "Dashboard";
        PageSubtitleText.Text = zh ? "面向 Windows PC 的 AI 原生性能守护系统" : "AI-native performance guardian for Windows PCs";
        ScanButton.Content = zh ? "扫描" : "Scan";
        OptimizeButton.Content = zh ? "AI 优化" : "AI Optimize";

        CpuStatusText.Text = zh ? "负载偏高" : "High load";
        GpuStatusText.Text = zh ? "温度关注" : "Thermal watch";
        MemoryStatusText.Text = zh ? "稳定" : "Stable";
        DiskStatusText.Text = zh ? "健康" : "Healthy";

        CoreTitleText.Text = zh ? "AI 性能核心" : "AI PERFORMANCE CORE";
        CoreDescriptionText.Text = zh
            ? "整体性能较强，但 GPU 压力和后台负载需要在游戏前重点关注。"
            : "Performance is strong, but GPU pressure and background load should be watched before gaming sessions.";
        GuardianLabelText.Text = zh ? "守护模式" : "GUARDIAN";
        ActiveStatusText.Text = zh ? "运行中" : "ACTIVE";
        SafeModeText.Text = zh ? "已启用安全模式" : "Verified Safe Mode";
        AnalyzeButton.Content = zh ? "立即分析" : "Analyze Now";
        ReportButton.Content = zh ? "查看报告" : "View Report";

        QueueTitleText.Text = zh ? "优化队列" : "Optimization Queue";
        QueueItemOneTitleText.Text = zh ? "降低启动压力" : "Reduce startup pressure";
        QueueItemOneBodyText.Text = zh ? "发现 3 个后台启动项需要复核。" : "3 background launch items require review.";
        QueueItemTwoTitleText.Text = zh ? "GPU 温度关注" : "GPU thermal watch";
        QueueItemTwoBodyText.Text = zh
            ? "检测到高负载。未经确认不会执行任何自动操作。"
            : "High load detected. No automatic action will run without confirmation.";
        QueueItemThreeTitleText.Text = zh ? "需要回滚方案" : "Rollback required";
        QueueItemThreeBodyText.Text = zh
            ? "每条优化规则都必须定义备份与回滚方式。"
            : "Every optimization rule must define backup and rollback before execution.";

        TipsTitleText.Text = zh ? "AI 提示" : "AI Tips";
        TipsBodyText.Text = zh
            ? "执行优化前，AETHER 会解释瓶颈、风险等级、备份方式、验证信号与回滚路径。"
            : "Before enabling optimization, AETHER will explain the bottleneck, risk level, backup method, verification signal, and rollback path.";

        FeedTitleText.Text = zh ? "Sentinel 动态" : "Sentinel Feed";
        FeedSubtitleText.Text = zh ? "实时洞察预览" : "Live insight preview";

        CurrentStateTitleText.Text = zh ? "当前状态" : "Current State";
        CurrentStateBodyTitleText.Text = zh ? "只读扫描已就绪" : "Ready for read-only scan";
        CurrentStateBodyText.Text = zh ? "当前原型不会执行任何系统更改。" : "No system changes are enabled in this prototype.";

        GameSessionTitleText.Text = zh ? "游戏会话" : "Game Session";
        GameSessionBodyTitleText.Text = zh ? "未检测到运行中的游戏" : "No active game detected";
        GameSessionBodyText.Text = zh
            ? "未来版本会在优化前识别全屏游戏状态。"
            : "Future versions will detect full-screen gaming sessions before optimization.";

        UpdateSystemTitleText.Text = zh ? "更新系统" : "Update System";
        UpdateSystemBodyTitleText.Text = zh ? "已预留" : "Reserved";
        UpdateSystemBodyText.Text = zh
            ? "版本检查、包验证与回滚仍处于架构预留状态。"
            : "Version checking, package verification, and rollback are architecture placeholders.";

        PhasePreviewTitleText.Text = zh ? "Phase 03 网络智能" : "Phase 03 Network Intelligence";
        PhasePreviewBodyText.Text = zh ? "网络测速、DNS Provider 与只读扫描模型已进入架构层。" : "Network speed testing, DNS providers, and read-only scan models are now in the architecture layer.";

        ZhButton.Classes.Set("active", zh);
        EnButton.Classes.Set("active", !zh);

        if (_lastSnapshot is not null)
        {
            _lastReport ??= _performanceAnalyzer.Analyze(_lastSnapshot);
            _lastGpuReport ??= _gpuIntelligenceAnalyzer.Analyze(_lastSnapshot);
            ApplySnapshot(_lastSnapshot);
        }

        NavigateTo(_currentPage);
    }

    private void ApplySnapshot(SystemSnapshot snapshot)
    {
        var report = _lastReport ?? _performanceAnalyzer.Analyze(snapshot);
        var memoryPercent = snapshot.Hardware.MemoryTotalMb <= 0
            ? 0
            : Math.Round((double)snapshot.Hardware.MemoryUsedMb / snapshot.Hardware.MemoryTotalMb * 100);
        var primaryStorage = snapshot.Hardware.Storage.FirstOrDefault();
        var diskPercent = primaryStorage?.ActivePercent ?? 0;
        var topProcess = snapshot.TopProcesses.FirstOrDefault();
        var dnsSummary = snapshot.Network.CurrentDnsServers.Count == 0
            ? (IsZh ? "未检测到 DNS" : "DNS not detected")
            : string.Join(", ", snapshot.Network.CurrentDnsServers.Take(2));

        CpuMetricText.Text = IsZh ? "只读" : "Read";
        CpuStatusText.Text = IsZh ? "CPU 已识别" : "CPU identified";
        CpuStatusText.Foreground = new SolidColorBrush(Color.Parse("#A8B3C2"));

        GpuMetricText.Text = IsZh ? "只读" : "Read";
        GpuStatusText.Text = CompactText(snapshot.Hardware.GpuName, 24);
        GpuStatusText.Foreground = new SolidColorBrush(Color.Parse("#A8B3C2"));

        MemoryMetricText.Text = memoryPercent <= 0 ? "--" : $"{memoryPercent:0}%";
        MemoryStatusText.Text = IsZh
            ? $"{FormatMb(snapshot.Hardware.MemoryUsedMb)} / {FormatMb(snapshot.Hardware.MemoryTotalMb)}"
            : $"{FormatMb(snapshot.Hardware.MemoryUsedMb)} / {FormatMb(snapshot.Hardware.MemoryTotalMb)}";
        MemoryStatusText.Foreground = new SolidColorBrush(memoryPercent >= 85 ? Color.Parse("#F2B84B") : Color.Parse("#2BD576"));

        DiskMetricText.Text = diskPercent <= 0 ? "--" : $"{diskPercent:0}%";
        DiskStatusText.Text = primaryStorage is null
            ? (IsZh ? "未检测到磁盘" : "No drive detected")
            : $"{primaryStorage.FreeGb} GB free";
        DiskStatusText.Foreground = new SolidColorBrush(diskPercent >= 85 ? Color.Parse("#F2B84B") : Color.Parse("#2BD576"));

        LocalScanReadyText.Text = IsZh
            ? $"已扫描 {snapshot.CapturedAt:HH:mm}"
            : $"Scanned {snapshot.CapturedAt:HH:mm}";

        CurrentStateBodyTitleText.Text = IsZh ? "只读扫描完成" : "Read-only scan complete";
        CurrentStateBodyText.Text = IsZh
            ? $"评分 {report.OverallScore}/100，优化潜力：{TranslatePotential(report.OptimizationPotential)}，DNS：{dnsSummary}"
            : $"Score {report.OverallScore}/100, potential: {TranslatePotential(report.OptimizationPotential)}, DNS: {dnsSummary}";

        PerformanceScoreText.Text = $"{report.OverallScore} / 100";

        ApplyRecommendationQueue(report, topProcess, dnsSummary);

        CoreDescriptionText.Text = IsZh
            ? $"AETHER 已完成只读分析，当前优化潜力为{TranslatePotential(report.OptimizationPotential)}。所有建议仍为只读解释，不会自动执行。"
            : $"AETHER completed read-only analysis. Optimization potential is {TranslatePotential(report.OptimizationPotential)}. Recommendations remain read-only.";
    }

    private void ApplyRecommendationQueue(
        PerformanceAnalysisReport report,
        ProcessSnapshot? topProcess,
        string dnsSummary)
    {
        var recommendations = report.Recommendations.Take(3).ToArray();

        QueueItemOneTitleText.Text = recommendations.ElementAtOrDefault(0)?.Title ?? (IsZh ? "本机系统" : "Local system");
        QueueItemOneBodyText.Text = recommendations.ElementAtOrDefault(0) is { } first
            ? TranslateRecommendation(first)
            : (IsZh ? "已完成只读扫描。" : "Read-only scan completed.");

        QueueItemTwoTitleText.Text = recommendations.ElementAtOrDefault(1)?.Title ?? (IsZh ? "占用最高进程" : "Top memory process");
        QueueItemTwoBodyText.Text = recommendations.ElementAtOrDefault(1) is { } second
            ? TranslateRecommendation(second)
            : topProcess is null
                ? (IsZh ? "当前未读取到进程列表。" : "No process list was captured.")
                : $"{topProcess.Name} · {FormatMb(topProcess.MemoryMb)}";

        QueueItemThreeTitleText.Text = recommendations.ElementAtOrDefault(2)?.Title ?? (IsZh ? "当前 DNS" : "Current DNS");
        QueueItemThreeBodyText.Text = recommendations.ElementAtOrDefault(2) is { } third
            ? TranslateRecommendation(third)
            : dnsSummary;
    }

    private string TranslatePotential(OptimizationPotentialLevel potential)
    {
        if (!IsZh)
        {
            return potential.ToString();
        }

        return potential switch
        {
            OptimizationPotentialLevel.High => "高",
            OptimizationPotentialLevel.Medium => "中",
            _ => "低"
        };
    }

    private string TranslateRecommendation(OptimizationRecommendation recommendation)
    {
        if (!IsZh)
        {
            return recommendation.Detail;
        }

        return recommendation.Category switch
        {
            RecommendationCategory.Memory => "建议复核高内存后台进程，游戏或创作前保留更多可用内存。",
            RecommendationCategory.Storage => "建议检查可用空间，避免大型游戏更新、缓存或视频导出受影响。",
            RecommendationCategory.Process => "发现后台负载可能占用性能余量；当前仅提示，不会自动关闭进程。",
            RecommendationCategory.Dns => "建议先对当前 DNS 与 360 安全 DNS 做延迟、抖动和失败率对比。",
            RecommendationCategory.Network => "建议复核网络接口状态，后续进入轻量 Ping/Jitter 检测。",
            _ => "当前基线看起来健康，后续扫描将用于对比变化。"
        };
    }

    private string TranslateRisk(RiskLevel riskLevel)
    {
        if (!IsZh)
        {
            return riskLevel.ToString();
        }

        return riskLevel switch
        {
            RiskLevel.Low => "低",
            RiskLevel.Medium => "中",
            RiskLevel.High => "高",
            _ => "只读"
        };
    }

    private void NavigateTo(string page)
    {
        _currentPage = page;
        var isDashboard = page == "dashboard";

        MetricsGrid.IsVisible = isDashboard;
        DashboardMainGrid.IsVisible = isDashboard;
        TipsCard.IsVisible = isDashboard;
        ModuleContent.IsVisible = !isDashboard;

        UpdateNavigationState(page);

        if (isDashboard)
        {
            PageTitleText.Text = IsZh ? "仪表盘" : "Dashboard";
            PageSubtitleText.Text = IsZh
                ? "面向 Windows PC 的 AI 原生性能守护系统"
                : "AI-native performance guardian for Windows PCs";
            ModuleContent.Content = null;
            return;
        }

        PageTitleText.Text = GetModuleTitle(page);
        PageSubtitleText.Text = GetModuleSubtitle(page);
        ModuleContent.Content = BuildModulePage(page);
    }

    private void UpdateNavigationState(string page)
    {
        var buttons = new[]
        {
            (NavDashboardButton, "dashboard"),
            (NavPcIntelligenceButton, "pc"),
            (NavGameOptimizationButton, "game"),
            (NavPerformanceMonitorButton, "monitor"),
            (NavOptimizationCenterButton, "optimization"),
            (NavToolkitButton, "toolkit"),
            (NavDnsOptimizationButton, "dns"),
            (NavNetworkSpeedButton, "speed"),
            (NavAiAdvisorButton, "advisor"),
            (NavHistoryButton, "history"),
            (NavSettingsButton, "settings")
        };

        foreach (var (button, key) in buttons)
        {
            button.Classes.Set("active", key == page);
        }
    }

    private bool IsZh => _currentLanguage == "zh-CN";

    private string GetModuleTitle(string page)
    {
        return page switch
        {
            "pc" => IsZh ? "电脑智能分析" : "PC Intelligence",
            "game" => IsZh ? "游戏优化" : "Game Optimization",
            "monitor" => IsZh ? "性能监控" : "Performance Monitor",
            "optimization" => IsZh ? "优化中心" : "Optimization Center",
            "toolkit" => IsZh ? "工具中心" : "Toolkit Center",
            "dns" => IsZh ? "DNS 优化" : "DNS Optimization",
            "speed" => IsZh ? "网络测速" : "Network Speed Test",
            "advisor" => IsZh ? "AI 顾问" : "AI Advisor",
            "history" => IsZh ? "历史记录" : "History",
            "settings" => IsZh ? "设置" : "Settings",
            _ => IsZh ? "仪表盘" : "Dashboard"
        };
    }

    private string GetModuleSubtitle(string page)
    {
        return page switch
        {
            "pc" => IsZh ? "只读理解硬件、系统、后台负载与风险状态" : "Read-only understanding of hardware, OS, background load, and risk state",
            "game" => IsZh ? "游戏前检查、游戏中保护与配置档预留" : "Pre-game checks, in-game protection, and profile reservations",
            "monitor" => IsZh ? "按需启动的轻量实时指标与趋势视图" : "On-demand lightweight realtime metrics and trend views",
            "optimization" => IsZh ? "安全优化规则、风险等级、备份与回滚框架" : "Safe optimization rules, risk levels, backup, and rollback framework",
            "toolkit" => IsZh ? "面向 Windows 的工具集合：每个工具都必须说明用途、风险和回滚路径" : "Windows-focused tools where every item explains purpose, risk, and revert path",
            "dns" => IsZh ? "检测 DNS 延迟、稳定性与安全风险，默认只读不改网络配置" : "Checks DNS latency, stability, and safety risk without changing network settings by default",
            "speed" => IsZh ? "识别运营商与地区后进行稳定测速，默认需要用户确认才消耗流量" : "Identifies ISP and region before stable speed testing; traffic usage requires user consent by default",
            "advisor" => IsZh ? "基于扫描报告的解释、建议与风险说明" : "Explanations, recommendations, and risk notes based on scan reports",
            "history" => IsZh ? "扫描、建议、优化和回滚记录" : "Scan, recommendation, optimization, and rollback history",
            "settings" => IsZh ? "语言、性能模式、隐私与更新设置" : "Language, performance mode, privacy, and update settings",
            _ => IsZh ? "面向 Windows PC 的 AI 原生性能守护系统" : "AI-native performance guardian for Windows PCs"
        };
    }

    private Control BuildModulePage(string page)
    {
        var rows = GetModuleRows(page);
        var grid = new Grid
        {
            ColumnDefinitions = new ColumnDefinitions("*,*"),
            RowDefinitions = new RowDefinitions("Auto,Auto,Auto"),
            ColumnSpacing = 14,
            RowSpacing = 14
        };

        for (var index = 0; index < rows.Length; index++)
        {
            var card = CreateModuleCard(rows[index].Title, rows[index].Body, rows[index].Badge, rows[index].Accent);
            Grid.SetColumn(card, index % 2);
            Grid.SetRow(card, index / 2);
            grid.Children.Add(card);
        }

        if (page is "speed" or "game" or "monitor" or "optimization")
        {
            return new ScrollViewer
            {
                VerticalScrollBarVisibility = ScrollBarVisibility.Auto,
                Content = new StackPanel
                {
                    Spacing = 14,
                    Children =
                    {
                        page switch
                        {
                            "speed" => CreateNetworkSpeedActionPanel(),
                            "game" => CreateGameSessionActionPanel(),
                            "monitor" => CreateMonitorActionPanel(),
                            _ => CreateDryRunActionPanel()
                        },
                        grid
                    }
                }
            };
        }

        return new ScrollViewer
        {
            VerticalScrollBarVisibility = ScrollBarVisibility.Auto,
            Content = grid
        };
    }

    private (string Title, string Body, string Badge, string Accent)[] GetModuleRows(string page)
    {
        if (page == "speed" && _lastNetworkDiagnostics is not null)
        {
            return GetNetworkDiagnosticRows(_lastNetworkDiagnostics);
        }

        if (page == "game" && _lastGameBoostPlan is not null)
        {
            return GetGameBoostRows(_lastGameBoostPlan);
        }

        if (page == "game" && _lastGameSessionAnalysis is not null)
        {
            return GetGameSessionRows(_lastGameSessionAnalysis);
        }

        if (page == "monitor" && _lastMonitorSnapshot is not null)
        {
            return GetMonitorRows(_lastMonitorSnapshot);
        }

        if (page == "optimization" && _lastExecutionReport is not null)
        {
            return GetExecutionRows(_lastExecutionReport);
        }

        if (page == "optimization" && _lastDryRunReport is not null)
        {
            return GetDryRunRows(_lastDryRunReport);
        }

        if (page == "dns" && _lastNetworkDiagnostics is not null)
        {
            return GetDnsDiagnosticRows(_lastNetworkDiagnostics);
        }

        if (page == "toolkit")
        {
            return GetToolkitRows();
        }

        if (_lastSnapshot is not null)
        {
            var liveRows = GetLiveModuleRows(page, _lastSnapshot);
            if (liveRows.Length > 0)
            {
                return liveRows;
            }
        }

        if (IsZh)
        {
            return page switch
            {
                "pc" => new[]
                {
                    ("硬件概览", "CPU、GPU、内存、磁盘与系统版本会先以只读方式采集，避免任何系统修改。", "只读", "blue"),
                    ("后台负载", "展示高占用进程、启动项和可能影响游戏的后台任务。", "预览", "amber"),
                    ("驱动与系统状态", "预留驱动版本、Windows 游戏模式、电源计划和 Defender 状态检查。", "预留", "green"),
                    ("分析结论", "所有结论必须附带来源、置信度和风险说明。", "AI Ready", "blue")
                },
                "game" => new[]
                {
                    ("游戏前检查", "启动游戏前检测高负载进程、录屏、浏览器和电源模式。", "规划中", "blue"),
                    ("游戏配置档", "预留 LOL、Steam 游戏和自定义 EXE 的独立配置档。", "Profile", "green"),
                    ("游戏中保护", "游戏运行时降低监控频率，避免影响帧率。", "低占用", "amber"),
                    ("优化建议", "只给出建议和风险解释，不会自动关闭进程。", "安全", "green")
                },
                "monitor" => new[]
                {
                    ("实时指标", "CPU、GPU、内存、磁盘、温度和延迟按需采样。", "按需", "blue"),
                    ("采样策略", "窗口隐藏或游戏运行时自动降频，目标是空闲 CPU 接近 0%。", "低占用", "green"),
                    ("趋势图表", "优先使用轻量 Avalonia 绘制，不引入重型图表运行时。", "轻量", "amber"),
                    ("告警规则", "异常负载、温度和内存压力会进入 Sentinel 动态。", "预留", "blue")
                },
                "optimization" => new[]
                {
                    ("规则库", "每条规则必须有目标系统、风险等级、备份方式和回滚方式。", "安全框架", "blue"),
                    ("Dry Run", "默认只展示将会修改什么，不直接执行真实优化。", "默认", "green"),
                    ("低风险优化", "启动项建议、临时文件清理、电源计划提示优先进入第一批。", "优先", "amber"),
                    ("验证机制", "优化后必须记录前后状态，并生成可回看报告。", "验证", "green")
                },
                "dns" => new[]
                {
                    ("当前 DNS 检测", "读取当前 DNS 配置、网络接口和解析延迟，默认只读。", "只读", "blue"),
                    ("延迟与稳定性测试", "对候选 DNS 做多轮解析耗时、失败率和抖动评估。", "测速", "green"),
                    ("360 安全 DNS", "已加入官方验证 Provider Registry，后续会按运营商、地区和稳定性评分推荐。", "已验证", "green"),
                    ("备份与回滚", "真实切换 DNS 前必须备份原配置，并支持一键恢复。", "必需", "green")
                },
                "speed" => new[]
                {
                    ("运营商与地区识别", "优先使用离线 IP 库识别中国大陆省市与运营商，避免依赖单一公网接口。", "只读", "blue"),
                    ("测速服务器选择", "按地区、运营商和延迟选择候选节点，未来支持自建 LibreSpeed 节点。", "匹配", "green"),
                    ("速度与稳定性", "同时记录下载、上传、延迟、抖动和失败率，避免只看峰值带宽。", "综合", "amber"),
                    ("流量保护", "完整测速会消耗流量，必须用户确认，并提供轻量 Ping 模式。", "低占用", "green")
                },
                "advisor" => new[]
                {
                    ("系统报告摘要", "AI 只读取扫描报告和本地结构化结果，不直接控制系统。", "解释", "blue"),
                    ("推荐理由", "每条建议都必须说明原因、收益、风险和验证方式。", "透明", "green"),
                    ("风险评估", "高风险操作必须明确标红并要求用户确认。", "守护", "amber"),
                    ("模型抽象", "预留 OpenAI、DeepSeek、本地模型等提供商切换。", "预留", "blue")
                },
                "history" => new[]
                {
                    ("扫描历史", "记录扫描时间、数据来源、结果摘要和置信度。", "记录", "blue"),
                    ("建议历史", "保留 AI 建议、风险等级和用户是否采纳。", "追踪", "green"),
                    ("优化历史", "未来真实优化必须记录执行日志、验证结果与回滚状态。", "审计", "amber"),
                    ("报告导出", "预留 Markdown / JSON / 本地报告导出。", "预留", "blue")
                },
                "settings" => new[]
                {
                    ("语言", "默认简体中文，支持切换英文界面。", "已启用", "green"),
                    ("性能模式", "默认低占用：不开常驻高频监控，不主动后台轮询。", "默认", "blue"),
                    ("隐私", "服务器地址、密钥、签名证书和遥测配置不进入开源仓库。", "安全", "amber"),
                    ("更新", "预留版本检查、包验证、安装和回滚设置。", "预留", "blue")
                },
                _ => Array.Empty<(string Title, string Body, string Badge, string Accent)>()
            };
        }

        return page switch
        {
            "pc" => new[]
            {
                ("Hardware Overview", "CPU, GPU, memory, disk, and OS version will be collected read-only first.", "Read-only", "blue"),
                ("Background Load", "Shows heavy processes, startup entries, and game-impacting background tasks.", "Preview", "amber"),
                ("Driver And OS State", "Reserves checks for drivers, Game Mode, power plans, and Defender state.", "Reserved", "green"),
                ("Analysis Result", "Every conclusion must include source, confidence, and risk notes.", "AI Ready", "blue")
            },
            "game" => new[]
            {
                ("Pre-game Check", "Detects heavy processes, recording tools, browsers, and power mode before launch.", "Planned", "blue"),
                ("Game Profiles", "Reserves profiles for League of Legends, Steam games, and custom EXE entries.", "Profile", "green"),
                ("In-game Guard", "Lowers monitoring frequency while gaming to protect frame rate.", "Low load", "amber"),
                ("Optimization Advice", "Recommends and explains risk without automatically closing processes.", "Safe", "green")
            },
            "monitor" => new[]
            {
                ("Realtime Metrics", "CPU, GPU, memory, disk, temperature, and latency sample only on demand.", "On demand", "blue"),
                ("Sampling Policy", "Hidden windows and game sessions reduce sampling frequency; idle CPU should approach 0%.", "Low load", "green"),
                ("Trend Charts", "Prefer lightweight Avalonia drawing over heavy chart runtimes.", "Light", "amber"),
                ("Alert Rules", "Abnormal load, thermal pressure, and memory pressure feed Sentinel updates.", "Reserved", "blue")
            },
            "optimization" => new[]
            {
                ("Rule Library", "Every rule needs target system, risk level, backup method, and rollback method.", "Safety", "blue"),
                ("Dry Run", "Default mode only shows what would change before real execution exists.", "Default", "green"),
                ("Low-risk Actions", "Startup suggestions, temp cleanup, and power-plan hints come first.", "Priority", "amber"),
                ("Verification", "Future actions must record before/after state and create a report.", "Verify", "green")
            },
            "dns" => new[]
            {
                ("Current DNS Check", "Reads current DNS configuration, network interfaces, and resolver latency in read-only mode.", "Read-only", "blue"),
                ("Latency And Stability", "Benchmarks candidate DNS providers across lookup time, failure rate, and jitter.", "Benchmark", "green"),
                ("360 Secure DNS", "Added to the verified provider registry; future recommendations depend on ISP, region, and stability score.", "Verified", "green"),
                ("Backup And Rollback", "Real DNS switching must back up the original configuration and support one-click restore.", "Required", "green")
            },
            "speed" => new[]
            {
                ("ISP And Region", "Prefer offline IP data to identify China mainland province, city, and ISP without depending on one public API.", "Read-only", "blue"),
                ("Server Selection", "Choose candidate nodes by region, ISP, and latency; future self-hosted LibreSpeed nodes are supported.", "Match", "green"),
                ("Speed And Stability", "Record download, upload, latency, jitter, and failure rate instead of peak bandwidth only.", "Balanced", "amber"),
                ("Traffic Guard", "Full tests consume data, require user consent, and keep a lightweight ping-only mode available.", "Low load", "green")
            },
            "advisor" => new[]
            {
                ("System Report Summary", "AI reads scan reports and structured local results; it does not control the system.", "Explain", "blue"),
                ("Recommendation Reasoning", "Every suggestion needs cause, benefit, risk, and verification method.", "Transparent", "green"),
                ("Risk Assessment", "High-risk actions must be highlighted and require user confirmation.", "Guarded", "amber"),
                ("Model Abstraction", "Reserves provider switching for OpenAI, DeepSeek, and local models.", "Reserved", "blue")
            },
            "history" => new[]
            {
                ("Scan History", "Records scan time, sources, summaries, and confidence.", "Record", "blue"),
                ("Advice History", "Stores AI advice, risk level, and whether the user accepted it.", "Track", "green"),
                ("Optimization History", "Future real actions record logs, verification, and rollback state.", "Audit", "amber"),
                ("Report Export", "Reserves Markdown, JSON, and local report export.", "Reserved", "blue")
            },
            "settings" => new[]
            {
                ("Language", "Simplified Chinese is default, with English UI switching.", "Enabled", "green"),
                ("Performance Mode", "Default low load: no persistent high-frequency monitoring or background polling.", "Default", "blue"),
                ("Privacy", "Server endpoints, keys, certificates, and telemetry config stay out of the open repo.", "Safe", "amber"),
                ("Updates", "Reserves version checking, package verification, installation, and rollback.", "Reserved", "blue")
            },
            _ => Array.Empty<(string Title, string Body, string Badge, string Accent)>()
        };
    }

    private (string Title, string Body, string Badge, string Accent)[] GetLiveModuleRows(string page, SystemSnapshot snapshot)
    {
        var report = _lastReport ?? _performanceAnalyzer.Analyze(snapshot);
        var topProcess = snapshot.TopProcesses.FirstOrDefault();
        var primaryStorage = snapshot.Hardware.Storage.FirstOrDefault();
        var factors = report.Factors.ToArray();
        var recommendations = report.Recommendations.ToArray();
        var dnsSummary = snapshot.Network.CurrentDnsServers.Count == 0
            ? (IsZh ? "未检测到 DNS 服务器，后续需要平台适配增强。" : "No DNS servers detected; platform adapter needs future enhancement.")
            : string.Join(", ", snapshot.Network.CurrentDnsServers);

        if (IsZh)
        {
            return page switch
            {
                "pc" => new[]
                {
                    ("系统信息", $"{snapshot.OperatingSystem.Name} / {snapshot.OperatingSystem.Architecture} / {snapshot.OperatingSystem.DeviceName}", "真实", "green"),
                    ("CPU", snapshot.Hardware.CpuName, "只读", "blue"),
                    ("GPU 智能", _lastGpuReport is null ? snapshot.Hardware.GpuName : $"{_lastGpuReport.Name} · {TranslateGpuVendor(_lastGpuReport.Vendor)} · 遥测：{TranslateGpuTelemetry(_lastGpuReport.TelemetryAvailability)}", "只读", _lastGpuReport?.DriverWriteActionsEnabled == true ? "amber" : "green"),
                    ("启动项", snapshot.StartupItems.Count == 0 ? "当前平台未读取到 Windows 启动项，需在 Windows 上验证。" : $"已读取 {snapshot.StartupItems.Count} 个启动项，{snapshot.StartupItems.Count(item => item.ImpactLevel is StartupImpactLevel.High or StartupImpactLevel.Medium)} 个需要复核。", "Windows", snapshot.StartupItems.Count == 0 ? "amber" : "green"),
                    ("电源计划", $"{snapshot.PowerPlan.Name} · {snapshot.PowerPlan.Source}", "只读", snapshot.PowerPlan.IsHighPerformanceCandidate ? "green" : "amber")
                },
                "game" => new[]
                {
                    ("游戏候选进程", snapshot.GameProcessCandidates.Count == 0 ? "未识别到游戏、启动器、反作弊或录制工具候选。" : $"识别到 {snapshot.GameProcessCandidates.Count} 个候选：{string.Join(", ", snapshot.GameProcessCandidates.Take(3).Select(candidate => candidate.Name))}", "只读", snapshot.GameProcessCandidates.Count == 0 ? "amber" : "green"),
                    ("候选角色", snapshot.GameProcessCandidates.Count == 0 ? "等待 Windows 游戏环境验证。" : string.Join(" / ", snapshot.GameProcessCandidates.GroupBy(candidate => candidate.Role).Select(group => $"{TranslateGameRole(group.Key)} {group.Count()}")), "分类", "blue"),
                    ("启动器边界", "Steam、Epic、Battle.net、Riot、WeGame 会先作为启动器候选，不直接当作游戏本体优化。", "安全", "green"),
                    ("反作弊边界", "Easy Anti-Cheat、BattlEye 等候选只做识别和保护，不做压制或规避。", "保护", "amber")
                },
                "dns" => new[]
                {
                    ("当前 DNS", dnsSummary, "真实", snapshot.Network.CurrentDnsServers.Count == 0 ? "amber" : "green"),
                    ("360 安全 DNS", "Provider Registry 已验证：101.226.4.6、218.30.118.6。真实切换仍需测速、备份和确认。", "已验证", "green"),
                    ("网络接口", $"{snapshot.Network.PrimaryInterfaceName} / {snapshot.Network.ConnectionType}", "只读", "blue"),
                    ("回滚要求", "DNS 切换执行层尚未启用。后续必须保存原 DNS 并支持一键恢复。", "必需", "amber")
                },
                "speed" => new[]
                {
                    ("网络接口", $"{snapshot.Network.PrimaryInterfaceName} / {snapshot.Network.ConnectionType}", "真实", "green"),
                    ("运营商与地区", "公网 IP 归属地识别尚未启用。后续优先接入离线 IP 库。", "待接入", "amber"),
                    ("轻量延迟测试", "完整测速会消耗流量，下一步先实现用户触发的 Ping/Jitter 模式。", "低占用", "blue"),
                    ("存储参考", primaryStorage is null ? "未读取到磁盘状态。" : $"{primaryStorage.Name} 使用率 {primaryStorage.ActivePercent:0}% / 剩余 {primaryStorage.FreeGb} GB。", "上下文", "green")
                },
                "optimization" => new[]
                {
                    ("综合评分", $"当前只读评分 {report.OverallScore}/100，优化潜力：{TranslatePotential(report.OptimizationPotential)}。", "评分", GetScoreAccent(report.OverallScore)),
                    ("主要风险", factors.OrderBy(factor => factor.Score).FirstOrDefault() is { } weakest ? $"{TranslateFactorTitle(weakest)}：{weakest.Detail}" : "未发现明显风险。", "分析", "amber"),
                    ("推荐动作", recommendations.FirstOrDefault() is { } first ? TranslateRecommendation(first) : "当前无需执行优化。", "只读", "blue"),
                    ("执行状态", "仍处于 Dry Run 之前阶段，不会修改系统、DNS 或进程。", "安全", "green")
                },
                "advisor" => new[]
                {
                    ("AI 摘要", $"本次扫描生成 {factors.Length} 个评分因子和 {recommendations.Length} 条只读建议。", "分析", "blue"),
                    ("优化潜力", $"当前判断为{TranslatePotential(report.OptimizationPotential)}，后续会结合游戏会话和实时监控继续校准。", "潜力", GetScoreAccent(report.OverallScore)),
                    ("风险透明", recommendations.FirstOrDefault() is { } first ? $"风险等级：{TranslateRisk(first.RiskLevel)}；验证信号：{first.VerificationSignal}" : "暂无风险建议。", "解释", "amber"),
                    ("执行边界", "AI 顾问只解释扫描报告，不直接控制系统。", "只读", "green")
                },
                _ => Array.Empty<(string Title, string Body, string Badge, string Accent)>()
            };
        }

        return page switch
        {
            "pc" => new[]
            {
                ("System", $"{snapshot.OperatingSystem.Name} / {snapshot.OperatingSystem.Architecture} / {snapshot.OperatingSystem.DeviceName}", "Live", "green"),
                ("CPU", snapshot.Hardware.CpuName, "Read-only", "blue"),
                ("GPU Intelligence", _lastGpuReport is null ? snapshot.Hardware.GpuName : $"{_lastGpuReport.Name} · {_lastGpuReport.Vendor} · telemetry: {_lastGpuReport.TelemetryAvailability}", "Read-only", _lastGpuReport?.DriverWriteActionsEnabled == true ? "amber" : "green"),
                ("Startup Items", snapshot.StartupItems.Count == 0 ? "No Windows startup items were captured on this platform; validate on Windows." : $"Captured {snapshot.StartupItems.Count} startup entries; {snapshot.StartupItems.Count(item => item.ImpactLevel is StartupImpactLevel.High or StartupImpactLevel.Medium)} need review.", "Windows", snapshot.StartupItems.Count == 0 ? "amber" : "green"),
                ("Power Plan", $"{snapshot.PowerPlan.Name} · {snapshot.PowerPlan.Source}", "Read-only", snapshot.PowerPlan.IsHighPerformanceCandidate ? "green" : "amber")
            },
            "game" => new[]
            {
                ("Game Candidates", snapshot.GameProcessCandidates.Count == 0 ? "No game, launcher, anti-cheat, or capture-tool candidate was detected." : $"Detected {snapshot.GameProcessCandidates.Count}: {string.Join(", ", snapshot.GameProcessCandidates.Take(3).Select(candidate => candidate.Name))}", "Read-only", snapshot.GameProcessCandidates.Count == 0 ? "amber" : "green"),
                ("Candidate Roles", snapshot.GameProcessCandidates.Count == 0 ? "Waiting for Windows gaming environment validation." : string.Join(" / ", snapshot.GameProcessCandidates.GroupBy(candidate => candidate.Role).Select(group => $"{group.Key} {group.Count()}")), "Classify", "blue"),
                ("Launcher Boundary", "Steam, Epic, Battle.net, Riot, and WeGame are treated as launcher candidates before game-body optimization.", "Safe", "green"),
                ("Anti-cheat Boundary", "Easy Anti-Cheat and BattlEye candidates are identified and protected, never bypassed.", "Protect", "amber")
            },
            "dns" => new[]
            {
                ("Current DNS", dnsSummary, snapshot.Network.CurrentDnsServers.Count == 0 ? "Watch" : "Live", snapshot.Network.CurrentDnsServers.Count == 0 ? "amber" : "green"),
                ("360 Secure DNS", "Provider registry verified: 101.226.4.6 and 218.30.118.6. Real switching still requires benchmark, backup, and confirmation.", "Verified", "green"),
                ("Network Interface", $"{snapshot.Network.PrimaryInterfaceName} / {snapshot.Network.ConnectionType}", "Read-only", "blue"),
                ("Rollback Requirement", "DNS switching is not enabled yet. Future execution must save original DNS and support one-click restore.", "Required", "amber")
            },
            "speed" => new[]
            {
                ("Network Interface", $"{snapshot.Network.PrimaryInterfaceName} / {snapshot.Network.ConnectionType}", "Live", "green"),
                ("ISP And Region", "Public IP region detection is not enabled yet. Offline IP data is the preferred next provider.", "Pending", "amber"),
                ("Light Latency Test", "Full speed tests consume traffic. Next step is user-triggered Ping/Jitter mode first.", "Low load", "blue"),
                ("Storage Context", primaryStorage is null ? "No drive state was captured." : $"{primaryStorage.Name} usage {primaryStorage.ActivePercent:0}% / {primaryStorage.FreeGb} GB free.", "Context", "green")
            },
            "optimization" => new[]
            {
                ("Overall Score", $"Current read-only score is {report.OverallScore}/100. Optimization potential: {TranslatePotential(report.OptimizationPotential)}.", "Score", GetScoreAccent(report.OverallScore)),
                ("Primary Risk", factors.OrderBy(factor => factor.Score).FirstOrDefault() is { } weakest ? $"{weakest.Title}: {weakest.Detail}" : "No clear risk found.", "Analysis", "amber"),
                ("Recommended Action", recommendations.FirstOrDefault()?.Detail ?? "No optimization action is needed right now.", "Read-only", "blue"),
                ("Execution State", "Still before Dry Run; no system, DNS, or process change will be applied.", "Safe", "green")
            },
            "advisor" => new[]
            {
                ("AI Summary", $"This scan produced {factors.Length} score factors and {recommendations.Length} read-only recommendations.", "Analysis", "blue"),
                ("Optimization Potential", $"Current potential is {TranslatePotential(report.OptimizationPotential)}. Future game sessions and monitoring will calibrate this.", "Potential", GetScoreAccent(report.OverallScore)),
                ("Risk Transparency", recommendations.FirstOrDefault() is { } first ? $"Risk: {first.RiskLevel}. Verification: {first.VerificationSignal}" : "No risk recommendation yet.", "Explain", "amber"),
                ("Execution Boundary", "AI Advisor explains scan reports; it does not control the system.", "Read-only", "green")
            },
            _ => Array.Empty<(string Title, string Body, string Badge, string Accent)>()
        };
    }

    private static string FormatMb(long mb)
    {
        if (mb <= 0)
        {
            return "--";
        }

        return mb >= 1024
            ? $"{mb / 1024d:0.0} GB"
            : $"{mb} MB";
    }

    private string TranslateFactorTitle(ScoreFactor factor)
    {
        if (!IsZh)
        {
            return factor.Title;
        }

        return factor.Key switch
        {
            "memory" => "内存",
            "storage" => "存储",
            "process" => "后台负载",
            "dns" => "DNS",
            "network" => "网络",
            _ => factor.Title
        };
    }

    private string TranslateGameRole(GameProcessRole role)
    {
        if (!IsZh)
        {
            return role.ToString();
        }

        return role switch
        {
            GameProcessRole.Game => "游戏",
            GameProcessRole.Launcher => "启动器",
            GameProcessRole.AntiCheat => "反作弊",
            GameProcessRole.Updater => "更新器",
            GameProcessRole.CaptureTool => "录制工具",
            _ => "未知"
        };
    }

    private static string CompactText(string value, int maxLength)
    {
        if (string.IsNullOrWhiteSpace(value) || value.Length <= maxLength)
        {
            return value;
        }

        return $"{value[..Math.Max(0, maxLength - 1)]}…";
    }

    private static string GetScoreAccent(int score)
    {
        return score switch
        {
            >= 85 => "green",
            >= 70 => "amber",
            _ => "red"
        };
    }

    private NetworkDiagnosticsRequest CreateNetworkDiagnosticsRequest()
    {
        var targets = new[]
        {
            new NetworkLatencyTarget("360 Secure DNS A", "101.226.4.6", "China Mainland", "360"),
            new NetworkLatencyTarget("360 Secure DNS B", "218.30.118.6", "China Mainland", "360"),
            new NetworkLatencyTarget("Public China Baseline", "223.5.5.5", "China Mainland", "AliDNS"),
            new NetworkLatencyTarget("Public China Baseline 2", "119.29.29.29", "China Mainland", "DNSPod")
        };

        var dnsCandidates = NetworkProviderCatalog.DefaultDnsCandidates
            .Where(candidate => candidate.OfficialEndpointConfirmed && candidate.Addresses.Count > 0)
            .ToArray();

        return new NetworkDiagnosticsRequest(
            SampleCount: 4,
            Timeout: TimeSpan.FromSeconds(2),
            LatencyTargets: targets,
            DnsCandidates: dnsCandidates,
            DnsLookupDomain: "www.qq.com");
    }

    private Control CreateNetworkSpeedActionPanel()
    {
        _networkSpeedStatusText = new TextBlock
        {
            Text = _lastNetworkDiagnostics is null
                ? (IsZh
                    ? "点击开始轻量测速：只执行 Ping/Jitter 与 DNS 查询基准测试，不进行下载/上传测速。"
                    : "Run a quick test: Ping/Jitter and DNS lookup benchmark only, without download/upload traffic.")
                : FormatNetworkDiagnosticsSummary(_lastNetworkDiagnostics),
            FontSize = 13,
            Foreground = new SolidColorBrush(Color.Parse("#A8B3C2")),
            TextWrapping = TextWrapping.Wrap
        };

        var button = new Button
        {
            Classes = { "primary" },
            Content = IsZh ? "开始轻量测速" : "Run Quick Test",
            MinWidth = 132,
            HorizontalContentAlignment = HorizontalAlignment.Center,
            VerticalContentAlignment = VerticalAlignment.Center
        };
        button.Click += OnNetworkSpeedTestClicked;
        Grid.SetColumn(button, 1);

        return new Border
        {
            Classes = { "card" },
            Padding = new Thickness(18),
            Child = new Grid
            {
                ColumnDefinitions = new ColumnDefinitions("*,Auto"),
                ColumnSpacing = 16,
                Children =
                {
                    new StackPanel
                    {
                        Spacing = 7,
                        Children =
                        {
                            new TextBlock
                            {
                                Text = IsZh ? "网络测速控制" : "Network Test Control",
                                FontSize = 18,
                                FontWeight = FontWeight.SemiBold
                            },
                            _networkSpeedStatusText
                        }
                    },
                    button
                }
            }
        };
    }

    private Control CreateGameSessionActionPanel()
    {
        _gameSessionStatusText = new TextBlock
        {
            Text = _lastGameSessionAnalysis is null
                ? (IsZh
                    ? $"游戏库 {_gameLibrary.Count} 个条目。添加 EXE 后执行扫描，即可做只读会话识别。"
                    : $"Game library has {_gameLibrary.Count} entries. Add an EXE, run scan, then detect the session read-only.")
                : (IsZh ? TranslateGameSessionExplanation(_lastGameSessionAnalysis) : _lastGameSessionAnalysis.Explanation),
            FontSize = 13,
            Foreground = new SolidColorBrush(Color.Parse("#A8B3C2")),
            TextWrapping = TextWrapping.Wrap
        };

        var addButton = new Button
        {
            Classes = { "secondary" },
            Content = IsZh ? "添加游戏" : "Add Game",
            MinWidth = 112,
            HorizontalContentAlignment = HorizontalAlignment.Center,
            VerticalContentAlignment = VerticalAlignment.Center
        };
        addButton.Click += OnAddGameClicked;

        var analyzeButton = new Button
        {
            Classes = { "primary" },
            Content = IsZh ? "识别会话" : "Detect Session",
            MinWidth = 112,
            HorizontalContentAlignment = HorizontalAlignment.Center,
            VerticalContentAlignment = VerticalAlignment.Center
        };
        analyzeButton.Click += OnAnalyzeGameSessionClicked;

        var boostButton = new Button
        {
            Classes = { "primary" },
            Content = IsZh ? "加速方案" : "Boost Plan",
            MinWidth = 112,
            HorizontalContentAlignment = HorizontalAlignment.Center,
            VerticalContentAlignment = VerticalAlignment.Center
        };
        boostButton.Click += OnGenerateGameBoostPlanClicked;

        var buttonPanel = new StackPanel
        {
            Orientation = Orientation.Horizontal,
            Spacing = 10,
            Children = { addButton, analyzeButton, boostButton }
        };
        Grid.SetColumn(buttonPanel, 1);

        return new Border
        {
            Classes = { "card" },
            Padding = new Thickness(18),
            Child = new Grid
            {
                ColumnDefinitions = new ColumnDefinitions("*,Auto"),
                ColumnSpacing = 16,
                Children =
                {
                    new StackPanel
                    {
                        Spacing = 7,
                        Children =
                        {
                            new TextBlock
                            {
                                Text = IsZh ? "游戏会话控制" : "Game Session Control",
                                FontSize = 18,
                                FontWeight = FontWeight.SemiBold
                            },
                            _gameSessionStatusText
                        }
                    },
                    buttonPanel
                }
            }
        };
    }

    private Control CreateMonitorActionPanel()
    {
        _monitorStatusText = new TextBlock
        {
            Text = _lastMonitorSnapshot is null
                ? (IsZh
                    ? "点击采样一次：只做短窗口轻量采样，不启动常驻后台监控。"
                    : "Sample once: short-window light sampling only, without persistent background monitoring.")
                : FormatMonitorSummary(_lastMonitorSnapshot),
            FontSize = 13,
            Foreground = new SolidColorBrush(Color.Parse("#A8B3C2")),
            TextWrapping = TextWrapping.Wrap
        };

        var button = new Button
        {
            Classes = { "primary" },
            Content = IsZh ? "采样一次" : "Sample Once",
            MinWidth = 112,
            HorizontalContentAlignment = HorizontalAlignment.Center,
            VerticalContentAlignment = VerticalAlignment.Center
        };
        button.Click += OnMonitorSampleClicked;
        Grid.SetColumn(button, 1);

        return new Border
        {
            Classes = { "card" },
            Padding = new Thickness(18),
            Child = new Grid
            {
                ColumnDefinitions = new ColumnDefinitions("*,Auto"),
                ColumnSpacing = 16,
                Children =
                {
                    new StackPanel
                    {
                        Spacing = 7,
                        Children =
                        {
                            new TextBlock
                            {
                                Text = IsZh ? "低占用监控控制" : "Low-overhead Monitor Control",
                                FontSize = 18,
                                FontWeight = FontWeight.SemiBold
                            },
                            _monitorStatusText
                        }
                    },
                    button
                }
            }
        };
    }

    private Control CreateDryRunActionPanel()
    {
        _dryRunStatusText = new TextBlock
        {
            Text = _lastDryRunReport is null
                ? (IsZh
                    ? "生成优化 Dry Run：只预览规则、风险、备份、验证和回滚，不执行任何系统修改。"
                    : "Generate optimization Dry Run: preview rules, risk, backup, verification, and rollback without changing the system.")
                : FormatDryRunSummary(_lastDryRunReport),
            FontSize = 13,
            Foreground = new SolidColorBrush(Color.Parse("#A8B3C2")),
            TextWrapping = TextWrapping.Wrap
        };

        var dryRunButton = new Button
        {
            Classes = { "primary" },
            Content = IsZh ? "生成 Dry Run" : "Generate Dry Run",
            MinWidth = 132,
            HorizontalContentAlignment = HorizontalAlignment.Center,
            VerticalContentAlignment = VerticalAlignment.Center
        };
        dryRunButton.Click += OnGenerateDryRunClicked;

        var simulateButton = new Button
        {
            Classes = { "secondary" },
            Content = IsZh ? "安全模拟" : "Simulate",
            MinWidth = 112,
            HorizontalContentAlignment = HorizontalAlignment.Center,
            VerticalContentAlignment = VerticalAlignment.Center
        };
        simulateButton.Click += OnSimulateExecutionClicked;

        var buttonPanel = new StackPanel
        {
            Orientation = Orientation.Horizontal,
            Spacing = 10,
            Children = { dryRunButton, simulateButton }
        };
        Grid.SetColumn(buttonPanel, 1);

        return new Border
        {
            Classes = { "card" },
            Padding = new Thickness(18),
            Child = new Grid
            {
                ColumnDefinitions = new ColumnDefinitions("*,Auto"),
                ColumnSpacing = 16,
                Children =
                {
                    new StackPanel
                    {
                        Spacing = 7,
                        Children =
                        {
                            new TextBlock
                            {
                                Text = IsZh ? "优化规则预演" : "Optimization Rule Dry Run",
                                FontSize = 18,
                                FontWeight = FontWeight.SemiBold
                            },
                            _dryRunStatusText
                        }
                    },
                    buttonPanel
                }
            }
        };
    }

    private (string Title, string Body, string Badge, string Accent)[] GetNetworkDiagnosticRows(NetworkDiagnosticsReport report)
    {
        var bestLatency = report.LatencyResults
            .Where(result => result.FailureRatePercent < 100)
            .OrderBy(result => result.AverageLatencyMs)
            .FirstOrDefault();
        var bestDns = report.DnsBenchmarkResults
            .Where(result => result.FailureRatePercent < 100)
            .OrderBy(result => result.AverageLatencyMs)
            .FirstOrDefault();
        var dns360 = report.DnsBenchmarkResults.FirstOrDefault(result => result.Resolver.Provider == "360");

        if (IsZh)
        {
            return new[]
            {
                ("轻量测速结果", FormatNetworkDiagnosticsSummary(report), "真实", GetNetworkAccent(report.SpeedResult.QualityLevel)),
                ("最佳延迟节点", bestLatency is null ? "未获得可用 Ping 结果。" : $"{bestLatency.Name} · {bestLatency.AverageLatencyMs:0.0} ms · 抖动 {bestLatency.JitterMs:0.0} ms · 失败率 {bestLatency.FailureRatePercent:0.0}%", "Ping", bestLatency is null ? "red" : GetNetworkAccent(bestLatency.QualityLevel)),
                ("360 DNS 基准", dns360 is null ? "360 DNS 尚未返回有效基准结果。" : $"{dns360.Resolver.Name} · 解析 {dns360.AverageLatencyMs:0.0} ms · 抖动 {dns360.JitterMs:0.0} ms · 失败率 {dns360.FailureRatePercent:0.0}%", "DNS", dns360 is null ? "amber" : GetDnsAccent(dns360.Recommendation)),
                ("最佳 DNS 候选", bestDns is null ? "未获得可推荐 DNS，暂不建议切换。" : $"{bestDns.Resolver.Name} · 推荐级别 {TranslateDnsRecommendation(bestDns.Recommendation)}。真实切换仍需备份与确认。", "推荐", bestDns is null ? "amber" : GetDnsAccent(bestDns.Recommendation))
            };
        }

        return new[]
        {
            ("Quick Test Result", FormatNetworkDiagnosticsSummary(report), "Live", GetNetworkAccent(report.SpeedResult.QualityLevel)),
            ("Best Latency Target", bestLatency is null ? "No usable ping result." : $"{bestLatency.Name} · {bestLatency.AverageLatencyMs:0.0} ms · jitter {bestLatency.JitterMs:0.0} ms · failure {bestLatency.FailureRatePercent:0.0}%", "Ping", bestLatency is null ? "red" : GetNetworkAccent(bestLatency.QualityLevel)),
            ("360 DNS Benchmark", dns360 is null ? "360 DNS has no valid benchmark result yet." : $"{dns360.Resolver.Name} · lookup {dns360.AverageLatencyMs:0.0} ms · jitter {dns360.JitterMs:0.0} ms · failure {dns360.FailureRatePercent:0.0}%", "DNS", dns360 is null ? "amber" : GetDnsAccent(dns360.Recommendation)),
            ("Best DNS Candidate", bestDns is null ? "No DNS recommendation yet; do not switch." : $"{bestDns.Resolver.Name} · recommendation {bestDns.Recommendation}. Real switching still requires backup and confirmation.", "Recommend", bestDns is null ? "amber" : GetDnsAccent(bestDns.Recommendation))
        };
    }

    private (string Title, string Body, string Badge, string Accent)[] GetDnsDiagnosticRows(NetworkDiagnosticsReport report)
    {
        var results = report.DnsBenchmarkResults
            .OrderBy(result => result.FailureRatePercent)
            .ThenBy(result => result.AverageLatencyMs <= 0 ? double.MaxValue : result.AverageLatencyMs)
            .ToArray();

        if (results.Length == 0)
        {
            return IsZh
                ? [("DNS 基准测试", "尚未获得 DNS 基准结果。请先进入网络测速页面执行轻量测试。", "待测试", "amber")]
                : [("DNS Benchmark", "No DNS benchmark result yet. Run a quick test from Network Speed Test first.", "Pending", "amber")];
        }

        return results
            .Select(result => (
                Title: result.Resolver.Name,
                Body: IsZh
                    ? $"解析 {result.AverageLatencyMs:0.0} ms，抖动 {result.JitterMs:0.0} ms，失败率 {result.FailureRatePercent:0.0}%。真实切换仍处于禁用状态。"
                    : $"Lookup {result.AverageLatencyMs:0.0} ms, jitter {result.JitterMs:0.0} ms, failure {result.FailureRatePercent:0.0}%. Real switching remains disabled.",
                Badge: IsZh ? TranslateDnsRecommendation(result.Recommendation) : result.Recommendation.ToString(),
                Accent: GetDnsAccent(result.Recommendation)))
            .Take(4)
            .ToArray();
    }

    private (string Title, string Body, string Badge, string Accent)[] GetGameSessionRows(GameSessionAnalysis analysis)
    {
        if (IsZh)
        {
            return new[]
            {
                ("会话状态", TranslateGameSessionExplanation(analysis), TranslateGameSessionState(analysis.State), GetGameSessionAccent(analysis.State)),
                ("游戏库", _gameLibrary.Count == 0 ? "游戏库为空。可通过添加游戏按钮加入 EXE。" : $"已启用 {_gameLibrary.Count(entry => entry.IsEnabled)} / {_gameLibrary.Count} 个条目。", "本地", "blue"),
                ("主候选进程", analysis.PrimaryCandidate is null ? "未识别到主候选进程。" : $"{analysis.PrimaryCandidate.Name} · PID {analysis.PrimaryCandidate.ProcessId} · {TranslateGameRole(analysis.PrimaryCandidate.Role)} · 置信度 {analysis.PrimaryCandidate.Confidence:P0}", "只读", analysis.PrimaryCandidate is null ? "amber" : "green"),
                ("安全边界", "不注入、不改内存、不改游戏文件、不规避反作弊；当前仅做识别。", "保护", "green")
            };
        }

        return new[]
        {
            ("Session State", analysis.Explanation, analysis.State.ToString(), GetGameSessionAccent(analysis.State)),
            ("Game Library", _gameLibrary.Count == 0 ? "Game library is empty. Add an EXE with the Add Game button." : $"{_gameLibrary.Count(entry => entry.IsEnabled)} / {_gameLibrary.Count} entries enabled.", "Local", "blue"),
            ("Primary Candidate", analysis.PrimaryCandidate is null ? "No primary candidate detected." : $"{analysis.PrimaryCandidate.Name} · PID {analysis.PrimaryCandidate.ProcessId} · {analysis.PrimaryCandidate.Role} · confidence {analysis.PrimaryCandidate.Confidence:P0}", "Read-only", analysis.PrimaryCandidate is null ? "amber" : "green"),
            ("Safety Boundary", "No injection, no memory modification, no game file modification, and no anti-cheat bypass. Detection only.", "Protect", "green")
        };
    }

    private (string Title, string Body, string Badge, string Accent)[] GetGameBoostRows(GameBoostPlan plan)
    {
        return plan.Actions
            .Take(4)
            .Select(action => (
                Title: IsZh ? TranslateGameBoostAction(action) : action.Name,
                Body: IsZh
                    ? $"{TranslateGameBoostState(action.State)}：{action.Reason} 安全：{action.SafetyNote}"
                    : $"{action.State}: {action.Reason} Safety: {action.SafetyNote}",
                Badge: IsZh ? TranslateGameBoostState(action.State) : action.State.ToString(),
                Accent: GetGameBoostAccent(action.State)))
            .ToArray();
    }

    private (string Title, string Body, string Badge, string Accent)[] GetMonitorRows(MonitorSnapshot snapshot)
    {
        var top = snapshot.TopMemoryProcesses.FirstOrDefault();
        var warning = snapshot.Warnings.FirstOrDefault();

        if (IsZh)
        {
            return new[]
            {
                ("AETHER 自身占用", $"CPU 估算 {snapshot.AppCpuPercent:0.00}% · 内存 {snapshot.AppMemoryMb} MB。", "自身", snapshot.AppCpuPercent <= 1 && snapshot.AppMemoryMb <= 150 ? "green" : "amber"),
                ("采样策略", snapshot.Method, "单次", "green"),
                ("进程压力", top is null ? "未读取到进程列表。" : $"{top.Name} · PID {top.ProcessId} · {FormatMb(top.MemoryMb)}。", "Top", top?.ImpactLevel == ProcessImpactLevel.High ? "amber" : "blue"),
                ("监控提示", warning is null ? "暂无监控提示。" : warning.Detail, warning is null ? "信息" : TranslateMonitorSeverity(warning.Severity), warning is null ? "blue" : GetMonitorWarningAccent(warning.Severity))
            };
        }

        return new[]
        {
            ("AETHER Overhead", $"CPU estimate {snapshot.AppCpuPercent:0.00}% · memory {snapshot.AppMemoryMb} MB.", "Self", snapshot.AppCpuPercent <= 1 && snapshot.AppMemoryMb <= 150 ? "green" : "amber"),
            ("Sampling Policy", snapshot.Method, "Once", "green"),
            ("Process Pressure", top is null ? "No process list captured." : $"{top.Name} · PID {top.ProcessId} · {FormatMb(top.MemoryMb)}.", "Top", top?.ImpactLevel == ProcessImpactLevel.High ? "amber" : "blue"),
            ("Monitor Note", warning?.Detail ?? "No monitor warning.", warning?.Severity.ToString() ?? "Info", warning is null ? "blue" : GetMonitorWarningAccent(warning.Severity))
        };
    }

    private (string Title, string Body, string Badge, string Accent)[] GetDryRunRows(OptimizationDryRunReport report)
    {
        var previews = report.Previews.Take(4).ToArray();
        if (previews.Length == 0)
        {
            return IsZh
                ? [("Dry Run", "暂无规则预览。", "空", "amber")]
                : [("Dry Run", "No rule previews.", "Empty", "amber")];
        }

        return previews
            .Select(preview => (
                Title: IsZh ? TranslateRuleName(preview.Rule) : preview.Rule.Name,
                Body: IsZh
                    ? $"{TranslatePreviewState(preview.State)}：{preview.Reason} 备份：{preview.Rule.BackupMethod} 回滚：{preview.Rule.RollbackMethod}"
                    : $"{preview.State}: {preview.Reason} Backup: {preview.Rule.BackupMethod} Rollback: {preview.Rule.RollbackMethod}",
                Badge: IsZh ? TranslatePreviewState(preview.State) : preview.State.ToString(),
                Accent: GetPreviewAccent(preview.State)))
            .ToArray();
    }

    private (string Title, string Body, string Badge, string Accent)[] GetExecutionRows(OptimizationExecutionReport report)
    {
        return report.Results
            .Take(4)
            .Select(result => (
                Title: IsZh ? TranslateExecutionRuleName(result.RuleName) : result.RuleName,
                Body: IsZh
                    ? $"{TranslateExecutionStatus(result.Status)}：{result.Message} 验证：{result.VerificationResult} 回滚：{result.RollbackState}"
                    : $"{result.Status}: {result.Message} Verification: {result.VerificationResult} Rollback: {result.RollbackState}",
                Badge: IsZh ? TranslateExecutionStatus(result.Status) : result.Status.ToString(),
                Accent: GetExecutionAccent(result.Status)))
            .ToArray();
    }

    private (string Title, string Body, string Badge, string Accent)[] GetToolkitRows()
    {
        return ToolkitCatalog.DefaultItems
            .Take(6)
            .Select(item => (
                Title: IsZh ? TranslateToolkitName(item) : item.Name,
                Body: IsZh
                    ? $"用途：{TranslateToolkitPurpose(item)} 风险：{TranslateToolkitRisk(item)} 回滚：{TranslateToolkitRevert(item)}"
                    : $"Purpose: {item.Purpose} Risk: {item.Risk} Revert: {item.RevertPath}",
                Badge: IsZh ? TranslateToolkitAvailability(item.Availability) : item.Availability.ToString(),
                Accent: GetToolkitAccent(item.Availability)))
            .ToArray();
    }

    private string FormatNetworkDiagnosticsSummary(NetworkDiagnosticsReport report)
    {
        if (!IsZh)
        {
            return $"{report.Summary} Method: {report.SpeedResult.Method}. Bandwidth consumed: {(report.ConsumedBandwidth ? "yes" : "no")}.";
        }

        var bestLatency = report.LatencyResults
            .Where(result => result.FailureRatePercent < 100)
            .OrderBy(result => result.AverageLatencyMs)
            .FirstOrDefault();
        var bestDns = report.DnsBenchmarkResults
            .Where(result => result.FailureRatePercent < 100)
            .OrderBy(result => result.AverageLatencyMs)
            .FirstOrDefault();

        return $"最佳延迟：{bestLatency?.Name ?? "暂无"} {bestLatency?.AverageLatencyMs:0.0} ms；最佳 DNS：{bestDns?.Resolver.Name ?? "暂无"} {bestDns?.AverageLatencyMs:0.0} ms；未进行下载/上传测速。";
    }

    private string TranslateDnsRecommendation(DnsRecommendationLevel recommendation)
    {
        if (!IsZh)
        {
            return recommendation.ToString();
        }

        return recommendation switch
        {
            DnsRecommendationLevel.Recommended => "推荐",
            DnsRecommendationLevel.Candidate => "候选",
            DnsRecommendationLevel.Avoid => "避免",
            _ => "未知"
        };
    }

    private string TranslateGameSessionState(GameSessionState state)
    {
        if (!IsZh)
        {
            return state.ToString();
        }

        return state switch
        {
            GameSessionState.LibraryMatch => "游戏库匹配",
            GameSessionState.GameCandidate => "游戏候选",
            GameSessionState.LauncherCandidate => "启动器候选",
            GameSessionState.NeedsConfirmation => "需要确认",
            _ => "未检测到"
        };
    }

    private string TranslateGameSessionExplanation(GameSessionAnalysis analysis)
    {
        return analysis.State switch
        {
            GameSessionState.LibraryMatch => $"已将运行进程 {analysis.PrimaryCandidate?.Name} 与游戏库条目 {analysis.LibraryMatch?.DisplayName} 匹配。当前仍为只读识别。",
            GameSessionState.GameCandidate => $"检测到可能的游戏进程 {analysis.PrimaryCandidate?.Name}。后续优化前必须用户确认。",
            GameSessionState.LauncherCandidate => $"检测到启动器候选 {analysis.PrimaryCandidate?.Name}。AETHER 不会自动把启动器当作游戏本体。",
            GameSessionState.NeedsConfirmation => "游戏库存在条目，但当前未发现匹配的运行进程。",
            _ => "未检测到运行中的游戏候选。"
        };
    }

    private string TranslateGameBoostAction(GameBoostActionPreview action)
    {
        return action.Category switch
        {
            GameBoostActionCategory.BackgroundPressure => "后台压力复核",
            GameBoostActionCategory.ProcessPriority => "游戏优先级策略",
            GameBoostActionCategory.IoPriority => "I/O 优先级策略",
            GameBoostActionCategory.PowerPlan => "会话电源计划",
            GameBoostActionCategory.NotificationFocus => "通知专注策略",
            GameBoostActionCategory.Restore => "会话恢复路径",
            _ => action.Name
        };
    }

    private string TranslateGameBoostState(GameBoostActionState state)
    {
        if (!IsZh)
        {
            return state.ToString();
        }

        return state switch
        {
            GameBoostActionState.EligibleForFutureExecution => "未来可执行",
            GameBoostActionState.Blocked => "已阻止",
            _ => "仅预览"
        };
    }

    private string TranslateMonitorSeverity(MonitorWarningSeverity severity)
    {
        if (!IsZh)
        {
            return severity.ToString();
        }

        return severity switch
        {
            MonitorWarningSeverity.Risk => "风险",
            MonitorWarningSeverity.Watch => "关注",
            _ => "信息"
        };
    }

    private string TranslateRuleName(OptimizationRule rule)
    {
        return rule.Category switch
        {
            OptimizationRuleCategory.Dns => "DNS 候选切换",
            OptimizationRuleCategory.Startup => "启动项复核",
            OptimizationRuleCategory.PowerPlan => "游戏电源计划",
            OptimizationRuleCategory.BackgroundPressure => "后台压力复核",
            OptimizationRuleCategory.Cleanup => "临时清理预览",
            OptimizationRuleCategory.GameFocus => "游戏专注策略",
            _ => rule.Name
        };
    }

    private string TranslatePreviewState(OptimizationRulePreviewState state)
    {
        if (!IsZh)
        {
            return state.ToString();
        }

        return state switch
        {
            OptimizationRulePreviewState.Eligible => "可预演",
            OptimizationRulePreviewState.Blocked => "已阻止",
            _ => "需更多数据"
        };
    }

    private string TranslateExecutionRuleName(string ruleName)
    {
        return ruleName switch
        {
            "Switch to measured DNS candidate" => "DNS 候选切换",
            "Disable reviewed startup item" => "启动项复核",
            "Switch game-session power plan" => "游戏电源计划",
            "Review background pressure" => "后台压力复核",
            "Preview temporary cleanup" => "临时清理预览",
            "Enable game focus notification policy" => "游戏专注策略",
            _ => ruleName
        };
    }

    private string TranslateExecutionStatus(OptimizationExecutionStatus status)
    {
        if (!IsZh)
        {
            return status.ToString();
        }

        return status switch
        {
            OptimizationExecutionStatus.Simulated => "已模拟",
            OptimizationExecutionStatus.Succeeded => "已成功",
            OptimizationExecutionStatus.Failed => "失败",
            _ => "已阻止"
        };
    }

    private string TranslateToolkitName(ToolkitItem item)
    {
        return item.Category switch
        {
            ToolkitCategory.Startup => "启动项管理",
            ToolkitCategory.Service => "服务审查",
            ToolkitCategory.Power => "电源计划中心",
            ToolkitCategory.Dns => "DNS 中心",
            ToolkitCategory.Network => "网络测试中心",
            ToolkitCategory.Storage => "存储清理",
            ToolkitCategory.Memory => "内存压力检查",
            ToolkitCategory.Gpu => "GPU 检查",
            ToolkitCategory.Restore => "恢复中心",
            ToolkitCategory.Shortcut => "系统快捷入口",
            _ => item.Name
        };
    }

    private string TranslateToolkitPurpose(ToolkitItem item)
    {
        return item.Category switch
        {
            ToolkitCategory.Startup => "复核启动压力，未来支持禁用与恢复。",
            ToolkitCategory.Service => "优化前审查服务候选。",
            ToolkitCategory.Power => "检查当前电源计划，未来支持会话级切换。",
            ToolkitCategory.Dns => "基准测试当前 DNS 与已验证候选。",
            ToolkitCategory.Network => "执行延迟、抖动和 DNS 基准测试。",
            ToolkitCategory.Storage => "删除前预览可清理临时文件。",
            ToolkitCategory.Memory => "检查高内存进程但不自动关闭。",
            ToolkitCategory.Gpu => "检查 GPU、驱动、温度与功耗状态。",
            ToolkitCategory.Restore => "展示未来备份与回滚记录。",
            ToolkitCategory.Shortcut => "打开常用 Windows 设置入口。",
            _ => item.Purpose
        };
    }

    private string TranslateToolkitRisk(ToolkitItem item)
    {
        return item.Category switch
        {
            ToolkitCategory.Service => "服务更改可能影响驱动、启动器、更新或反作弊。",
            ToolkitCategory.Power => "电源更改可能增加发热或耗电。",
            ToolkitCategory.Dns => "错误 DNS 可能影响浏览或延迟。",
            ToolkitCategory.Storage => "错误删除可能移除缓存或用户数据。",
            ToolkitCategory.Memory => "关闭进程可能导致未保存内容丢失。",
            ToolkitCategory.Gpu => "驱动写入可能影响稳定性。",
            _ => item.Risk
        };
    }

    private string TranslateToolkitRevert(ToolkitItem item)
    {
        return item.Category switch
        {
            ToolkitCategory.Startup => "恢复原启动项。",
            ToolkitCategory.Service => "恢复原服务启动方式。",
            ToolkitCategory.Power => "恢复原电源计划 GUID。",
            ToolkitCategory.Dns => "恢复原适配器 DNS。",
            ToolkitCategory.Storage => "仅允许低风险路径，保留删除列表。",
            ToolkitCategory.Restore => "使用记录的恢复点。",
            _ => item.RevertPath
        };
    }

    private string TranslateToolkitAvailability(ToolkitAvailability availability)
    {
        return availability switch
        {
            ToolkitAvailability.Ready => "可用",
            ToolkitAvailability.Preview => "预览",
            ToolkitAvailability.WindowsOnly => "Windows",
            _ => "预留"
        };
    }

    private string TranslateGpuVendor(GpuVendor vendor)
    {
        if (!IsZh)
        {
            return vendor.ToString();
        }

        return vendor switch
        {
            GpuVendor.Nvidia => "NVIDIA",
            GpuVendor.Amd => "AMD",
            GpuVendor.Intel => "Intel",
            GpuVendor.Apple => "Apple",
            GpuVendor.MicrosoftBasic => "微软基础显示",
            _ => "未知厂商"
        };
    }

    private string TranslateGpuTelemetry(GpuTelemetryAvailability availability)
    {
        if (!IsZh)
        {
            return availability.ToString();
        }

        return availability switch
        {
            GpuTelemetryAvailability.NameOnly => "仅名称",
            GpuTelemetryAvailability.DriverOnly => "仅驱动",
            GpuTelemetryAvailability.Partial => "部分",
            GpuTelemetryAvailability.Full => "完整",
            _ => "未知"
        };
    }

    private static string GetNetworkAccent(NetworkQualityLevel qualityLevel)
    {
        return qualityLevel switch
        {
            NetworkQualityLevel.Good => "green",
            NetworkQualityLevel.Watch => "amber",
            NetworkQualityLevel.Poor => "red",
            _ => "blue"
        };
    }

    private static string GetDnsAccent(DnsRecommendationLevel recommendation)
    {
        return recommendation switch
        {
            DnsRecommendationLevel.Recommended => "green",
            DnsRecommendationLevel.Avoid => "red",
            DnsRecommendationLevel.Candidate => "blue",
            _ => "amber"
        };
    }

    private static string GetGameSessionAccent(GameSessionState state)
    {
        return state switch
        {
            GameSessionState.LibraryMatch => "green",
            GameSessionState.GameCandidate => "blue",
            GameSessionState.LauncherCandidate => "amber",
            GameSessionState.NeedsConfirmation => "amber",
            _ => "blue"
        };
    }

    private static string GetGameBoostAccent(GameBoostActionState state)
    {
        return state switch
        {
            GameBoostActionState.EligibleForFutureExecution => "green",
            GameBoostActionState.Blocked => "red",
            _ => "blue"
        };
    }

    private static string GetMonitorWarningAccent(MonitorWarningSeverity severity)
    {
        return severity switch
        {
            MonitorWarningSeverity.Risk => "red",
            MonitorWarningSeverity.Watch => "amber",
            _ => "green"
        };
    }

    private string FormatMonitorSummary(MonitorSnapshot snapshot)
    {
        if (!IsZh)
        {
            return $"AETHER CPU {snapshot.AppCpuPercent:0.00}%, memory {snapshot.AppMemoryMb} MB, processes {snapshot.ProcessCount}.";
        }

        return $"AETHER CPU {snapshot.AppCpuPercent:0.00}%，内存 {snapshot.AppMemoryMb} MB，进程数 {snapshot.ProcessCount}。";
    }

    private string FormatDryRunSummary(OptimizationDryRunReport report)
    {
        if (!IsZh)
        {
            return report.Summary;
        }

        return $"已生成 {report.Previews.Count} 条规则预览：{report.EligibleCount} 条可预演，{report.BlockedCount} 条被阻止；不会执行系统修改。";
    }

    private string FormatExecutionSummary(OptimizationExecutionReport report)
    {
        if (!IsZh)
        {
            return report.Summary;
        }

        var simulated = report.Results.Count(result => result.Status == OptimizationExecutionStatus.Simulated);
        var blocked = report.Results.Count(result => result.Status == OptimizationExecutionStatus.Blocked);
        return $"安全执行模拟完成：{simulated} 条已模拟，{blocked} 条被安全门阻止，真实系统写入仍禁用。";
    }

    private static string GetPreviewAccent(OptimizationRulePreviewState state)
    {
        return state switch
        {
            OptimizationRulePreviewState.Eligible => "green",
            OptimizationRulePreviewState.Blocked => "red",
            _ => "amber"
        };
    }

    private static string GetExecutionAccent(OptimizationExecutionStatus status)
    {
        return status switch
        {
            OptimizationExecutionStatus.Succeeded => "green",
            OptimizationExecutionStatus.Simulated => "blue",
            OptimizationExecutionStatus.Failed => "red",
            _ => "amber"
        };
    }

    private static string GetToolkitAccent(ToolkitAvailability availability)
    {
        return availability switch
        {
            ToolkitAvailability.Ready => "green",
            ToolkitAvailability.Preview => "blue",
            ToolkitAvailability.WindowsOnly => "amber",
            _ => "amber"
        };
    }

    private static IReadOnlyList<GameLibraryEntry> LoadGameLibrary()
    {
        try
        {
            var path = GetGameLibraryPath();
            if (!File.Exists(path))
            {
                return [];
            }

            var json = File.ReadAllText(path);
            return JsonSerializer.Deserialize<List<GameLibraryEntry>>(json) ?? [];
        }
        catch
        {
            return [];
        }
    }

    private static void SaveGameLibrary(IReadOnlyList<GameLibraryEntry> entries)
    {
        var path = GetGameLibraryPath();
        Directory.CreateDirectory(Path.GetDirectoryName(path)!);
        var json = JsonSerializer.Serialize(entries, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(path, json);
    }

    private static string GetGameLibraryPath()
    {
        var appData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        return Path.Combine(appData, "AETHER AGENTIC Studio", "AETHER SENTINEL AI", "game-library.json");
    }

    private Border CreateModuleCard(string title, string body, string badge, string accent)
    {
        IBrush accentBrush = accent switch
        {
            "green" => Brushes.SpringGreen,
            "amber" => new SolidColorBrush(Color.Parse("#F2B84B")),
            "red" => new SolidColorBrush(Color.Parse("#FF4D5E")),
            _ => new SolidColorBrush(Color.Parse("#2F80FF"))
        };

        return new Border
        {
            Classes = { "card" },
            Padding = new Thickness(18),
            MinHeight = 136,
            Child = new StackPanel
            {
                Spacing = 10,
                Children =
                {
                    new Grid
                    {
                        ColumnDefinitions = new ColumnDefinitions("*,Auto"),
                        Children =
                        {
                            new TextBlock
                            {
                                Text = title,
                                FontSize = 18,
                                FontWeight = FontWeight.SemiBold,
                                TextWrapping = TextWrapping.Wrap
                            },
                            CreateBadge(badge, accentBrush)
                        }
                    },
                    new TextBlock
                    {
                        Text = body,
                        FontSize = 13,
                        Foreground = new SolidColorBrush(Color.Parse("#A8B3C2")),
                        TextWrapping = TextWrapping.Wrap
                    }
                }
            }
        };
    }

    private Border CreateBadge(string text, IBrush accentBrush)
    {
        var badge = new Border
        {
            Background = new SolidColorBrush(Color.Parse("#0B141F")),
            BorderBrush = accentBrush,
            BorderThickness = new Thickness(1),
            CornerRadius = new CornerRadius(8),
            Padding = new Thickness(10, 5),
            Child = new TextBlock
            {
                Text = text,
                FontSize = 11,
                FontWeight = FontWeight.SemiBold,
                Foreground = accentBrush
            }
        };
        Grid.SetColumn(badge, 1);
        return badge;
    }
}
