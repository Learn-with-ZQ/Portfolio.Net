using Portfolio.Application.Common.Models;
using Portfolio.Application.DTOs.Analytics;
using Portfolio.Domain.Entities.Analytics;

namespace Portfolio.Application.Abstractions.Persistence;

public interface IAnalyticsRepository
{
    Task<SpExecutionResult> TrackAsync(AnalyticsEvent analyticsEvent, CancellationToken cancellationToken = default);
    Task<AnalyticsSummaryDto> GetSummaryAsync(int portfolioProfileId, CancellationToken cancellationToken = default);
    Task<PagedResult<AnalyticsEventDto>> GetPagedAsync(GetAnalyticsPagedRequest request, CancellationToken cancellationToken = default);
}
