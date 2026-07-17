namespace Portfolio.Application.DTOs.Experience;

public sealed class UpdateExperienceDetailRequest
{
    public int ExperienceDetailId { get; set; }
    public string ExperienceDetailName { get; set; } = string.Empty;
    public int SortOrder { get; set; }
    public byte[] RowVersion { get; set; } = [];
}
