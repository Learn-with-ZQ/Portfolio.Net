namespace Portfolio.Application.DTOs.Projects;

public sealed class SyncProjectTechnologiesRequest
{
    public int ProjectId { get; set; }
    public IReadOnlyList<int> TechnologyIds { get; set; } = [];
}
