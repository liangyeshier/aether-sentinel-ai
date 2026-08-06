using System.Diagnostics;
using AetherSentinel.Core.Monitoring;
using AetherSentinel.Core.Scanning;

namespace AetherSentinel.Platforms.Monitoring;

public sealed class LocalLowOverheadMonitor : ILowOverheadMonitor
{
    public async ValueTask<MonitorSnapshot> CaptureOnceAsync(
        MonitorRequest request,
        CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        if (request.Mode == MonitorSamplingMode.Off)
        {
            return new MonitorSnapshot(
                CapturedAt: DateTimeOffset.Now,
                Mode: request.Mode,
                AppCpuPercent: 0,
                AppMemoryMb: GetCurrentProcessMemoryMb(),
                ProcessCount: 0,
                TopMemoryProcesses: [],
                Warnings: [new MonitorWarning("Monitoring off", "No realtime sampling was executed.", MonitorWarningSeverity.Info)],
                Method: "Off mode");
        }

        using var currentProcess = Process.GetCurrentProcess();
        var startCpu = currentProcess.TotalProcessorTime;
        var stopwatch = Stopwatch.StartNew();
        var sampleWindow = ClampSampleWindow(request.SampleWindow, request.Mode);
        await Task.Delay(sampleWindow, cancellationToken);
        currentProcess.Refresh();
        stopwatch.Stop();

        var cpuDelta = currentProcess.TotalProcessorTime - startCpu;
        var appCpuPercent = stopwatch.Elapsed.TotalMilliseconds <= 0
            ? 0
            : cpuDelta.TotalMilliseconds / stopwatch.Elapsed.TotalMilliseconds / Environment.ProcessorCount * 100;
        var topProcesses = CaptureTopMemoryProcesses(request.TopProcessCount);
        var warnings = CreateWarnings(appCpuPercent, currentProcess.WorkingSet64 / 1024 / 1024, topProcesses);

        return new MonitorSnapshot(
            CapturedAt: DateTimeOffset.Now,
            Mode: request.Mode,
            AppCpuPercent: Math.Round(appCpuPercent, 2),
            AppMemoryMb: currentProcess.WorkingSet64 / 1024 / 1024,
            ProcessCount: SafeProcessCount(),
            TopMemoryProcesses: topProcesses,
            Warnings: warnings,
            Method: $"Single {request.Mode} sample over {sampleWindow.TotalMilliseconds:0} ms; no persistent polling");
    }

    private static TimeSpan ClampSampleWindow(TimeSpan requested, MonitorSamplingMode mode)
    {
        var min = mode == MonitorSamplingMode.Light ? 200 : 500;
        var max = mode == MonitorSamplingMode.Light ? 750 : 2000;
        var ms = Math.Clamp(requested.TotalMilliseconds <= 0 ? min : requested.TotalMilliseconds, min, max);
        return TimeSpan.FromMilliseconds(ms);
    }

    private static long GetCurrentProcessMemoryMb()
    {
        using var process = Process.GetCurrentProcess();
        return process.WorkingSet64 / 1024 / 1024;
    }

    private static int SafeProcessCount()
    {
        try
        {
            return Process.GetProcesses().Length;
        }
        catch
        {
            return 0;
        }
    }

    private static IReadOnlyList<ProcessSnapshot> CaptureTopMemoryProcesses(int count)
    {
        try
        {
            return Process.GetProcesses()
                .Select(CreateProcessSnapshotSafely)
                .OfType<ProcessSnapshot>()
                .OrderByDescending(process => process.MemoryMb)
                .Take(Math.Clamp(count, 1, 10))
                .ToArray();
        }
        catch
        {
            return [];
        }
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

    private static IReadOnlyList<MonitorWarning> CreateWarnings(
        double appCpuPercent,
        long appMemoryMb,
        IReadOnlyList<ProcessSnapshot> topProcesses)
    {
        var warnings = new List<MonitorWarning>();

        if (appCpuPercent > 1)
        {
            warnings.Add(new MonitorWarning(
                "AETHER overhead watch",
                $"Current sampling estimated AETHER CPU at {appCpuPercent:0.00}%.",
                MonitorWarningSeverity.Watch));
        }

        if (appMemoryMb > 150)
        {
            warnings.Add(new MonitorWarning(
                "AETHER memory watch",
                $"AETHER working set is {appMemoryMb} MB, above the default budget.",
                MonitorWarningSeverity.Watch));
        }

        var top = topProcesses.FirstOrDefault();
        if (top is { ImpactLevel: ProcessImpactLevel.High })
        {
            warnings.Add(new MonitorWarning(
                "Background memory pressure",
                $"{top.Name} is using {top.MemoryMb} MB.",
                MonitorWarningSeverity.Watch));
        }

        if (warnings.Count == 0)
        {
            warnings.Add(new MonitorWarning(
                "Low-overhead sample healthy",
                "No immediate sampling overhead or top-process memory pressure warning was detected.",
                MonitorWarningSeverity.Info));
        }

        return warnings;
    }
}
