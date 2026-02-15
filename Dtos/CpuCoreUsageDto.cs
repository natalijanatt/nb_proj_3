namespace WorkspaceMonitor.Dtos;

public record CpuCoreUsageDto(
    int CoreNum,
    ulong PercentUsage,
    long Timestamp
    );