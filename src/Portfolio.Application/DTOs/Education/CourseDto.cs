namespace Portfolio.Application.DTOs.Education;

public sealed class CourseDto
{
    public int CourseId { get; init; }
    public int? InstituteId { get; init; }
    public string CourseName { get; init; } = string.Empty;
    public int SortOrder { get; init; }
}
