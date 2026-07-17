using FluentValidation;
using Portfolio.Application.Abstractions.Identity;
using Portfolio.Application.Abstractions.Persistence;
using Portfolio.Application.Common.Interfaces;
using Portfolio.Application.Common.Results;
using Portfolio.Application.DTOs.Auth;
using Portfolio.Application.Services.Interfaces;
using Portfolio.Domain.Enums;

namespace Portfolio.Application.Services;

public sealed class AuthService : ApplicationServiceBase, IAuthService
{
    private readonly IAuthRepository _authRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly ITokenHasher _tokenHasher;
    private readonly IJwtTokenService _jwtTokenService;
    private readonly ICurrentUserService _currentUser;
    private readonly IValidator<LoginRequest> _loginValidator;
    private readonly IValidator<RefreshTokenRequest> _refreshValidator;
    private readonly IValidator<LogoutRequest> _logoutValidator;

    public AuthService(
        IAuthRepository authRepository,
        IPasswordHasher passwordHasher,
        ITokenHasher tokenHasher,
        IJwtTokenService jwtTokenService,
        ICurrentUserService currentUser,
        IValidator<LoginRequest> loginValidator,
        IValidator<RefreshTokenRequest> refreshValidator,
        IValidator<LogoutRequest> logoutValidator)
    {
        _authRepository = authRepository;
        _passwordHasher = passwordHasher;
        _tokenHasher = tokenHasher;
        _jwtTokenService = jwtTokenService;
        _currentUser = currentUser;
        _loginValidator = loginValidator;
        _refreshValidator = refreshValidator;
        _logoutValidator = logoutValidator;
    }

    public async Task<ServiceResult<AuthTokensDto>> LoginAsync(LoginRequest request, string? ipAddress, CancellationToken cancellationToken = default)
    {
        try
        {
            await ValidateAsync(_loginValidator, request, cancellationToken).ConfigureAwait(false);

            var user = await _authRepository.GetUserByUserNameAsync(request.UserName, cancellationToken).ConfigureAwait(false);
            if (user is null || !_passwordHasher.Verify(request.Password, user.PasswordHash))
                return ServiceResult<AuthTokensDto>.Failure("Invalid username or password.", SpStatusCode.Unauthorized);

            if (!user.IsActive || user.IsDeleted)
                return ServiceResult<AuthTokensDto>.Failure("Account is disabled.", SpStatusCode.Forbidden);

            var roles = await _authRepository.GetUserRolesAsync(user.UserId, cancellationToken).ConfigureAwait(false);
            if (roles.Count == 0)
                return ServiceResult<AuthTokensDto>.Failure("User has no assigned roles.", SpStatusCode.Forbidden);

            var tokens = await IssueTokenPairAsync(user.UserId, user.UserName, roles, ipAddress, cancellationToken).ConfigureAwait(false);
            return ServiceResult<AuthTokensDto>.Success(tokens);
        }
        catch (Exception ex)
        {
            return HandleException<AuthTokensDto>(ex);
        }
    }

    public async Task<ServiceResult<AuthTokensDto>> RefreshAsync(RefreshTokenRequest request, string? ipAddress, CancellationToken cancellationToken = default)
    {
        try
        {
            await ValidateAsync(_refreshValidator, request, cancellationToken).ConfigureAwait(false);

            var tokenHash = _tokenHasher.Hash(request.RefreshToken);
            var storedToken = await _authRepository.GetRefreshTokenByHashAsync(tokenHash, cancellationToken).ConfigureAwait(false);

            if (storedToken is null || !storedToken.IsValid)
                return ServiceResult<AuthTokensDto>.Failure("Invalid or expired refresh token.", SpStatusCode.Unauthorized);

            var roles = await _authRepository.GetUserRolesAsync(storedToken.UserId, cancellationToken).ConfigureAwait(false);
            if (roles.Count == 0)
                return ServiceResult<AuthTokensDto>.Failure("User has no assigned roles.", SpStatusCode.Forbidden);

            var newRefreshTokenValue = _jwtTokenService.GenerateRefreshTokenValue();
            var newTokenHash = _tokenHasher.Hash(newRefreshTokenValue);

            await _authRepository.RevokeRefreshTokenAsync(tokenHash, newTokenHash, cancellationToken).ConfigureAwait(false);

            var access = _jwtTokenService.GenerateAccessToken(storedToken.UserId, storedToken.UserName, roles, Guid.NewGuid().ToString("N"));
            var expiresAt = _jwtTokenService.GetRefreshTokenExpiryUtc();

            await _authRepository.SaveRefreshTokenAsync(
                storedToken.UserId,
                newTokenHash,
                access.JwtId,
                expiresAt,
                ipAddress,
                cancellationToken).ConfigureAwait(false);

            return ServiceResult<AuthTokensDto>.Success(new AuthTokensDto
            {
                AccessToken = access.AccessToken,
                RefreshToken = newRefreshTokenValue,
                ExpiresInSeconds = access.ExpiresInSeconds,
                Roles = roles
            });
        }
        catch (Exception ex)
        {
            return HandleException<AuthTokensDto>(ex);
        }
    }

    public async Task<ServiceResult> LogoutAsync(LogoutRequest request, CancellationToken cancellationToken = default)
    {
        try
        {
            await ValidateAsync(_logoutValidator, request, cancellationToken).ConfigureAwait(false);

            var tokenHash = _tokenHasher.Hash(request.RefreshToken);
            await _authRepository.RevokeRefreshTokenAsync(tokenHash, null, cancellationToken).ConfigureAwait(false);

            return ServiceResult.Success();
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    public async Task<ServiceResult> LogoutAllAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            if (_currentUser.UserId is null)
                return ServiceResult.Failure("User is not authenticated.", SpStatusCode.Unauthorized);

            await _authRepository.RevokeAllUserRefreshTokensAsync(_currentUser.UserId.Value, cancellationToken).ConfigureAwait(false);
            return ServiceResult.Success();
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    private async Task<AuthTokensDto> IssueTokenPairAsync(
        int userId,
        string userName,
        IReadOnlyList<string> roles,
        string? ipAddress,
        CancellationToken cancellationToken)
    {
        var jwtId = Guid.NewGuid().ToString("N");
        var access = _jwtTokenService.GenerateAccessToken(userId, userName, roles, jwtId);
        var refreshTokenValue = _jwtTokenService.GenerateRefreshTokenValue();
        var refreshTokenHash = _tokenHasher.Hash(refreshTokenValue);
        var refreshExpiresAt = _jwtTokenService.GetRefreshTokenExpiryUtc();

        await _authRepository.SaveRefreshTokenAsync(
            userId,
            refreshTokenHash,
            jwtId,
            refreshExpiresAt,
            ipAddress,
            cancellationToken).ConfigureAwait(false);

        return new AuthTokensDto
        {
            AccessToken = access.AccessToken,
            RefreshToken = refreshTokenValue,
            ExpiresInSeconds = access.ExpiresInSeconds,
            Roles = roles
        };
    }
}
