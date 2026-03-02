namespace WorkspaceMonitor.Dtos;

public record NetworkUsageDto(
    string AdapterName,
    ulong BytesReceivedPerSec,
    ulong BytesSentPerSec,
    long Timestamp
);
