namespace Portfolio.Application.DTOs.References;

public sealed class UpdateReferenceRequest
{
    public int ReferenceId { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string? Organization { get; set; }
    public string? Designation { get; set; }
    public string? Relationship { get; set; }
    public string? Email { get; set; }
    public string? Phone { get; set; }
    public string? LinkedInUrl { get; set; }
    public bool IsContactVisible { get; set; }
    public bool IsPublic { get; set; }
    public int SortOrder { get; set; }
    public byte[] RowVersion { get; set; } = [];
}
