using Portfolio.Domain.Common;

namespace Portfolio.Domain.Entities.Lookups;

public sealed class Course : AuditableEntity
{
    public int CourseId { get; set; }
    public string CourseName { get; set; } = string.Empty;
    public int? InstituteId { get; set; }
    public int SortOrder { get; set; }
}
