using WorkspaceMonitor.Dtos;
using WorkspaceMonitor.Services.HwStatsProvider;
using WorkspaceMonitor.Services.SystemInfo;

namespace WorkspaceMonitor.Services.Processing;

public class BatteryProcessService : IProcessService
{
    private readonly IHwStatsProvider _hwStatsProvider;
    private readonly SystemInfoService _systemInfoService;
    private readonly InfluxDbService _influxDbService;

    public BatteryProcessService(IHwStatsProvider hwStatsProvider, SystemInfoService systemInfoService, InfluxDbService influxDbService)
    {
        _hwStatsProvider = hwStatsProvider;
        _systemInfoService = systemInfoService;
        _influxDbService = influxDbService;
    }

    public async Task Process()
    {
        _hwStatsProvider.RefreshBattery();
        int batteryCount = _systemInfoService.BatteryCount;

        Console.WriteLine("Processing battery");

        List<Task> batteryTasks = [];

        for (int i = 0; i < batteryCount; i++)
        {
            batteryTasks.Add(ProcessBattery(i));
        }

        await Task.WhenAll(batteryTasks);
    }

    private async Task ProcessBattery(int index)
    {
        Console.WriteLine($"Processing battery {index}");

        var dto = new BatteryDto(
            BatteryIndex: index,
            EstimatedChargeRemaining: _hwStatsProvider.GetBatteryChargeRemaining(index),
            BatteryStatus: _hwStatsProvider.GetBatteryStatus(index),  // 3=Full, 6=Charging, 1=Discharging
            EstimatedRunTime: _hwStatsProvider.GetBatteryEstimatedRunTime(index),
            TimeToFullCharge: _hwStatsProvider.GetBatteryTimeToFullCharge(index),
            Timestamp: DateTimeOffset.UtcNow.ToUnixTimeSeconds()
        );

        await _influxDbService.WriteBatteryAsync(dto);
    }
}
