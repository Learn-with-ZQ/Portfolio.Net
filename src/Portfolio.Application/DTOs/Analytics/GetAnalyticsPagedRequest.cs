using Portfolio.Application.Common.Models;

namespace Portfolio.Application.DTOs.Analytics;

public sealed class GetAnalyticsPagedRequest : PagedRequestBase
{
    public string? EventType { get; set; }
}
