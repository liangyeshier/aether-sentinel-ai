using Avalonia.Controls;
using Avalonia.Interactivity;

namespace AetherSentinel.UI;

public partial class MainWindow : Window
{
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

    private void SetLanguage(string language)
    {
        var zh = language == "zh-CN";

        BrandSubtitleText.Text = zh ? "性能智能体" : "Performance Intelligence";

        NavDashboardText.Text = zh ? "仪表盘" : "Dashboard";
        NavPcIntelligenceText.Text = zh ? "电脑智能分析" : "PC Intelligence";
        NavGameOptimizationText.Text = zh ? "游戏优化" : "Game Optimization";
        NavPerformanceMonitorText.Text = zh ? "性能监控" : "Performance Monitor";
        NavOptimizationCenterText.Text = zh ? "优化中心" : "Optimization Center";
        NavAiAdvisorText.Text = zh ? "AI 顾问" : "AI Advisor";
        NavHistoryText.Text = zh ? "历史记录" : "History";
        NavSettingsText.Text = zh ? "设置" : "Settings";

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

        PhasePreviewTitleText.Text = zh ? "Phase 01 预览" : "Phase 01 Preview";
        PhasePreviewBodyText.Text = zh ? "基于 Avalonia 的桌面壳，当前使用模拟性能数据。" : "Avalonia shell with simulated performance data.";

        ZhButton.Classes.Set("active", zh);
        EnButton.Classes.Set("active", !zh);
    }
}
