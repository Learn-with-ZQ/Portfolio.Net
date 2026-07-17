namespace Portfolio.Application.DTOs.Projects;

public sealed class ProjectDto
{
    public int ProjectId { get; init; }
    public string ProjectName { get; init; } = string.Empty;
    public string? ContextName { get; init; }
    public string Technologies { get; init; } = string.Empty;
    public DateOnly StartDate { get; init; }
    public DateOnly? EndDate { get; init; }
    public int SortOrder { get; init; }
}
