using Portfolio.Application.Common.Models;

namespace Portfolio.Application.DTOs.Awards;

public sealed class CreateAwardRequest : ModuleRequestBase
{
    public string AwardName { get; set; } = string.Empty;
    public DateOnly StartDate { get; set; }
    public DateOnly? EndDate { get; set; }
    public int SortOrder { get; set; }
}
