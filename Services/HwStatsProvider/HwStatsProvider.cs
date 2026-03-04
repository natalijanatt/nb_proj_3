using System.Net.NetworkInformation;
using Hardware.Info;
using System.IO;
using System.Runtime.InteropServices;

namespace WorkspaceMonitor.Services.HwStatsProvider;

public class HwStatsProvider : IHwStatsProvider
{
    private readonly HardwareInfo _hardwareInfo;
    private readonly HashSet<string> _physicalAdapterNames;

    public HwStatsProvider(HardwareInfo hardwareInfo)
    {
        _hardwareInfo = hardwareInfo;
        _physicalAdapterNames = NetworkInterface.GetAllNetworkInterfaces()
            .Where(ni => ni.NetworkInterfaceType != NetworkInterfaceType.Loopback
                      && ni.NetworkInterfaceType != NetworkInterfaceType.Tunnel
                      && ni.OperationalStatus == OperationalStatus.Up)
            .Select(ni => ni.Name)
            .ToHashSet();
    }

    public ulong GetCpuCorePercentUsage(int coreNum)
    {
        return _hardwareInfo.CpuList.First().CpuCoreList[coreNum].PercentProcessorTime;
    }

    public void RefreshCpu()
    {
        _hardwareInfo.RefreshCPUList();
    }

    public double GetRamUsage()
    {  
        var available = _hardwareInfo.MemoryStatus.AvailablePhysical;
        var total = _hardwareInfo.MemoryStatus.TotalPhysical;
        
        var usage = (1.0 - available / (double)total) * 100.0;
        
        return Math.Round(usage, 2);
    }

    public void RefreshRam()
    {
        _hardwareInfo.RefreshMemoryStatus();
    }

    public void RefreshBattery()
    {
        _hardwareInfo.RefreshBatteryList();
    }

    public ushort GetBatteryChargeRemaining(int index)
    {
        return _hardwareInfo.BatteryList[index].EstimatedChargeRemaining;
    }

    public ushort GetBatteryStatus(int index)
    {
        return _hardwareInfo.BatteryList[index].BatteryStatus;
    }

    public uint GetBatteryEstimatedRunTime(int index)
    {
        return _hardwareInfo.BatteryList[index].EstimatedRunTime;
    }

    public uint GetBatteryTimeToFullCharge(int index)
    {
        return _hardwareInfo.BatteryList[index].TimeToFullCharge;
    }

    public void RefreshDisk()
    {
        _hardwareInfo.RefreshDriveList();
    }

    public void RefreshNetwork()
    {
        _hardwareInfo.RefreshNetworkAdapterList(includeBytesPerSec: true);
    }

    public int GetNetworkAdapterCount()
    {
        return _hardwareInfo.NetworkAdapterList
            .Count(a => _physicalAdapterNames.Contains(a.NetConnectionID));
    }

    public (ulong BytesReceivedPerSec, ulong BytesSentPerSec, string Name) GetNetworkAdapterStats(int index)
    {
        var adapter = _hardwareInfo.NetworkAdapterList
            .Where(a => _physicalAdapterNames.Contains(a.NetConnectionID))
            .ToList()[index];
        return (adapter.BytesReceivedPersec, adapter.BytesSentPersec, adapter.Name);
    }
    public (double Usage, string Name,double UsageGB ) GetDiskUsageWithName(int diskNum)
    {

        if (diskNum < 0 || diskNum >= _hardwareInfo.DriveList.Count)
        {
            return (0, "Unknown Drive",0);
        }

        var physicalDrive = _hardwareInfo.DriveList[diskNum];

        bool isWindows = RuntimeInformation.IsOSPlatform(OSPlatform.Windows);

        var volumeNames = physicalDrive.PartitionList
        .SelectMany(p => p.VolumeList)
        .Select(v => v.Name)
        .ToList();

        var myLogicalDrives = DriveInfo.GetDrives()
        .Where(d =>
        {
            if (!d.IsReady) return false;

            if (isWindows)
            {
                var driveLetter = d.Name.Substring(0, 1).ToUpper();
                return volumeNames.Any(vn => vn.ToUpper().StartsWith(driveLetter));
            }
            else
            {
                return volumeNames.Contains(d.Name);
            }
        })
        .ToList();

        long totalSize = myLogicalDrives.Sum(d => d.TotalSize);
        long totalFree = myLogicalDrives.Sum(d => d.AvailableFreeSpace);

        if (totalSize == 0) return (0, physicalDrive.Name,0);

        var usage = (1.0 - (double)totalFree / totalSize) * 100.0;

        long usedBytes = totalSize - totalFree;
        double usedGb = usedBytes / (1024.0 * 1024.0 * 1024.0);

        return (Math.Round(usage, 2), physicalDrive.Name,Math.Round(usedGb, 2));

    }
}