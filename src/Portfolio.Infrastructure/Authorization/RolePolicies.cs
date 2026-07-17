using Portfolio.Domain.Constants;

namespace Portfolio.Infrastructure.Authorization;

public static class RolePolicies
{
    public const string AdminOnly = nameof(AdminOnly);
    public const string ReadAccess = nameof(ReadAccess);

    public static void Configure(Microsoft.AspNetCore.Authorization.AuthorizationOptions options)
    {
        options.AddPolicy(AdminOnly, policy => policy.RequireRole(RoleNames.Admin));
        options.AddPolicy(ReadAccess, policy => policy.RequireRole(RoleNames.Admin, RoleNames.Public));
    }
}
