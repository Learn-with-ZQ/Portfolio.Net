using Portfolio.Domain.Common;

namespace Portfolio.Domain.Entities.Certifications;

public sealed class CertificationDetail : AuditableEntity
{
    public int CertificationDetailId { get; set; }
    public int CertificationId { get; set; }
    public string DetailText { get; set; } = string.Empty;
    public int SortOrder { get; set; }
}
