namespace Portfolio.Application.DTOs.Projects;

public sealed class ProjectDetailItemDto
{
    public int ProjectDetailId { get; init; }
    public int ProjectId { get; init; }
    public string ProjectDetailName { get; init; } = string.Empty;
    public int SortOrder { get; init; }
    public byte[] RowVersion { get; init; } = [];
}
