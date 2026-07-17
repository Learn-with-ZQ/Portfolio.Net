namespace Portfolio.Application.DTOs.Certifications;

public sealed class UpdateCertificationRequest
{
    public int CertificationId { get; set; }
    public int CertificationIssuerId { get; set; }
    public string CertificationName { get; set; } = string.Empty;
    public string? CredentialId { get; set; }
    public string? CredentialUrl { get; set; }
    public DateOnly IssueDate { get; set; }
    public DateOnly? ExpiryDate { get; set; }
    public bool DoesNotExpire { get; set; }
    public int SortOrder { get; set; }
    public byte[] RowVersion { get; set; } = [];
}
