using Portfolio.Application.Common.Models;

namespace Portfolio.Application.DTOs.Education;

public sealed class EducationDetailDto
{
    public int EducationId { get; init; }
    public int PortfolioProfileId { get; init; }
    public int DegreeLevelId { get; init; }
    public string DegreeLevelName { get; init; } = string.Empty;
    public string DegreePrefix { get; init; } = string.Empty;
    public int DegreeId { get; init; }
    public string DegreeName { get; init; } = string.Empty;
    public int InstituteId { get; init; }
    public string InstituteName { get; init; } = string.Empty;
    public string? Location { get; init; }
    public DateOnly StartDate { get; init; }
    public DateOnly? EndDate { get; init; }
    public decimal? Gpa { get; init; }
    public decimal? Cgpa { get; init; }
    public int SortOrder { get; init; }
    public byte[] RowVersion { get; init; } = [];
    public AuditInfoDto Audit { get; init; } = new();
    public IReadOnlyList<CourseDto> Courses { get; init; } = [];
}
