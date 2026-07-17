namespace Portfolio.Application.DTOs.Projects;

public sealed class UpdateProjectRequest
{
    public int ProjectId { get; set; }
    public string ProjectName { get; set; } = string.Empty;
    public string? ProjectSummary { get; set; }
    public string? Practice { get; set; }
    public int? CompanyId { get; set; }
    public int? CourseId { get; set; }
    public DateOnly StartDate { get; set; }
    public DateOnly? EndDate { get; set; }
    public int SortOrder { get; set; }
    public byte[] RowVersion { get; set; } = [];
}
