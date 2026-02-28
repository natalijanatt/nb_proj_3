using Hardware.Info;
using System.Net.NetworkInformation;

namespace WorkspaceMonitor.Services.SystemInfo;

public class SystemInfoService
{
    public SystemInfoService(HardwareInfo hardwareInfo)
    {
        hardwareInfo.RefreshAll();
        CpuCoreCount = hardwareInfo.CpuList.First().CpuCoreList.Count;
        BatteryCount = hardwareInfo.BatteryList.Count;
        DiskCount = hardwareInfo.DriveList.Count;
        NetworkAdapterCount = NetworkInterface.GetAllNetworkInterfaces()
            .Count(ni => ni.NetworkInterfaceType != NetworkInterfaceType.Loopback
                      && ni.NetworkInterfaceType != NetworkInterfaceType.Tunnel
                      && ni.OperationalStatus == OperationalStatus.Up);
    }

    public int CpuCoreCount { get; }
    public int BatteryCount { get; }
    public int DiskCount { get; }
    public int NetworkAdapterCount { get; }
}
