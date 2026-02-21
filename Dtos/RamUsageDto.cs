namespace WorkspaceMonitor.Dtos;

public record RamUsageDto(
    ulong PercentUsage,
    long Timestamp
    );