using Portfolio.Domain.Common;

namespace Portfolio.Domain.Entities.Lookups;

public sealed class Degree : AuditableEntity
{
    public int DegreeId { get; set; }
    public string DegreeName { get; set; } = string.Empty;
}
