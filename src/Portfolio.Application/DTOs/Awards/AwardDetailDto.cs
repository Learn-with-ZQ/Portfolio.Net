using Portfolio.Application.Common.Models;

namespace Portfolio.Application.DTOs.Awards;

public sealed class AwardDetailDto
{
    public int AwardId { get; init; }
    public int PortfolioProfileId { get; init; }
    public string AwardName { get; init; } = string.Empty;
    public DateOnly StartDate { get; init; }
    public DateOnly? EndDate { get; init; }
    public int SortOrder { get; init; }
    public byte[] RowVersion { get; init; } = [];
    public AuditInfoDto Audit { get; init; } = new();
    public IReadOnlyList<AwardDetailItemDto> Details { get; init; } = [];
}
