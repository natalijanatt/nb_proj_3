using Microsoft.Extensions.Logging;
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
        
        Console.WriteLine("Processing CPU");

        List<Task> coreProcesses = [];
        
        for (int coreNum = 0; coreNum < coreCount; coreNum++)
        {
            coreProcesses.Add(ProcessCore(coreNum));
        }

        await Task.WhenAll(coreProcesses.ToArray());
    }

    private async Task ProcessCore(int coreNum)
    {
        Console.WriteLine($"Processing core {coreNum}");
            
        var usage = _hwStatsProvider.GetCpuCorePercentUsage(coreNum);
            
        var dto = new CpuCoreUsageDto(coreNum, usage, DateTimeOffset.UtcNow.ToUnixTimeSeconds());

        await _influxDbService.WriteCpuCoreUsageAsync(dto);
    }
}