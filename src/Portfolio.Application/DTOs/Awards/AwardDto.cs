namespace Portfolio.Application.DTOs.Awards;

public sealed class AwardDto
{
    public int AwardId { get; init; }
    public string AwardName { get; init; } = string.Empty;
    public DateOnly StartDate { get; init; }
    public DateOnly? EndDate { get; init; }
    public int SortOrder { get; init; }
}
