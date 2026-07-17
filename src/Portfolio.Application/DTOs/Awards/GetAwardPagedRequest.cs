using Portfolio.Application.Common.Models;

namespace Portfolio.Application.DTOs.Awards;

public sealed class GetAwardPagedRequest : PagedRequestBase
{
    public DateOnly? StartDateFrom { get; set; }
    public DateOnly? StartDateTo { get; set; }
}
