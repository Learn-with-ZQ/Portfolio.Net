using Portfolio.Domain.Common;

namespace Portfolio.Domain.Entities.Certifications;

public sealed class Certification : AuditableEntity
{
    public int CertificationId { get; set; }
    public int PortfolioProfileId { get; set; }
    public int CertificationIssuerId { get; set; }
    public string CertificationName { get; set; } = string.Empty;
    public string? CredentialId { get; set; }
    public string? CredentialUrl { get; set; }
    public DateOnly IssueDate { get; set; }
    public DateOnly? ExpiryDate { get; set; }
    public bool DoesNotExpire { get; set; }
    public int SortOrder { get; set; }

    public string? IssuerName { get; set; }
    public string? IssuerWebsite { get; set; }

    public ICollection<CertificationDetail> Details { get; set; } = [];
}
