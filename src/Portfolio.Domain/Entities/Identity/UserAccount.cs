namespace Portfolio.Domain.Entities.Identity;

public sealed class UserAccount
{
    public int UserId { get; set; }
    public string UserName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? DisplayName { get; set; }
    public string PasswordHash { get; set; } = string.Empty;
    public bool IsActive { get; set; }
    public bool IsDeleted { get; set; }
    public IReadOnlyList<string> Roles { get; set; } = [];
}
