namespace Portfolio.Application.DTOs.Analytics;

public sealed class CountItem
{
    public string Label { get; set; } = string.Empty;
    public int Count { get; set; }
}

public sealed class AnalyticsSummaryDto
{
    public int TotalEvents { get; set; }
    public int UniqueVisitors { get; set; }
    public int PageViews { get; set; }
    public int ResumeDownloads { get; set; }
    public int CertificateDownloads { get; set; }
    public int ProjectViews { get; set; }
    public int BlogViews { get; set; }
    public int ContactRequests { get; set; }

    public IReadOnlyList<CountItem> ByCountry { get; set; } = [];
    public IReadOnlyList<CountItem> ByBrowser { get; set; } = [];
    public IReadOnlyList<CountItem> ByDevice { get; set; } = [];
}
