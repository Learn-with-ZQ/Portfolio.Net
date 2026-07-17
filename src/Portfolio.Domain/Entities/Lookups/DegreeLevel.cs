using Portfolio.Domain.Common;

namespace Portfolio.Domain.Entities.Lookups;

public sealed class DegreeLevel : AuditableEntity
{
    public int DegreeLevelId { get; set; }
    public string DegreeLevelName { get; set; } = string.Empty;
    public string DegreePrefix { get; set; } = string.Empty;
    public int SortOrder { get; set; }
}
