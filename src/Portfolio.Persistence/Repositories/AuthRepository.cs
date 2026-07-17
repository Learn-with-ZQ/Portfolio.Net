using Portfolio.Application.Abstractions.Persistence;
using Portfolio.Domain.Entities.Identity;
using Portfolio.Domain.Enums;
using Portfolio.Persistence.Common;
using Portfolio.Persistence.Mapping;

namespace Portfolio.Persistence.Repositories;

public sealed class AuthRepository : IAuthRepository
{
    private readonly Database _db;
    public AuthRepository(Database db) => _db = db;

    public async Task<UserAccount?> GetUserByUserNameAsync(string userName, CancellationToken cancellationToken = default)
    {
        var (_, row) = await _db.QuerySingleAsync<dynamic>("dbo.usp_Auth_GetUserByUserName", new Dictionary<string, object?>
        { ["UserName"] = userName }, cancellationToken).ConfigureAwait(false);
        return row is null ? null : DapperColumnMapping.MapUserAccount(row);
    }

    public async Task<IReadOnlyList<string>> GetUserRolesAsync(int userId, CancellationToken cancellationToken = default)
    {
        var (_, rows) = await _db.QueryAsync<dynamic>("dbo.usp_Auth_GetUserRoles", new Dictionary<string, object?>
        { ["UserID"] = userId }, cancellationToken).ConfigureAwait(false);
        return rows
            .Select(r => (string)((IDictionary<string, object>)r)["RoleName"])
            .ToList();
    }

    public async Task<int> SaveRefreshTokenAsync(
        int userId,
        string tokenHash,
        string jwtId,
        DateTime expiresAtUtc,
        string? createdByIp,
        CancellationToken cancellationToken = default)
    {
        var result = await _db.ExecuteAsync("dbo.usp_RefreshToken_Insert", new Dictionary<string, object?>
        {
            ["UserID_Fk"] = userId,
            ["TokenHash"] = tokenHash,
            ["JwtId"] = jwtId,
            ["ExpiresAt"] = expiresAtUtc,
            ["CreatedByIp"] = createdByIp
        }, outIdKey: "OutRefreshTokenID", cancellationToken: cancellationToken).ConfigureAwait(false);

        if (result.StatusCode != SpStatusCode.Success)
            throw new InvalidOperationException(result.StatusMessage);

        return result.OutId ?? 0;
    }

    public async Task<RefreshTokenRecord?> GetRefreshTokenByHashAsync(string tokenHash, CancellationToken cancellationToken = default)
    {
        var (_, row) = await _db.QuerySingleAsync<dynamic>("dbo.usp_RefreshToken_GetByHash", new Dictionary<string, object?>
        { ["TokenHash"] = tokenHash }, cancellationToken).ConfigureAwait(false);
        return row is null ? null : DapperColumnMapping.MapRefreshToken(row);
    }

    public async Task RevokeRefreshTokenAsync(string tokenHash, string? replacedByTokenHash, CancellationToken cancellationToken = default)
    {
        await _db.ExecuteAsync("dbo.usp_RefreshToken_Revoke", new Dictionary<string, object?>
        {
            ["TokenHash"] = tokenHash,
            ["ReplacedByTokenHash"] = replacedByTokenHash
        }, cancellationToken: cancellationToken).ConfigureAwait(false);
    }

    public async Task RevokeAllUserRefreshTokensAsync(int userId, CancellationToken cancellationToken = default)
    {
        await _db.ExecuteAsync("dbo.usp_RefreshToken_RevokeAllForUser", new Dictionary<string, object?>
        { ["UserID"] = userId }, cancellationToken: cancellationToken).ConfigureAwait(false);
    }
}
