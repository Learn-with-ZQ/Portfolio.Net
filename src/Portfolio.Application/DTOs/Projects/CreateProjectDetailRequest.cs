namespace Portfolio.Application.DTOs.Projects;

public sealed class CreateProjectDetailRequest
{
    public int ProjectId { get; set; }
    public string ProjectDetailName { get; set; } = string.Empty;
    public int SortOrder { get; set; }
}
