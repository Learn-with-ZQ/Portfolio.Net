namespace Portfolio.Application.DTOs.Awards;

public sealed class CreateAwardDetailRequest
{
    public int AwardId { get; set; }
    public string AwardDetailName { get; set; } = string.Empty;
    public int SortOrder { get; set; }
}
