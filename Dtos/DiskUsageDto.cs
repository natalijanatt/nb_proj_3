namespace WorkspaceMonitor.Dtos;

public record DiskUsageDto
(
    string Name,
    double PercentUsage,
    long Timestamp
);