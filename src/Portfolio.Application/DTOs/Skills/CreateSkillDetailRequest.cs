namespace Portfolio.Application.DTOs.Skills;

public sealed class CreateSkillDetailRequest
{
    public int SkillId { get; set; }
    public string SkillDetailName { get; set; } = string.Empty;
    public byte? ProficiencyLevel { get; set; }
    public int SortOrder { get; set; }
}
