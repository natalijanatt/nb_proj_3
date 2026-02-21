namespace WorkspaceMonitor.Services.HwStatsProvider;

public interface IHwStatsProvider
{
    ulong GetCpuCorePercentUsage(int coreNum);

    void RefreshCpu();
    
    double GetRamUsage();
    void RefreshRam();
}