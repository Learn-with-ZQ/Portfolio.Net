using Portfolio.Domain.Common;

namespace Portfolio.Domain.Entities.Lookups;

public sealed class Technology : AuditableEntity
{
    public int TechnologyId { get; set; }
    public string TechnologyName { get; set; } = string.Empty;
    public string? Category { get; set; }
}
