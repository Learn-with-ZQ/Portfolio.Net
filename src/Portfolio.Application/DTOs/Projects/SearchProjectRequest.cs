using Portfolio.Application.Common.Models;

namespace Portfolio.Application.DTOs.Projects;

public sealed class SearchProjectRequest : SearchRequestBase
{
    public int? CompanyId { get; set; }
    public int? CourseId { get; set; }
    public int? TechnologyId { get; set; }
}
