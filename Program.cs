using Hardware.Info;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using WorkspaceMonitor.Services;
using WorkspaceMonitor.Services.BackgroundWorker;
using WorkspaceMonitor.Services.HwStatsProvider;
using WorkspaceMonitor.Services.Processing;
using Microsoft.Extensions.Logging;
using WorkspaceMonitor.Services.SystemInfo;

DotNetEnv.Env.Load();

var builder = Host.CreateDefaultBuilder(args)
    .ConfigureLogging(logging =>
    {
        logging.ClearProviders();
        logging.AddConsole();
    });

//servisi
builder.ConfigureServices(services =>
{
    services.AddSingleton<HardwareInfo>();
    
    services.AddSingleton<IHwStatsProvider, HwStatsProvider>();
    services.AddSingleton<SystemInfoService>();
    services.AddSingleton<InfluxDbService>();

    services.AddSingleton<CpuProcessService>();
    services.AddSingleton<RamProcessService>();
    services.AddSingleton<BatteryProcessService>();
    services.AddSingleton<DiskProcessService>();

    services.AddHostedService<BackgroundWorkerService>();
});

var app = builder.Build();

await app.RunAsync();