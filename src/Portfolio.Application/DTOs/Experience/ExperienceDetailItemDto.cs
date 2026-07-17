namespace Portfolio.Application.DTOs.Experience;

public sealed class ExperienceDetailItemDto
{
    public int ExperienceDetailId { get; init; }
    public int ExperienceId { get; init; }
    public string ExperienceDetailName { get; init; } = string.Empty;
    public int SortOrder { get; init; }
    public byte[] RowVersion { get; init; } = [];
}
