using Portfolio.Domain.Common;

namespace Portfolio.Domain.Entities.Projects;

public sealed class ProjectDetail : AuditableEntity
{
    public int ProjectDetailId { get; set; }
    public int ProjectId { get; set; }
    public string ProjectDetailName { get; set; } = string.Empty;
    public int SortOrder { get; set; }
}
