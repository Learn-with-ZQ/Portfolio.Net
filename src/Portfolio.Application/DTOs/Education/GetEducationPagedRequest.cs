using Portfolio.Application.Common.Models;

namespace Portfolio.Application.DTOs.Education;

public sealed class GetEducationPagedRequest : PagedRequestBase
{
    public int? InstituteId { get; set; }
}
