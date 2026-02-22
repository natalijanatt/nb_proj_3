using System.Text.Json;
using InfluxDB3.Client;
using InfluxDB3.Client.Write;
using Microsoft.Extensions.Configuration;
using WorkspaceMonitor.Dtos;

namespace WorkspaceMonitor.Services;

public class InfluxDbService
{
    private readonly InfluxDBClient _client;
    private readonly string _database;

    public InfluxDbService(IConfiguration config)
    {
        _database = config["INFLUX_DATABASE"]!;
        
        _client = new InfluxDBClient(
            host: config["INFLUX_URL"]!,
            token: config["INFLUX_TOKEN"]!,
            database: _database,
            organization: config["INFLUX_ORGANIZATION"]
        );
    }
    
    public async Task WriteTestAsync()
    {
        var point = PointData
            .Measurement("test")
            .SetTag("testTag", "testTagValue")
            .SetField("testField", "testFieldValue")
            .SetTimestamp(DateTimeOffset.UtcNow);

        await _client.WritePointAsync(point);
    }
/*
    public async Task WriteHomeDataAsync(HomeSensorDto data)
    {
        var point = PointData
            .Measurement("home")
            .SetTag("room", data.Room)
            .SetField("temp", data.Temp)
            .SetField("hum", data.Hum)
            .SetField("co", data.Co)
            .SetTimestamp(DateTimeOffset.FromUnixTimeSeconds(data.Timestamp));

        await _client.WritePointAsync(point);
        
        Console.WriteLine($"home,room={data.Room} temp={data.Temp},hum={data.Hum},co={data.Co} {data.Timestamp}");
    }
*/
    public async Task WriteCpuCoreUsageAsync(CpuCoreUsageDto data)
    {
        var point = PointData
            .Measurement("cpu")
            .SetTag("core_num", data.CoreNum.ToString())
            .SetField("percent_usage", data.PercentUsage)
            .SetTimestamp(DateTimeOffset.FromUnixTimeSeconds(data.Timestamp));

        await _client.WritePointAsync(point);
        
    }

    public async Task WriteRamUsageAsync(RamUsageDto data)
    {
        var point = PointData
            .Measurement("ram")
            .SetField("percent_usage", data.PercentUsage)
            .SetTimestamp(DateTimeOffset.FromUnixTimeSeconds(data.Timestamp));
        
        await _client.WritePointAsync(point);
    }

    public async Task WriteBatteryAsync(BatteryDto data)
    {
        var point = PointData
            .Measurement("battery")
            .SetTag("battery_index", data.BatteryIndex.ToString())
            .SetField("percent_charge", data.EstimatedChargeRemaining)
            .SetField("battery_status", data.BatteryStatus)
            .SetField("estimated_run_time", data.EstimatedRunTime)
            .SetField("time_to_full_charge", data.TimeToFullCharge)
            .SetTimestamp(DateTimeOffset.FromUnixTimeSeconds(data.Timestamp));

        await _client.WritePointAsync(point);
    }

    public async Task WriteDiskUsageAsync(DiskUsageDto data)
    {
        var point = PointData
        .Measurement("disk")
        .SetTag("disk_name", data.Name)
        .SetField("percent_usage",data.PercentUsage)
        .SetTimestamp(DateTimeOffset.FromUnixTimeSeconds(data.Timestamp));

        await _client.WritePointAsync(point);
        
    }

    public async Task<string> QuerySqlAsync(string sql)
    {
        Console.WriteLine($"SQL koji se izvrsava: {sql}");
    
        var results = new List<object>();
    
        await foreach (var row in _client.Query(sql))
        {
            results.Add(row);
        }
    
        var json = JsonSerializer.Serialize(results);
        Console.WriteLine($"Rezultat: {json}");
    
        return json;
    
    }
}