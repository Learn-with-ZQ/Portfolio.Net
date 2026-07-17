namespace Portfolio.Application.DTOs.References;

/// <summary>Public-safe reference. Email/Phone are redacted unless IsContactVisible.</summary>
public sealed class ReferenceDto
{
    public int ReferenceId { get; init; }
    public string FullName { get; init; } = string.Empty;
    public string? Organization { get; init; }
    public string? Designation { get; init; }
    public string? Relationship { get; init; }
    public string? Email { get; init; }
    public string? Phone { get; init; }
    public string? LinkedInUrl { get; init; }
    public bool IsContactVisible { get; init; }
    public int SortOrder { get; init; }
}
