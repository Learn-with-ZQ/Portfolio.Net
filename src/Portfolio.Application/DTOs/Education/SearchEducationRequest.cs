using Portfolio.Application.Common.Models;

namespace Portfolio.Application.DTOs.Education;

public sealed class SearchEducationRequest : SearchRequestBase
{
    public int? InstituteId { get; set; }
    public int? DegreeLevelId { get; set; }
}
