namespace WorkspaceMonitor.Services.HwStatsProvider;

public interface IHwStatsProvider
{
    ulong GetCpuCorePercentUsage(int coreNum);

    void RefreshCpu();

    void RefreshDisk();
    (double Usage, string Name) GetDiskUsageWithName(int diskNum);
}