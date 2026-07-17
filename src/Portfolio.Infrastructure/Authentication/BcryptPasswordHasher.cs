using Portfolio.Application.Abstractions.Identity;

namespace Portfolio.Infrastructure.Authentication;

public sealed class BcryptPasswordHasher : IPasswordHasher
{
    public string Hash(string password) => BCrypt.Net.BCrypt.HashPassword(password);

    public bool Verify(string password, string passwordHash) =>
        !string.IsNullOrWhiteSpace(passwordHash) && BCrypt.Net.BCrypt.Verify(password, passwordHash);
}
