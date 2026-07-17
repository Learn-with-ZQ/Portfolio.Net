namespace Portfolio.Application.DTOs.Experience;

public sealed class CreateExperienceDetailRequest
{
    public int ExperienceId { get; set; }
    public string ExperienceDetailName { get; set; } = string.Empty;
    public int SortOrder { get; set; }
}
