namespace Portfolio.Application.DTOs.Skills;

public sealed class SkillDto
{
    public int SkillId { get; init; }
    public string SkillName { get; init; } = string.Empty;
    public int SortOrder { get; init; }
    public int DetailCount { get; init; }
}
