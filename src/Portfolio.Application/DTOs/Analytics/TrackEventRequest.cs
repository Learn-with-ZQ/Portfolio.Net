using Portfolio.Application.Common.Models;

namespace Portfolio.Application.DTOs.Analytics;

public sealed class TrackEventRequest : ModuleRequestBase
{
    public string EventType { get; set; } = string.Empty;
    public int? EntityId { get; set; }
    public string? Path { get; set; }
    public string? VisitorId { get; set; }
    public string? Country { get; set; }
    public string? City { get; set; }
    public string? Browser { get; set; }
    public string? Device { get; set; }
    public string? Referrer { get; set; }
}
