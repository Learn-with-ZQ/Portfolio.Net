using Portfolio.Domain.Common;

namespace Portfolio.Domain.Entities.Projects;

public sealed class Project : AuditableEntity
{
    public int ProjectId { get; set; }
    public int PortfolioProfileId { get; set; }
    public string ProjectName { get; set; } = string.Empty;
    public string? ProjectSummary { get; set; }
    public string? Practice { get; set; }
    public int? CompanyId { get; set; }
    public int? CourseId { get; set; }
    public DateOnly StartDate { get; set; }
    public DateOnly? EndDate { get; set; }
    public int SortOrder { get; set; }

    public string? CompanyName { get; set; }
    public string? CourseName { get; set; }

    public ICollection<ProjectDetail> Details { get; set; } = [];
    public ICollection<ProjectTechnology> Technologies { get; set; } = [];
}
