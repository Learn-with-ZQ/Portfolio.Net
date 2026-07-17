using Portfolio.Application.Abstractions.Persistence;
using Portfolio.Application.Common.Models;
using Portfolio.Application.DTOs.Analytics;
using Portfolio.Domain.Entities.Analytics;
using Portfolio.Persistence.Common;

namespace Portfolio.Persistence.Repositories;

public sealed class AnalyticsRepository : IAnalyticsRepository
{
    private readonly Database _db;
    public AnalyticsRepository(Database db) => _db = db;

    public Task<SpExecutionResult> TrackAsync(AnalyticsEvent e, CancellationToken cancellationToken = default) =>
        _db.ExecuteAsync("dbo.usp_Analytics_Track", new Dictionary<string, object?>
        {
            ["PortfolioProfileID_Fk"] = e.PortfolioProfileId,
            ["EventType"] = e.EventType,
            ["EntityId"] = e.EntityId,
            ["Path"] = e.Path,
            ["VisitorId"] = e.VisitorId,
            ["Country"] = e.Country,
            ["City"] = e.City,
            ["Browser"] = e.Browser,
            ["Device"] = e.Device,
            ["Referrer"] = e.Referrer,
            ["IpHash"] = e.IpHash
        }, cancellationToken: cancellationToken);

    public async Task<AnalyticsSummaryDto> GetSummaryAsync(int portfolioProfileId, CancellationToken cancellationToken = default)
    {
        var (_, summary) = await _db.QueryMultipleAsync("dbo.usp_Analytics_GetSummary",
            new Dictionary<string, object?> { ["PortfolioProfileID_Fk"] = portfolioProfileId },
            async grid =>
            {
                var result = await grid.ReadFirstOrDefaultAsync<AnalyticsSummaryDto>().ConfigureAwait(false)
                             ?? new AnalyticsSummaryDto();
                result.ByCountry = (await grid.ReadAsync<CountItem>().ConfigureAwait(false)).ToList();
                result.ByBrowser = (await grid.ReadAsync<CountItem>().ConfigureAwait(false)).ToList();
                result.ByDevice = (await grid.ReadAsync<CountItem>().ConfigureAwait(false)).ToList();
                return result;
            }, cancellationToken).ConfigureAwait(false);
        return summary;
    }

    public async Task<PagedResult<AnalyticsEventDto>> GetPagedAsync(GetAnalyticsPagedRequest request, CancellationToken cancellationToken = default)
    {
        var (_, rows, total, pages) = await _db.QueryPagedAsync<AnalyticsEventDto>(
            "dbo.usp_Analytics_GetPaged", new Dictionary<string, object?>
            {
                ["PortfolioProfileID_Fk"] = request.PortfolioProfileId,
                ["PageNumber"] = request.PageNumber,
                ["PageSize"] = request.PageSize,
                ["SearchTerm"] = request.SearchTerm,
                ["EventType"] = request.EventType
            }, cancellationToken).ConfigureAwait(false);

        return new PagedResult<AnalyticsEventDto>
        {
            Items = rows,
            PageNumber = request.PageNumber,
            PageSize = request.PageSize,
            TotalRecords = total,
            TotalPages = pages
        };
    }
}
