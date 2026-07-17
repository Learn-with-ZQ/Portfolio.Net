using Portfolio.Domain.Common;

namespace Portfolio.Domain.Entities.Skills;

public sealed class SkillDetail : AuditableEntity
{
    public int SkillDetailId { get; set; }
    public int SkillId { get; set; }
    public string SkillDetailName { get; set; } = string.Empty;
    public byte? ProficiencyLevel { get; set; }
    public int SortOrder { get; set; }
}
