using Portfolio.Application.Common.Models;

namespace Portfolio.Application.DTOs.Skills;

public sealed class SearchSkillRequest : SearchRequestBase
{
    public byte? MinProficiencyLevel { get; set; }
}
