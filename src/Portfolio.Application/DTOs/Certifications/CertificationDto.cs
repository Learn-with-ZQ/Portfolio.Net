namespace Portfolio.Application.DTOs.Certifications;

public sealed class CertificationDto
{
    public int CertificationId { get; init; }
    public string CertificationName { get; init; } = string.Empty;
    public string IssuerName { get; init; } = string.Empty;
    public string? CredentialId { get; init; }
    public DateOnly IssueDate { get; init; }
    public DateOnly? ExpiryDate { get; init; }
    public bool DoesNotExpire { get; init; }
    public int SortOrder { get; init; }
}
