namespace Portfolio.Application.DTOs.Awards;

public sealed class AwardDetailItemDto
{
    public int AwardDetailId { get; init; }
    public int AwardId { get; init; }
    public string AwardDetailName { get; init; } = string.Empty;
    public int SortOrder { get; init; }
    public byte[] RowVersion { get; init; } = [];
}
