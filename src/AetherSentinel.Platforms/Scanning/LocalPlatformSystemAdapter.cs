using System.Diagnostics;
using System.Net.NetworkInformation;
using System.Runtime.InteropServices;
using AetherSentinel.Core.Scanning;

namespace AetherSentinel.Platforms.Scanning;

public sealed class LocalPlatformSystemAdapter : IPlatformSystemAdapter
{
    public string PlatformName => RuntimeInformation.OSDescription;

    public ValueTask<PlatformCapabilitySet> GetCapabilitiesAsync(CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        return ValueTask.FromResult(new PlatformCapabilitySet(
            CanReadHardware: true,
            CanReadProcesses: true,
            CanReadStartupItems: false,
            CanReadNetworkInterfaces: true,
            CanReadDnsConfiguration: OperatingSystem.IsMacOS() || OperatingSystem.IsWindows(),
            CanRunNetworkSpeedTest: false,
            MissingPermissions: []));
    }

    public async ValueTask<SystemSnapshot> CaptureReadOnlySnapshotAsync(
        ScanRequest request,
        CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var operatingSystem = CreateOperatingSystemSnapshot();
        var storage = CreateStorageSnapshots();
        var hardware = new HardwareSnapshot(
            CpuName: await ReadCpuNameAsync(cancellationToken),
            GpuName: "Read-only adapter pending",
            MemoryTotalMb: await ReadTotalMemoryMbAsync(cancellationToken),
            MemoryUsedMb: await ReadUsedMemoryMbAsync(cancellationToken),
            Storage: storage);

        var processes = request.IncludeProcesses
            ? CreateTopProcessSnapshots()
            : [];

        var network = request.IncludeNetwork
            ? await CreateNetworkSnapshotAsync(request.IncludeDns, cancellationToken)
            : CreateEmptyNetworkSnapshot();

        var insights = CreateInsights(hardware, processes, network);

        return new SystemSnapshot(
            CapturedAt: DateTimeOffset.Now,
            OperatingSystem: operatingSystem,
            Hardware: hardware,
            TopProcesses: processes,
            Network: network,
            Insights: insights);
    }

    private static OperatingSystemSnapshot CreateOperatingSystemSnapshot()
    {
        return new OperatingSystemSnapshot(
            Name: RuntimeInformation.OSDescription.Trim(),
            Version: Environment.OSVersion.VersionString,
            Architecture: RuntimeInformation.OSArchitecture.ToString(),
            DeviceName: Environment.MachineName);
    }

    private static IReadOnlyList<StorageSnapshot> CreateStorageSnapshots()
    {
        return DriveInfo.GetDrives()
            .Where(drive => drive.IsReady && drive.DriveType is DriveType.Fixed or DriveType.Removable)
            .Select(drive =>
            {
                var totalGb = BytesToGb(drive.TotalSize);
                var freeGb = BytesToGb(drive.AvailableFreeSpace);
                var activePercent = drive.TotalSize <= 0
                    ? 0
                    : Math.Round((1 - (double)drive.AvailableFreeSpace / drive.TotalSize) * 100, 1);

                return new StorageSnapshot(drive.Name, totalGb, freeGb, activePercent);
            })
            .OrderByDescending(drive => drive.ActivePercent)
            .Take(4)
            .ToArray();
    }

    private static IReadOnlyList<ProcessSnapshot> CreateTopProcessSnapshots()
    {
        return Process.GetProcesses()
            .Select(CreateProcessSnapshotSafely)
            .OfType<ProcessSnapshot>()
            .OrderByDescending(process => process.MemoryMb)
            .Take(5)
            .ToArray();
    }

    private static ProcessSnapshot? CreateProcessSnapshotSafely(Process process)
    {
        try
        {
            var memoryMb = process.WorkingSet64 / 1024 / 1024;
            return new ProcessSnapshot(
                Name: process.ProcessName,
                ProcessId: process.Id,
                CpuPercent: 0,
                MemoryMb: memoryMb,
                ImpactLevel: memoryMb switch
                {
                    >= 1024 => ProcessImpactLevel.High,
                    >= 300 => ProcessImpactLevel.Medium,
                    _ => ProcessImpactLevel.Low
                });
        }
        catch
        {
            return null;
        }
        finally
        {
            process.Dispose();
        }
    }

    private static async ValueTask<NetworkSnapshot> CreateNetworkSnapshotAsync(
        bool includeDns,
        CancellationToken cancellationToken)
    {
        var primaryInterface = NetworkInterface.GetAllNetworkInterfaces()
            .Where(networkInterface =>
                networkInterface.OperationalStatus == OperationalStatus.Up &&
                networkInterface.NetworkInterfaceType != NetworkInterfaceType.Loopback)
            .OrderByDescending(networkInterface => networkInterface.Speed)
            .FirstOrDefault();

        var dnsServers = includeDns
            ? await ReadDnsServersAsync(cancellationToken)
            : [];

        return new NetworkSnapshot(
            PrimaryInterfaceName: primaryInterface?.Name ?? "Unknown",
            ConnectionType: primaryInterface?.NetworkInterfaceType.ToString() ?? "Unknown",
            CurrentDnsServers: dnsServers,
            IspRegion: new IspRegionInfo(
                Country: "Unknown",
                Region: "Unknown",
                City: "Unknown",
                Isp: "Unknown",
                Source: "Not implemented",
                Confidence: 0),
            Quality: new NetworkQualitySnapshot(
                LatencyMs: 0,
                JitterMs: 0,
                PacketLossPercent: 0,
                QualityLevel: NetworkQualityLevel.Unknown));
    }

    private static NetworkSnapshot CreateEmptyNetworkSnapshot()
    {
        return new NetworkSnapshot(
            PrimaryInterfaceName: "Not scanned",
            ConnectionType: "Unknown",
            CurrentDnsServers: [],
            IspRegion: new IspRegionInfo("Unknown", "Unknown", "Unknown", "Unknown", "Not scanned", 0),
            Quality: new NetworkQualitySnapshot(0, 0, 0, NetworkQualityLevel.Unknown));
    }

    private static IReadOnlyList<SystemInsight> CreateInsights(
        HardwareSnapshot hardware,
        IReadOnlyList<ProcessSnapshot> processes,
        NetworkSnapshot network)
    {
        var insights = new List<SystemInsight>
        {
            new(
                Title: "Read-only scan completed",
                Detail: "AETHER collected local system data without applying optimization actions.",
                Severity: InsightSeverity.Info,
                Source: "LocalPlatformSystemAdapter")
        };

        if (hardware.Storage.Any(storage => storage.ActivePercent >= 85))
        {
            insights.Add(new SystemInsight(
                Title: "Storage pressure",
                Detail: "One or more drives have less free space than recommended for game updates and cache usage.",
                Severity: InsightSeverity.Warning,
                Source: "DriveInfo"));
        }

        if (processes.Any(process => process.ImpactLevel == ProcessImpactLevel.High))
        {
            insights.Add(new SystemInsight(
                Title: "High memory process",
                Detail: "A high-memory process was detected. Future analysis will compare this against game and creator workloads.",
                Severity: InsightSeverity.Warning,
                Source: "Process.WorkingSet64"));
        }

        if (network.CurrentDnsServers.Count == 0)
        {
            insights.Add(new SystemInsight(
                Title: "DNS pending",
                Detail: "DNS servers were not detected on this platform or require a platform-specific adapter.",
                Severity: InsightSeverity.Info,
                Source: "Network adapter"));
        }

        return insights;
    }

    private static async ValueTask<string> ReadCpuNameAsync(CancellationToken cancellationToken)
    {
        if (OperatingSystem.IsMacOS())
        {
            var value = await RunCommandAsync("/usr/sbin/sysctl", cancellationToken, "-n", "machdep.cpu.brand_string");
            if (!string.IsNullOrWhiteSpace(value))
            {
                return value.Trim();
            }
        }

        return RuntimeInformation.ProcessArchitecture.ToString();
    }

    private static async ValueTask<long> ReadTotalMemoryMbAsync(CancellationToken cancellationToken)
    {
        if (OperatingSystem.IsMacOS())
        {
            var value = await RunCommandAsync("/usr/sbin/sysctl", cancellationToken, "-n", "hw.memsize");
            if (long.TryParse(value.Trim(), out var bytes))
            {
                return bytes / 1024 / 1024;
            }
        }

        return GC.GetGCMemoryInfo().TotalAvailableMemoryBytes / 1024 / 1024;
    }

    private static async ValueTask<long> ReadUsedMemoryMbAsync(CancellationToken cancellationToken)
    {
        if (OperatingSystem.IsMacOS())
        {
            var output = await RunCommandAsync("/usr/bin/vm_stat", cancellationToken);
            var pageSize = ParseMacOsPageSize(output);
            var usedPages =
                ParseMacOsPageCount(output, "Pages active") +
                ParseMacOsPageCount(output, "Pages wired down") +
                ParseMacOsPageCount(output, "Pages occupied by compressor");

            if (pageSize > 0 && usedPages > 0)
            {
                return usedPages * pageSize / 1024 / 1024;
            }
        }

        return GC.GetTotalMemory(forceFullCollection: false) / 1024 / 1024;
    }

    private static long ParseMacOsPageSize(string vmStatOutput)
    {
        const string marker = "page size of ";
        var markerIndex = vmStatOutput.IndexOf(marker, StringComparison.OrdinalIgnoreCase);
        if (markerIndex < 0)
        {
            return 0;
        }

        var start = markerIndex + marker.Length;
        var end = vmStatOutput.IndexOf(" bytes", start, StringComparison.OrdinalIgnoreCase);
        if (end <= start)
        {
            return 0;
        }

        return long.TryParse(vmStatOutput[start..end], out var pageSize) ? pageSize : 0;
    }

    private static long ParseMacOsPageCount(string vmStatOutput, string label)
    {
        var line = vmStatOutput
            .Split('\n', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
            .FirstOrDefault(value => value.StartsWith(label, StringComparison.OrdinalIgnoreCase));

        if (line is null)
        {
            return 0;
        }

        var digits = new string(line.Where(char.IsDigit).ToArray());
        return long.TryParse(digits, out var pages) ? pages : 0;
    }

    private static async ValueTask<IReadOnlyList<string>> ReadDnsServersAsync(CancellationToken cancellationToken)
    {
        if (OperatingSystem.IsMacOS())
        {
            return await ReadMacOsDnsServersAsync(cancellationToken);
        }

        if (OperatingSystem.IsWindows())
        {
            return await ReadWindowsDnsServersAsync(cancellationToken);
        }

        return [];
    }

    private static async ValueTask<IReadOnlyList<string>> ReadMacOsDnsServersAsync(CancellationToken cancellationToken)
    {
        var servicesOutput = await RunCommandAsync("/usr/sbin/networksetup", cancellationToken, "-listallnetworkservices");
        var services = servicesOutput
            .Split('\n', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
            .Where(line => !line.StartsWith("An asterisk", StringComparison.OrdinalIgnoreCase))
            .Where(line => !line.StartsWith('*'))
            .Take(8)
            .ToArray();

        var servers = new List<string>();
        foreach (var service in services)
        {
            var output = await RunCommandAsync("/usr/sbin/networksetup", cancellationToken, "-getdnsservers", service);
            if (output.Contains("There aren't any DNS Servers", StringComparison.OrdinalIgnoreCase))
            {
                continue;
            }

            servers.AddRange(output
                .Split('\n', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
                .Where(IsPotentialDnsAddress));
        }

        return servers.Distinct(StringComparer.OrdinalIgnoreCase).ToArray();
    }

    private static async ValueTask<IReadOnlyList<string>> ReadWindowsDnsServersAsync(CancellationToken cancellationToken)
    {
        var output = await RunCommandAsync(
            "powershell",
            cancellationToken,
            "-NoProfile",
            "-Command",
            "Get-DnsClientServerAddress -AddressFamily IPv4 | Select-Object -ExpandProperty ServerAddresses");

        return output
            .Split('\n', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
            .Where(IsPotentialDnsAddress)
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .ToArray();
    }

    private static bool IsPotentialDnsAddress(string value)
    {
        return value.Any(char.IsDigit) && !value.Contains(' ') && value.Length <= 45;
    }

    private static async ValueTask<string> RunCommandAsync(
        string fileName,
        CancellationToken cancellationToken,
        params string[] arguments)
    {
        try
        {
            using var process = new Process();
            process.StartInfo = new ProcessStartInfo(fileName)
            {
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            foreach (var argument in arguments)
            {
                process.StartInfo.ArgumentList.Add(argument);
            }

            process.Start();

            var outputTask = process.StandardOutput.ReadToEndAsync(cancellationToken);
            await process.WaitForExitAsync(cancellationToken);
            return await outputTask;
        }
        catch
        {
            return string.Empty;
        }
    }

    private static long BytesToGb(long bytes)
    {
        return bytes / 1024 / 1024 / 1024;
    }
}
