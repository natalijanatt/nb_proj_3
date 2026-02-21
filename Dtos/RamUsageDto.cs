namespace WorkspaceMonitor.Dtos;

public record RamUsageDto(
    double PercentUsage,
    long Timestamp
    );