using Portfolio.Application.Common.Models;

namespace Portfolio.Application.DTOs.Projects;

public sealed class ProjectDetailDto
{
    public int ProjectId { get; init; }
    public int PortfolioProfileId { get; init; }
    public string ProjectName { get; init; } = string.Empty;
    public string? ProjectSummary { get; init; }
    public string? Practice { get; init; }
    public int? CompanyId { get; init; }
    public string? CompanyName { get; init; }
    public int? CourseId { get; init; }
    public string? CourseName { get; init; }
    public DateOnly StartDate { get; init; }
    public DateOnly? EndDate { get; init; }
    public int SortOrder { get; init; }
    public byte[] RowVersion { get; init; } = [];
    public AuditInfoDto Audit { get; init; } = new();
    public IReadOnlyList<ProjectTechnologyDto> Technologies { get; init; } = [];
    public IReadOnlyList<ProjectDetailItemDto> Details { get; init; } = [];
}
