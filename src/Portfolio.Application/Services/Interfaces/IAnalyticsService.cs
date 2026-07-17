using Portfolio.Application.Common.Models;
using Portfolio.Application.Common.Results;
using Portfolio.Application.DTOs.Analytics;

namespace Portfolio.Application.Services.Interfaces;

public interface IAnalyticsService
{
    Task<ServiceResult> TrackAsync(TrackEventRequest request, string? clientIp, CancellationToken cancellationToken = default);
    Task<ServiceResult<AnalyticsSummaryDto>> GetSummaryAsync(int portfolioProfileId, CancellationToken cancellationToken = default);
    Task<ServiceResult<PagedResult<AnalyticsEventDto>>> GetPagedAsync(GetAnalyticsPagedRequest request, CancellationToken cancellationToken = default);
}
