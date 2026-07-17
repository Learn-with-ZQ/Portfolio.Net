using Portfolio.Application.Common.Models;

namespace Portfolio.Application.DTOs.Projects;

public sealed class GetProjectPagedRequest : PagedRequestBase
{
    public int? CompanyId { get; set; }
    public int? TechnologyId { get; set; }
}
