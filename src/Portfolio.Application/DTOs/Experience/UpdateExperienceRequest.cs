namespace Portfolio.Application.DTOs.Experience;

public sealed class UpdateExperienceRequest
{
    public int ExperienceId { get; set; }
    public string Designation { get; set; } = string.Empty;
    public int? CompanyId { get; set; }
    public int? DeployDetailId { get; set; }
    public DateOnly StartDate { get; set; }
    public DateOnly? EndDate { get; set; }
    public int SortOrder { get; set; }
    public byte[] RowVersion { get; set; } = [];
}
