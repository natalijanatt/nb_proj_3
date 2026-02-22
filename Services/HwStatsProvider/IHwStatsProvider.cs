namespace WorkspaceMonitor.Services.HwStatsProvider;

public interface IHwStatsProvider
{
    ulong GetCpuCorePercentUsage(int coreNum);

    void RefreshCpu();

    void RefreshBattery();

    ushort GetBatteryChargeRemaining(int index);

    ushort GetBatteryStatus(int index);

    uint GetBatteryEstimatedRunTime(int index);

    uint GetBatteryTimeToFullCharge(int index);
}