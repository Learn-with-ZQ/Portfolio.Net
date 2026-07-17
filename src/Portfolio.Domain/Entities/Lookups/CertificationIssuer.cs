using Portfolio.Domain.Common;

namespace Portfolio.Domain.Entities.Lookups;

public sealed class CertificationIssuer : AuditableEntity
{
    public int CertificationIssuerId { get; set; }
    public string IssuerName { get; set; } = string.Empty;
    public string? IssuerWebsite { get; set; }
}
