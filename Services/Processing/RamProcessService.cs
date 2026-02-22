using Microsoft.Extensions.Logging;
using WorkspaceMonitor.Dtos;
using WorkspaceMonitor.Services.HwStatsProvider;

namespace WorkspaceMonitor.Services.Processing;

public class RamProcessService : IProcessService
{
    private readonly IHwStatsProvider _hwStatsProvider;
    private readonly InfluxDbService _influxDbService;
    private readonly ILogger _logger;

    public RamProcessService(IHwStatsProvider hwStatsProvider, InfluxDbService influxDbService, ILogger<RamProcessService> logger)
    {
        _hwStatsProvider = hwStatsProvider;
        _influxDbService = influxDbService;
        _logger = logger;
    }
    
    public async Task Process()
    {
        _hwStatsProvider.RefreshRam();
        
        _logger.LogInformation("Processing RAM");
        
        var usage = _hwStatsProvider.GetRamUsage();
        var dto = new RamUsageDto(usage, DateTimeOffset.UtcNow.ToUnixTimeSeconds());
        
        await _influxDbService.WriteRamUsageAsync(dto);
    }
}