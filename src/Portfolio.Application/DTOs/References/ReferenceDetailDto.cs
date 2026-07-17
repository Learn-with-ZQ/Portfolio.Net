using Portfolio.Application.Common.Models;

namespace Portfolio.Application.DTOs.References;

public sealed class ReferenceDetailDto
{
    public int ReferenceId { get; init; }
    public int PortfolioProfileId { get; init; }
    public string FullName { get; init; } = string.Empty;
    public string? Organization { get; init; }
    public string? Designation { get; init; }
    public string? Relationship { get; init; }
    public string? Email { get; init; }
    public string? Phone { get; init; }
    public string? LinkedInUrl { get; init; }
    public bool IsContactVisible { get; init; }
    public bool IsPublic { get; init; }
    public int SortOrder { get; init; }
    public byte[] RowVersion { get; init; } = [];
    public AuditInfoDto Audit { get; init; } = new();
}
