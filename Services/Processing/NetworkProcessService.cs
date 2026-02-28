using Microsoft.Extensions.Logging;
using WorkspaceMonitor.Dtos;
using WorkspaceMonitor.Services.HwStatsProvider;

namespace WorkspaceMonitor.Services.Processing;

public class NetworkProcessService : IProcessService
{
    private readonly IHwStatsProvider _hwStatsProvider;
    private readonly InfluxDbService _influxDbService;
    private readonly ILogger _logger;

    public NetworkProcessService(IHwStatsProvider hwStatsProvider, InfluxDbService influxDbService, ILogger<NetworkProcessService> logger)
    {
        _hwStatsProvider = hwStatsProvider;
        _influxDbService = influxDbService;
        _logger = logger;
    }

    public async Task Process()
    {
        _hwStatsProvider.RefreshNetwork();
        int count = _hwStatsProvider.GetNetworkAdapterCount();
        _logger.LogInformation("Processing Network");

        var tasks = Enumerable.Range(0, count).Select(ProcessAdapter);
        await Task.WhenAll(tasks);
    }

    private async Task ProcessAdapter(int index)
    {
        var (recv, sent, name) = _hwStatsProvider.GetNetworkAdapterStats(index);
        var dto = new NetworkUsageDto(name, recv, sent, DateTimeOffset.UtcNow.ToUnixTimeSeconds());
        await _influxDbService.WriteNetworkUsageAsync(dto);
    }
}
