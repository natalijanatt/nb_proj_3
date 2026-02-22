using Microsoft.Extensions.Logging;
using WorkspaceMonitor.Dtos;
using WorkspaceMonitor.Services.HwStatsProvider;
using WorkspaceMonitor.Services.SystemInfo;

namespace WorkspaceMonitor.Services.Processing;

public class DiskProcessService : IProcessService
{
    private readonly IHwStatsProvider _hwStatsProvider;
    private readonly SystemInfoService _systemInfoService;
    private readonly InfluxDbService _influxDbService;
    private readonly ILogger _logger;


    public DiskProcessService(IHwStatsProvider hwStatsProvider, SystemInfoService systemInfoService, InfluxDbService influxDbService, ILogger<DiskProcessService> logger)
    {
        _hwStatsProvider = hwStatsProvider;
        _systemInfoService = systemInfoService;
        _influxDbService = influxDbService;
        _logger = logger;

    }

    public async Task Process()
    {
        _hwStatsProvider.RefreshDisk();

        int diskCount = _systemInfoService.DiskCount;

        _logger.LogInformation("Processing Disks");

        List<Task> diskProcesses = new();

        for (int diskNum = 0; diskNum < diskCount; diskNum++)
        {
            var task = ProcessDisk(diskNum);
            diskProcesses.Add(task);
        }

        await Task.WhenAll(diskProcesses);

    }

    private async Task ProcessDisk(int diskNum)
    {
        _logger.LogInformation($"Processing Disk {diskNum}");

        var (usage, name) = _hwStatsProvider.GetDiskUsageWithName(diskNum);

        var dto = new DiskUsageDto(name, usage, DateTimeOffset.UtcNow.ToUnixTimeSeconds());

        await _influxDbService.WriteDiskUsageAsync(dto);
    }
}