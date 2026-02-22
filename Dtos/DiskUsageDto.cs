namespace WorkspaceMonitor.Dtos;

public record DiskUsageDto
(
    String Name,
    double PercentUsage,
    long Timestamp
);