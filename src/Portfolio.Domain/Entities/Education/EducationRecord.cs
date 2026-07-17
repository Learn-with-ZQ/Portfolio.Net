using Portfolio.Domain.Common;

namespace Portfolio.Domain.Entities.Education;

public sealed class EducationRecord : AuditableEntity
{
    public int EducationId { get; set; }
    public int PortfolioProfileId { get; set; }
    public int DegreeLevelId { get; set; }
    public int DegreeId { get; set; }
    public int InstituteId { get; set; }
    public DateOnly StartDate { get; set; }
    public DateOnly? EndDate { get; set; }
    public decimal? Gpa { get; set; }
    public decimal? Cgpa { get; set; }
    public int SortOrder { get; set; }

    public string? DegreeLevelName { get; set; }
    public string? DegreePrefix { get; set; }
    public string? DegreeName { get; set; }
    public string? InstituteName { get; set; }
    public string? Location { get; set; }
    public ICollection<CourseRecord> Courses { get; set; } = [];
}

public sealed class CourseRecord
{
    public int CourseId { get; set; }
    public int? InstituteId { get; set; }
    public string CourseName { get; set; } = string.Empty;
    public int SortOrder { get; set; }
}
