using Portfolio.Domain.Entities.Identity;

namespace Portfolio.Application.Abstractions.Persistence;

public interface IAuthRepository
{
    Task<UserAccount?> GetUserByUserNameAsync(string userName, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<string>> GetUserRolesAsync(int userId, CancellationToken cancellationToken = default);
    Task<int> SaveRefreshTokenAsync(int userId, string tokenHash, string jwtId, DateTime expiresAtUtc, string? createdByIp, CancellationToken cancellationToken = default);
    Task<RefreshTokenRecord?> GetRefreshTokenByHashAsync(string tokenHash, CancellationToken cancellationToken = default);
    Task RevokeRefreshTokenAsync(string tokenHash, string? replacedByTokenHash, CancellationToken cancellationToken = default);
    Task RevokeAllUserRefreshTokensAsync(int userId, CancellationToken cancellationToken = default);
}
