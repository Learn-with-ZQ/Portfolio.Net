using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Portfolio.Application.DTOs.Analytics;
using Portfolio.Application.Services.Interfaces;
using Portfolio.Infrastructure.Authorization;

namespace Portfolio.Api.Controllers;

[Route("api/analytics")]
public sealed class AnalyticsController : ApiControllerBase
{
    private readonly IAnalyticsService _service;

    public AnalyticsController(IAnalyticsService service) => _service = service;

    /// <summary>Records a visitor/interaction event. Public, fire-and-forget.</summary>
    [HttpPost("track")]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Track([FromBody] TrackEventRequest request, CancellationToken cancellationToken)
        => FromResult(await _service.TrackAsync(request, GetClientIp(), cancellationToken).ConfigureAwait(false));

    /// <summary>Aggregate dashboard summary — admin only.</summary>
    [HttpGet("summary")]
    [Authorize(Policy = RolePolicies.AdminOnly)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetSummary([FromQuery] int portfolioProfileId, CancellationToken cancellationToken)
        => FromResult(await _service.GetSummaryAsync(portfolioProfileId, cancellationToken).ConfigureAwait(false));

    /// <summary>Raw event log (visitor management) — admin only.</summary>
    [HttpGet("paged")]
    [Authorize(Policy = RolePolicies.AdminOnly)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetPaged([FromQuery] GetAnalyticsPagedRequest request, CancellationToken cancellationToken)
        => FromResult(await _service.GetPagedAsync(request, cancellationToken).ConfigureAwait(false));

    private string? GetClientIp() => HttpContext.Connection.RemoteIpAddress?.ToString();
}
