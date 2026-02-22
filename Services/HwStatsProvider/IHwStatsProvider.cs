namespace WorkspaceMonitor.Services.HwStatsProvider;

public interface IHwStatsProvider
{
    ulong GetCpuCorePercentUsage(int coreNum);

    void RefreshCpu();

    void RefreshDisk();
    (double Usage, String Name) GetDiskUsage(int diskNum);
    }