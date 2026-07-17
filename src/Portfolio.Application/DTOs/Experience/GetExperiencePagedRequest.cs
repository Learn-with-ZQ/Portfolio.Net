using Portfolio.Application.Common.Models;

namespace Portfolio.Application.DTOs.Experience;

public sealed class GetExperiencePagedRequest : PagedRequestBase
{
    public int? CompanyId { get; set; }
    public bool? IsCurrentOnly { get; set; }
}
