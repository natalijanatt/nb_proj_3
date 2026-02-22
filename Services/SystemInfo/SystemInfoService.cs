using Hardware.Info;

namespace WorkspaceMonitor.Services.SystemInfo;

public class SystemInfoService
{
    public SystemInfoService(HardwareInfo hardwareInfo)
    {
        hardwareInfo.RefreshAll();
        CpuCoreCount = hardwareInfo.CpuList.First().CpuCoreList.Count;
        DiskCount= hardwareInfo.DriveList.Count;
    }

    public int CpuCoreCount { get; }
    public int DiskCount { get; }
}
