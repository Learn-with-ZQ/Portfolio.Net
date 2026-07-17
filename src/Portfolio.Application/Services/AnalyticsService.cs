using System.Security.Cryptography;
using System.Text;
using FluentValidation;
using Portfolio.Application.Abstractions.Persistence;
using Portfolio.Application.Common.BusinessRules;
using Portfolio.Application.Common.Helpers;
using Portfolio.Application.Common.Models;
using Portfolio.Application.Common.Results;
using Portfolio.Application.DTOs.Analytics;
using Portfolio.Application.Services.Interfaces;
using Portfolio.Domain.Entities.Analytics;

namespace Portfolio.Application.Services;

public sealed class AnalyticsService : ApplicationServiceBase, IAnalyticsService
{
    private readonly IAnalyticsRepository _repository;
    private readonly IValidator<TrackEventRequest> _trackValidator;

    public AnalyticsService(IAnalyticsRepository repository, IValidator<TrackEventRequest> trackValidator)
    {
        _repository = repository;
        _trackValidator = trackValidator;
    }

    public async Task<ServiceResult> TrackAsync(TrackEventRequest request, string? clientIp, CancellationToken cancellationToken = default)
    {
        try
        {
            await ValidateAsync(_trackValidator, request, cancellationToken).ConfigureAwait(false);
            PortfolioProfileRules.EnsureValidProfileId(request.PortfolioProfileId);

            var entity = new AnalyticsEvent
            {
                PortfolioProfileId = request.PortfolioProfileId,
                EventType = request.EventType,
                EntityId = request.EntityId,
                Path = request.Path,
                VisitorId = request.VisitorId,
                Country = request.Country,
                City = request.City,
                Browser = request.Browser,
                Device = request.Device,
                Referrer = request.Referrer,
                IpHash = HashIp(clientIp)
            };

            SpResultHelper.EnsureSuccess(await _repository.TrackAsync(entity, cancellationToken).ConfigureAwait(false));
            return ServiceResult.Success();
        }
        catch (Exception ex) { return HandleException(ex); }
    }

    public async Task<ServiceResult<AnalyticsSummaryDto>> GetSummaryAsync(int portfolioProfileId, CancellationToken cancellationToken = default)
    {
        try
        {
            PortfolioProfileRules.EnsureValidProfileId(portfolioProfileId);
            return ServiceResult<AnalyticsSummaryDto>.Success(
                await _repository.GetSummaryAsync(portfolioProfileId, cancellationToken).ConfigureAwait(false));
        }
        catch (Exception ex) { return HandleException<AnalyticsSummaryDto>(ex); }
    }

    public async Task<ServiceResult<PagedResult<AnalyticsEventDto>>> GetPagedAsync(GetAnalyticsPagedRequest request, CancellationToken cancellationToken = default)
    {
        try
        {
            PortfolioProfileRules.EnsureValidProfileId(request.PortfolioProfileId);
            return ServiceResult<PagedResult<AnalyticsEventDto>>.Success(
                await _repository.GetPagedAsync(request, cancellationToken).ConfigureAwait(false));
        }
        catch (Exception ex) { return HandleException<PagedResult<AnalyticsEventDto>>(ex); }
    }

    /// <summary>Store only a one-way hash of the IP for privacy.</summary>
    private static string? HashIp(string? ip)
    {
        if (string.IsNullOrWhiteSpace(ip)) return null;
        var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(ip));
        return Convert.ToHexString(bytes);
    }
}
