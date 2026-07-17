using Portfolio.Application.Common.Models;

namespace Portfolio.Application.DTOs.Certifications;

public sealed class CertificationDetailDto
{
    public int CertificationId { get; init; }
    public int PortfolioProfileId { get; init; }
    public int CertificationIssuerId { get; init; }
    public string IssuerName { get; init; } = string.Empty;
    public string? IssuerWebsite { get; init; }
    public string CertificationName { get; init; } = string.Empty;
    public string? CredentialId { get; init; }
    public string? CredentialUrl { get; init; }
    public DateOnly IssueDate { get; init; }
    public DateOnly? ExpiryDate { get; init; }
    public bool DoesNotExpire { get; init; }
    public int SortOrder { get; init; }
    public byte[] RowVersion { get; init; } = [];
    public AuditInfoDto Audit { get; init; } = new();
    public IReadOnlyList<CertificationDetailItemDto> Details { get; init; } = [];
}
