namespace Portfolio.Application.DTOs.Certifications;

public sealed class CreateCertificationDetailRequest
{
    public int CertificationId { get; set; }
    public string DetailText { get; set; } = string.Empty;
    public int SortOrder { get; set; }
}
