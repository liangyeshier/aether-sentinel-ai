namespace AetherSentinel.Core.Toolkit;

public sealed record ToolkitItem(
    string Id,
    string Name,
    ToolkitCategory Category,
    ToolkitAvailability Availability,
    string Purpose,
    string Risk,
    string RevertPath);

public enum ToolkitCategory
{
    Startup,
    Service,
    Power,
    Dns,
    Network,
    Storage,
    Memory,
    Gpu,
    Restore,
    Shortcut
}

public enum ToolkitAvailability
{
    Ready,
    Preview,
    WindowsOnly,
    Reserved
}
