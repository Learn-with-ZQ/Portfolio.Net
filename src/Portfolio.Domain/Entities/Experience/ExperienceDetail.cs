using Portfolio.Domain.Common;

namespace Portfolio.Domain.Entities.Experience;

public sealed class ExperienceDetail : AuditableEntity
{
    public int ExperienceDetailId { get; set; }
    public int ExperienceId { get; set; }
    public string ExperienceDetailName { get; set; } = string.Empty;
    public int SortOrder { get; set; }
}
