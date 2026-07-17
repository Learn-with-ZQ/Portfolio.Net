namespace Portfolio.Application.DTOs.Education;

public sealed class EducationDto
{
    public int EducationId { get; init; }
    public string Degree { get; init; } = string.Empty;
    public string InstituteName { get; init; } = string.Empty;
    public DateOnly StartDate { get; init; }
    public DateOnly? EndDate { get; init; }
    public decimal? Gpa { get; init; }
    public decimal? Cgpa { get; init; }
    public int SortOrder { get; init; }
}
