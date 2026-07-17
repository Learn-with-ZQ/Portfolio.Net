using Portfolio.Application.Common.Models;

namespace Portfolio.Application.DTOs.Experience;

public sealed class SearchExperienceRequest : SearchRequestBase
{
    public int? CompanyId { get; set; }
    public bool? IsCurrentOnly { get; set; }
    public DateOnly? StartDateFrom { get; set; }
    public DateOnly? StartDateTo { get; set; }
}
