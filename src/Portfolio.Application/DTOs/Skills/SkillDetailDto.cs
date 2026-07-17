using Portfolio.Application.Common.Models;

namespace Portfolio.Application.DTOs.Skills;

public sealed class SkillDetailDto
{
    public int SkillId { get; init; }
    public int PortfolioProfileId { get; init; }
    public string SkillName { get; init; } = string.Empty;
    public int SortOrder { get; init; }
    public byte[] RowVersion { get; init; } = [];
    public AuditInfoDto Audit { get; init; } = new();
    public IReadOnlyList<SkillDetailItemDto> Details { get; init; } = [];
}
