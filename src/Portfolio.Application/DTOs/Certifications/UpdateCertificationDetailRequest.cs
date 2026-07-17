namespace Portfolio.Application.DTOs.Certifications;

public sealed class UpdateCertificationDetailRequest
{
    public int CertificationDetailId { get; set; }
    public string DetailText { get; set; } = string.Empty;
    public int SortOrder { get; set; }
    public byte[] RowVersion { get; set; } = [];
}
