namespace Portfolio.Application.DTOs.Lookups;

public sealed class CertificationIssuerDto
{
    public int CertificationIssuerId { get; init; }
    public string IssuerName { get; init; } = string.Empty;
    public string? IssuerWebsite { get; init; }
    public byte[] RowVersion { get; init; } = [];
}

public sealed class CreateCertificationIssuerRequest
{
    public string IssuerName { get; set; } = string.Empty;
    public string? IssuerWebsite { get; set; }
}

public sealed class UpdateCertificationIssuerRequest
{
    public int CertificationIssuerId { get; set; }
    public string IssuerName { get; set; } = string.Empty;
    public string? IssuerWebsite { get; set; }
    public byte[] RowVersion { get; set; } = [];
}
