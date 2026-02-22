namespace WorkspaceMonitor.Services.HwStatsProvider;

public interface IHwStatsProvider
{
    ulong GetCpuCorePercentUsage(int coreNum);

    void RefreshCpu();

    void RefreshDisk();
    (double Usage, string Name,double UsageGB) GetDiskUsageWithName(int diskNum);
}