using Portfolio.Domain.Common;

namespace Portfolio.Domain.Entities.Experience;

public sealed class Experience : AuditableEntity
{
    public int ExperienceId { get; set; }
    public int PortfolioProfileId { get; set; }
    public string Designation { get; set; } = string.Empty;
    public int? CompanyId { get; set; }
    public int? DeployDetailId { get; set; }
    public DateOnly StartDate { get; set; }
    public DateOnly? EndDate { get; set; }
    public int SortOrder { get; set; }
    public bool IsCurrent { get; set; }

    public string? CompanyName { get; set; }
    public string? DeployDetailName { get; set; }
    public string? DeployedTo { get; set; }

    public ICollection<ExperienceDetail> Details { get; set; } = [];
}
