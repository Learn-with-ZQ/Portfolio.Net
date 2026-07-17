using Portfolio.Application.Common.Results;
using Portfolio.Application.DTOs.Auth;

namespace Portfolio.Application.Services.Interfaces;

public interface IAuthService
{
    Task<ServiceResult<AuthTokensDto>> LoginAsync(LoginRequest request, string? ipAddress, CancellationToken cancellationToken = default);
    Task<ServiceResult<AuthTokensDto>> RefreshAsync(RefreshTokenRequest request, string? ipAddress, CancellationToken cancellationToken = default);
    Task<ServiceResult> LogoutAsync(LogoutRequest request, CancellationToken cancellationToken = default);
    Task<ServiceResult> LogoutAllAsync(CancellationToken cancellationToken = default);
}
