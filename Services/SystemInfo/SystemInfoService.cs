using Hardware.Info;

namespace WorkspaceMonitor.Services.SystemInfo;

public class SystemInfoService
{
    public SystemInfoService(HardwareInfo hardwareInfo)
    {
        hardwareInfo.RefreshAll();
        CpuCoreCount = hardwareInfo.CpuList.First().CpuCoreList.Count;
        BatteryCount = hardwareInfo.BatteryList.Count;
    }

    public int CpuCoreCount { get; }
    public int BatteryCount { get; }
}
