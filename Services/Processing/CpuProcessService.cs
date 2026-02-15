using WorkspaceMonitor.Dtos;
using WorkspaceMonitor.Services.HwStatsProvider;
using WorkspaceMonitor.Services.SystemInfo;

namespace WorkspaceMonitor.Services.Processing;

public class CpuProcessService : IProcessService
{
    private readonly IHwStatsProvider _hwStatsProvider;
    private readonly SystemInfoService _systemInfoService;
    private readonly InfluxDbService _influxDbService;

    public CpuProcessService(IHwStatsProvider hwStatsProvider, SystemInfoService systemInfoService, InfluxDbService influxDbService)
    {
        _hwStatsProvider = hwStatsProvider;
        _systemInfoService = systemInfoService;
        _influxDbService = influxDbService;
    }
    
    public async Task Process()
    {
        _hwStatsProvider.RefreshCpu();
        int coreCount = _systemInfoService.CpuCoreCount;

        for (int coreNum = 0; coreNum < coreCount; coreNum++)
        {
            var usage = _hwStatsProvider.GetCpuCorePercentUsage(coreNum);
            
            var dto = new CpuCoreUsageDto(coreNum, usage, DateTimeOffset.UtcNow.ToUnixTimeSeconds());

            await _influxDbService.WriteCpuCoreUsageAsync(dto);
        }
    }
}