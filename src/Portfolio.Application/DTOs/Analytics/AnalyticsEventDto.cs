namespace Portfolio.Application.DTOs.Analytics;

public sealed class AnalyticsEventDto
{
    public long AnalyticsEventId { get; init; }
    public string EventType { get; init; } = string.Empty;
    public int? EntityId { get; init; }
    public string? Path { get; init; }
    public string? VisitorId { get; init; }
    public string? Country { get; init; }
    public string? City { get; init; }
    public string? Browser { get; init; }
    public string? Device { get; init; }
    public DateTime CreatedAt { get; init; }
}
