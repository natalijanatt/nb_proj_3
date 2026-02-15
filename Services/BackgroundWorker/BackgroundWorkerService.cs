using System.Reflection;
using Microsoft.Extensions.Hosting;
using WorkspaceMonitor.Services.Processing;

namespace WorkspaceMonitor.Services.BackgroundWorker;

public class BackgroundWorkerService : BackgroundService
{
    private readonly ICollection<IProcessService> _processServices;

    public BackgroundWorkerService(
        CpuProcessService cpuProcessService
        )
    {
        _processServices = new List<IProcessService>();
        
        _processServices.Add(cpuProcessService);
    }
    
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while(!stoppingToken.IsCancellationRequested)
        {
            foreach (var processService in _processServices)
            {
                await processService.Process();
            }

            await Task.Delay(1000, stoppingToken);
        }
    }


}