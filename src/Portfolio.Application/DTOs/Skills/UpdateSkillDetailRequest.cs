namespace Portfolio.Application.DTOs.Skills;

public sealed class UpdateSkillDetailRequest
{
    public int SkillDetailId { get; set; }
    public string SkillDetailName { get; set; } = string.Empty;
    public byte? ProficiencyLevel { get; set; }
    public int SortOrder { get; set; }
    public byte[] RowVersion { get; set; } = [];
}
