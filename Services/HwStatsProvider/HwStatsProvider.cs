using System.Net.NetworkInformation;
using Hardware.Info;

namespace WorkspaceMonitor.Services.HwStatsProvider;

public class HwStatsProvider : IHwStatsProvider
{
    private readonly HardwareInfo _hardwareInfo;

    public HwStatsProvider(HardwareInfo hardwareInfo)
    {
        _hardwareInfo = hardwareInfo;
    }

    public ulong GetCpuCorePercentUsage(int coreNum)
    {
        return _hardwareInfo.CpuList.First().CpuCoreList[coreNum].PercentProcessorTime;
    }

    public void RefreshCpu()
    {
        _hardwareInfo.RefreshCPUList();
    }

    public ulong GetRamUsage()
    {  
        var available = _hardwareInfo.MemoryStatus.AvailablePhysical;
        var total = _hardwareInfo.MemoryStatus.TotalPhysical;
        
        return (ulong) ((1.0 - available / (double)total) * 100.0);
    }

    public void RefreshRam()
    {
        _hardwareInfo.RefreshMemoryStatus();
    }
}