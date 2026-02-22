using System.Reflection;
using Microsoft.Extensions.Hosting;
using WorkspaceMonitor.Services.Processing;

namespace WorkspaceMonitor.Services.BackgroundWorker;

public class BackgroundWorkerService : BackgroundService
{
    private readonly ICollection<IProcessService> _processServices;

    public BackgroundWorkerService(
        CpuProcessService cpuProcessService,
        RamProcessService ramProcessService,
        BatteryProcessService batteryProcessService
        )
    {
        _processServices = new List<IProcessService>();

        _processServices.Add(cpuProcessService);
        _processServices.Add(batteryProcessService);
        _processServices.Add(ramProcessService);
    }
    
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while(!stoppingToken.IsCancellationRequested)
        {
            List<Task> processTasks = [];
            
            foreach (var processService in _processServices)
            {
                var task = processService.Process();
                processTasks.Add(task);
            }
            
            await Task.WhenAll(processTasks);

            await Task.Delay(1000, stoppingToken);
        }
    }
}