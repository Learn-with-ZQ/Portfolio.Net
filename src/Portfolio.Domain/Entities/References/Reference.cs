using Portfolio.Domain.Common;

namespace Portfolio.Domain.Entities.References;

public sealed class Reference : AuditableEntity
{
    public int ReferenceId { get; set; }
    public int PortfolioProfileId { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string? Organization { get; set; }
    public string? Designation { get; set; }
    /// <summary>Manager, Team Lead, Professor, Mentor, Client, etc.</summary>
    public string? Relationship { get; set; }
    public string? Email { get; set; }
    public string? Phone { get; set; }
    public string? LinkedInUrl { get; set; }
    /// <summary>Permission-based contact visibility — hides email/phone publicly when false.</summary>
    public bool IsContactVisible { get; set; }
    public bool IsPublic { get; set; } = true;
    public int SortOrder { get; set; }
}
