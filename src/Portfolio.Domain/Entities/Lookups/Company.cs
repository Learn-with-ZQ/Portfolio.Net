using Portfolio.Domain.Common;

namespace Portfolio.Domain.Entities.Lookups;

public sealed class Company : AuditableEntity
{
    public int CompanyId { get; set; }
    public string CompanyName { get; set; } = string.Empty;
    public string? WebsiteUrl { get; set; }
}
