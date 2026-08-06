namespace AetherSentinel.Core.Toolkit;

public static class ToolkitCatalog
{
    public static IReadOnlyList<ToolkitItem> DefaultItems { get; } =
    [
        new("tool.startup.manager", "Startup Manager", ToolkitCategory.Startup, ToolkitAvailability.Preview, "Review startup pressure and future disable/restore actions.", "Disabling required startup software can break workflows.", "Restore original startup entry."),
        new("tool.service.review", "Service Review", ToolkitCategory.Service, ToolkitAvailability.WindowsOnly, "Review service candidates before optimization.", "Service changes can break drivers, launchers, updates, and anti-cheat.", "Restore original service startup mode."),
        new("tool.power.center", "Power Plan Center", ToolkitCategory.Power, ToolkitAvailability.Preview, "Inspect active power plan and future session plan switching.", "Power changes can increase heat or battery usage.", "Restore original power plan GUID."),
        new("tool.dns.center", "DNS Center", ToolkitCategory.Dns, ToolkitAvailability.Ready, "Benchmark current DNS and verified candidates.", "Bad DNS can break browsing or increase latency.", "Restore original adapter DNS settings."),
        new("tool.network.test", "Network Test Center", ToolkitCategory.Network, ToolkitAvailability.Ready, "Run latency, jitter, and DNS benchmark checks.", "Full tests consume traffic and must require consent.", "No rollback needed for read-only tests."),
        new("tool.storage.cleanup", "Storage Cleanup", ToolkitCategory.Storage, ToolkitAvailability.Preview, "Preview removable temporary files before deletion.", "Deleting wrong files can remove caches or user data.", "Use deletion list and avoid irreversible paths."),
        new("tool.memory.pressure", "Memory Pressure Inspector", ToolkitCategory.Memory, ToolkitAvailability.Ready, "Inspect top memory processes without closing them.", "Closing processes can lose work.", "No automatic close in current implementation."),
        new("tool.gpu.inspector", "GPU Inspector", ToolkitCategory.Gpu, ToolkitAvailability.Reserved, "Inspect GPU vendor, driver, thermal, and power state.", "Driver writes can affect stability.", "Restore original driver setting values."),
        new("tool.restore.center", "Restore Center", ToolkitCategory.Restore, ToolkitAvailability.Preview, "Show future backup and rollback records.", "Incomplete restore points are unsafe.", "Use recorded restore point."),
        new("tool.system.shortcuts", "System Shortcuts", ToolkitCategory.Shortcut, ToolkitAvailability.Reserved, "Open common Windows settings panels.", "Shortcut-only actions should not change system state directly.", "No rollback needed for shortcut launch.")
    ];
}
