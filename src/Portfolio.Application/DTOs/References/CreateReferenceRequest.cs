using Portfolio.Application.Common.Models;

namespace Portfolio.Application.DTOs.References;

public sealed class CreateReferenceRequest : ModuleRequestBase
{
    public string FullName { get; set; } = string.Empty;
    public string? Organization { get; set; }
    public string? Designation { get; set; }
    public string? Relationship { get; set; }
    public string? Email { get; set; }
    public string? Phone { get; set; }
    public string? LinkedInUrl { get; set; }
    public bool IsContactVisible { get; set; }
    public bool IsPublic { get; set; } = true;
    public int SortOrder { get; set; }
}
