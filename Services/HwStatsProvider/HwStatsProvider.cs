using System.Net.NetworkInformation;
using Hardware.Info;
using System.IO;
using System.Runtime.InteropServices;

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

    public (double Usage, string Name) GetDiskUsageWithName(int diskNum)
    {

        if (diskNum < 0 || diskNum >= _hardwareInfo.DriveList.Count)
        {
            return (0, "Unknown Drive");
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

        if (totalSize == 0) return (0, physicalDrive.Name);

        var usage = (1.0 - (double)totalFree / totalSize) * 100.0;

        return (Math.Round(usage, 2), physicalDrive.Name);

    }
}