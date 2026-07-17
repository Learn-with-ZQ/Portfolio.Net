using Portfolio.Domain.Common;

namespace Portfolio.Domain.Entities.Lookups;

public sealed class Institute : AuditableEntity
{
    public int InstituteId { get; set; }
    public string InstituteName { get; set; } = string.Empty;
    public string? Location { get; set; }
}
