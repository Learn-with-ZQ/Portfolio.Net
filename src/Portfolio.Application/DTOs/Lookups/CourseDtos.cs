namespace Portfolio.Application.DTOs.Lookups;

public sealed class CourseDto
{
    public int CourseId { get; init; }
    public string CourseName { get; init; } = string.Empty;
    public int? InstituteId { get; init; }
    public string? InstituteName { get; init; }
    public int SortOrder { get; init; }
    public byte[] RowVersion { get; init; } = [];
}

public sealed class CreateCourseRequest
{
    public string CourseName { get; set; } = string.Empty;
    public int? InstituteId { get; set; }
    public int SortOrder { get; set; }
}

public sealed class UpdateCourseRequest
{
    public int CourseId { get; set; }
    public string CourseName { get; set; } = string.Empty;
    public int? InstituteId { get; set; }
    public int SortOrder { get; set; }
    public byte[] RowVersion { get; set; } = [];
}
