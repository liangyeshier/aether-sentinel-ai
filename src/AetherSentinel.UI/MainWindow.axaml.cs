using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Interactivity;
using Avalonia.Layout;
using Avalonia.Media;

namespace AetherSentinel.UI;

public partial class MainWindow : Window
{
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

        PhasePreviewTitleText.Text = zh ? "Phase 02 模块激活" : "Phase 02 Module Activation";
        PhasePreviewBodyText.Text = zh ? "全部主导航已可点击，当前仍为只读与预留能力。" : "All main navigation modules are clickable; capabilities remain read-only or reserved.";

        ZhButton.Classes.Set("active", zh);
        EnButton.Classes.Set("active", !zh);

        NavigateTo(_currentPage);
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

        return new ScrollViewer
        {
            VerticalScrollBarVisibility = ScrollBarVisibility.Auto,
            Content = grid
        };
    }

    private (string Title, string Body, string Badge, string Accent)[] GetModuleRows(string page)
    {
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

    private Border CreateModuleCard(string title, string body, string badge, string accent)
    {
        IBrush accentBrush = accent switch
        {
            "green" => Brushes.SpringGreen,
            "amber" => new SolidColorBrush(Color.Parse("#F2B84B")),
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
