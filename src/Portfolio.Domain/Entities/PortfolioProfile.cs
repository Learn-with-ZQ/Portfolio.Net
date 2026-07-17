using Portfolio.Domain.Common;

namespace Portfolio.Domain.Entities;

public sealed class PortfolioProfile : AuditableEntity
{
    public int PortfolioProfileId { get; set; }
    public int UserId { get; set; }
    public string ProfileSlug { get; set; } = string.Empty;
    public string? ProfileTitle { get; set; }
    public bool IsPublic { get; set; } = true;
    public bool IsPrimary { get; set; } = true;
}
