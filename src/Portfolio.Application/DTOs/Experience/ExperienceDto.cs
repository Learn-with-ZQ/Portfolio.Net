namespace Portfolio.Application.DTOs.Experience;

public sealed class ExperienceDto
{
    public int ExperienceId { get; init; }
    public string Designation { get; init; } = string.Empty;
    public string? CompanyName { get; init; }
    public DateOnly StartDate { get; init; }
    public DateOnly? EndDate { get; init; }
    public bool IsCurrent { get; init; }
    public int SortOrder { get; init; }
}
