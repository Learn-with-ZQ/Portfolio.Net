namespace Portfolio.Application.DTOs.Awards;

public sealed class UpdateAwardDetailRequest
{
    public int AwardDetailId { get; set; }
    public string AwardDetailName { get; set; } = string.Empty;
    public int SortOrder { get; set; }
    public byte[] RowVersion { get; set; } = [];
}
