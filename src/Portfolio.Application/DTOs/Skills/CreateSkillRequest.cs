using Portfolio.Application.Common.Models;

namespace Portfolio.Application.DTOs.Skills;

public sealed class CreateSkillRequest : ModuleRequestBase
{
    public string SkillName { get; set; } = string.Empty;
    public int SortOrder { get; set; }
}
