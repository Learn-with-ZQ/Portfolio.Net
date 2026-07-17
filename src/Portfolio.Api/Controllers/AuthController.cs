using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Portfolio.Api.Models;
using Portfolio.Application.DTOs.Auth;
using Portfolio.Application.Services.Interfaces;
using Portfolio.Domain.Enums;

namespace Portfolio.Api.Controllers;

[ApiController]
[Route("api/auth")]
public sealed class AuthController : ApiControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService) => _authService = authService;

    /// <summary>
    /// Authenticates a user and returns JWT access and refresh tokens.
    /// </summary>
    [HttpPost("login")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(ApiEnvelope<AuthTokensDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiEnvelope<object>), StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Login([FromBody] LoginRequest request, CancellationToken cancellationToken)
    {
        var result = await _authService.LoginAsync(request, GetClientIp(), cancellationToken).ConfigureAwait(false);
        return FromAuthResult(result);
    }

    /// <summary>
    /// Rotates the refresh token and issues a new access token.
    /// </summary>
    [HttpPost("refresh")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(ApiEnvelope<AuthTokensDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiEnvelope<object>), StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Refresh([FromBody] RefreshTokenRequest request, CancellationToken cancellationToken)
    {
        var result = await _authService.RefreshAsync(request, GetClientIp(), cancellationToken).ConfigureAwait(false);
        return FromAuthResult(result);
    }

    /// <summary>
    /// Revokes the supplied refresh token (logout from current session).
    /// </summary>
    [HttpPost("logout")]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> Logout([FromBody] LogoutRequest request, CancellationToken cancellationToken)
        => FromResult(await _authService.LogoutAsync(request, cancellationToken).ConfigureAwait(false));

    /// <summary>
    /// Revokes all active refresh tokens for the authenticated user.
    /// </summary>
    [HttpPost("logout-all")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> LogoutAll(CancellationToken cancellationToken)
        => FromResult(await _authService.LogoutAllAsync(cancellationToken).ConfigureAwait(false));

    private IActionResult FromAuthResult(Application.Common.Results.ServiceResult<AuthTokensDto> result)
    {
        if (result.IsSuccess)
            return Ok(ApiEnvelope<AuthTokensDto>.Ok(result.Data!));

        var envelope = ApiEnvelope<object>.Fail(result.ErrorMessage ?? "Authentication failed.", result.ValidationErrors);

        return result.StatusCode switch
        {
            SpStatusCode.Unauthorized => Unauthorized(envelope),
            SpStatusCode.Forbidden => Forbid(),
            SpStatusCode.ValidationError => BadRequest(envelope),
            _ => BadRequest(envelope)
        };
    }

    private string? GetClientIp() =>
        HttpContext.Connection.RemoteIpAddress?.ToString();
}
