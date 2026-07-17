namespace Portfolio.Application.DTOs.Auth;

public sealed class AuthTokensDto
{
    public string AccessToken { get; init; } = string.Empty;
    public string RefreshToken { get; init; } = string.Empty;
    public int ExpiresInSeconds { get; init; }
    public string TokenType { get; init; } = "Bearer";
    public IReadOnlyList<string> Roles { get; init; } = [];
}
