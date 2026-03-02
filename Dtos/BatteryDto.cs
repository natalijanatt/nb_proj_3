namespace WorkspaceMonitor.Dtos;

public record BatteryDto(
    int BatteryIndex,
    ushort EstimatedChargeRemaining,
    ushort BatteryStatus,
    uint EstimatedRunTime,
    uint TimeToFullCharge,
    long Timestamp
);
