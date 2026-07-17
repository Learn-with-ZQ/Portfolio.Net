namespace Portfolio.Application.DTOs.Skills;

public sealed class SkillSearchResultDto
{
    public int SkillId { get; init; }
    public string SkillName { get; init; } = string.Empty;
    public int SortOrder { get; init; }
    public int? SkillDetailId { get; init; }
    public string? SkillDetailName { get; init; }
    public byte? ProficiencyLevel { get; init; }
}
