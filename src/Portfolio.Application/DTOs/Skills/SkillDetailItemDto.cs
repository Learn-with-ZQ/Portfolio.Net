namespace Portfolio.Application.DTOs.Skills;

public sealed class SkillDetailItemDto
{
    public int SkillDetailId { get; init; }
    public int SkillId { get; init; }
    public string SkillDetailName { get; init; } = string.Empty;
    public byte? ProficiencyLevel { get; init; }
    public int SortOrder { get; init; }
    public byte[] RowVersion { get; init; } = [];
}
