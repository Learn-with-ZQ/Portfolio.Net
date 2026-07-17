using Portfolio.Application.Common.Models;

namespace Portfolio.Application.DTOs.Experience;

public sealed class ExperienceDetailDto
{
    public int ExperienceId { get; init; }
    public int PortfolioProfileId { get; init; }
    public string Designation { get; init; } = string.Empty;
    public int? CompanyId { get; init; }
    public string? CompanyName { get; init; }
    public int? DeployDetailId { get; init; }
    public string? DeployDetailName { get; init; }
    public string? DeployedTo { get; init; }
    public DateOnly StartDate { get; init; }
    public DateOnly? EndDate { get; init; }
    public bool IsCurrent { get; init; }
    public int SortOrder { get; init; }
    public byte[] RowVersion { get; init; } = [];
    public AuditInfoDto Audit { get; init; } = new();
    public IReadOnlyList<ExperienceDetailItemDto> Details { get; init; } = [];
}
