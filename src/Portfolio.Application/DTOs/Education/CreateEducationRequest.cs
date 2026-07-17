using Portfolio.Application.Common.Models;

namespace Portfolio.Application.DTOs.Education;

public sealed class CreateEducationRequest : ModuleRequestBase
{
    public int DegreeLevelId { get; set; }
    public int DegreeId { get; set; }
    public int InstituteId { get; set; }
    public DateOnly StartDate { get; set; }
    public DateOnly? EndDate { get; set; }
    public decimal? Gpa { get; set; }
    public decimal? Cgpa { get; set; }
    public int SortOrder { get; set; }
}
