namespace Portfolio.Application.DTOs.Awards;

public sealed class UpdateAwardRequest
{
    public int AwardId { get; set; }
    public string AwardName { get; set; } = string.Empty;
    public DateOnly StartDate { get; set; }
    public DateOnly? EndDate { get; set; }
    public int SortOrder { get; set; }
    public byte[] RowVersion { get; set; } = [];
}
