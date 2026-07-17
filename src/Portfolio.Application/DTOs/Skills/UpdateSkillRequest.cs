namespace Portfolio.Application.DTOs.Skills;

public sealed class UpdateSkillRequest
{
    public int SkillId { get; set; }
    public string SkillName { get; set; } = string.Empty;
    public int SortOrder { get; set; }
    public byte[] RowVersion { get; set; } = [];
}
