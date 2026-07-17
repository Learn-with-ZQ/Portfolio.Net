using Portfolio.Domain.Common;

namespace Portfolio.Domain.Entities.Projects;

public sealed class ProjectTechnology : AuditableEntity
{
    public int ProjectTechnologyId { get; set; }
    public int ProjectId { get; set; }
    public int TechnologyId { get; set; }
    public string? TechnologyName { get; set; }
}
