using Portfolio.Domain.Common;

namespace Portfolio.Domain.Entities.Skills;

public sealed class Skill : AuditableEntity
{
    public int SkillId { get; set; }
    public int PortfolioProfileId { get; set; }
    public string SkillName { get; set; } = string.Empty;
    public int SortOrder { get; set; }

    public ICollection<SkillDetail> Details { get; set; } = [];
}
