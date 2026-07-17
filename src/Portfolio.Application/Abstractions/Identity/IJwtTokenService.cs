using Portfolio.Application.DTOs.Auth;

namespace Portfolio.Application.Abstractions.Identity;

public interface IJwtTokenService
{
    string GenerateRefreshTokenValue();
    AuthTokenResult GenerateAccessToken(int userId, string userName, IEnumerable<string> roles, string jwtId);
    DateTime GetRefreshTokenExpiryUtc();
}

public sealed class AuthTokenResult
{
    public string AccessToken { get; init; } = string.Empty;
    public string JwtId { get; init; } = string.Empty;
    public DateTime ExpiresAtUtc { get; init; }
    public int ExpiresInSeconds { get; init; }
}
