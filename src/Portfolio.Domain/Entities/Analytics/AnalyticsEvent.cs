namespace Portfolio.Domain.Entities.Analytics;

/// <summary>Immutable analytics event (no soft-delete/audit — append-only log).</summary>
public sealed class AnalyticsEvent
{
    public long AnalyticsEventId { get; set; }
    public int PortfolioProfileId { get; set; }
    /// <summary>PageView, Visit, ResumeDownload, CertificateDownload, ProjectView, BlogView, ContactRequest.</summary>
    public string EventType { get; set; } = string.Empty;
    public int? EntityId { get; set; }
    public string? Path { get; set; }
    public string? VisitorId { get; set; }
    public string? Country { get; set; }
    public string? City { get; set; }
    public string? Browser { get; set; }
    public string? Device { get; set; }
    public string? Referrer { get; set; }
    public string? IpHash { get; set; }
    public DateTime CreatedAt { get; set; }
}
