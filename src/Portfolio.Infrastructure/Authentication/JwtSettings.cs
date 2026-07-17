namespace Portfolio.Infrastructure.Authentication;

public sealed class JwtSettings
{
    public const string SectionName = "Jwt";

    public string Issuer { get; set; } = string.Empty;
    public string Audience { get; set; } = string.Empty;
    public string SecretKey { get; set; } = string.Empty;
    public int ExpirationMinutes { get; set; } = 60;
    public int AccessTokenExpirationMinutes { get; set; }
    public int RefreshTokenExpirationDays { get; set; } = 7;

    public int EffectiveAccessTokenExpirationMinutes =>
        AccessTokenExpirationMinutes > 0 ? AccessTokenExpirationMinutes : ExpirationMinutes;
}
