namespace Portfolio.Application.DTOs.Projects;

public sealed class ProjectTechnologyDto
{
    public int ProjectTechnologyId { get; init; }
    public int TechnologyId { get; init; }
    public string TechnologyName { get; init; } = string.Empty;
    public byte[] RowVersion { get; init; } = [];
}
