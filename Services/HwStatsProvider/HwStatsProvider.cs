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
}