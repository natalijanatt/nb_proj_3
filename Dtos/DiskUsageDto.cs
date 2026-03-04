namespace WorkspaceMonitor.Dtos;

public record DiskUsageDto
(
    string Name,
    double PercentUsage,
    double GbUsage,
    long Timestamp
);