namespace Portfolio.Application.DTOs.Certifications;

public sealed class CertificationDetailItemDto
{
    public int CertificationDetailId { get; init; }
    public int CertificationId { get; init; }
    public string DetailText { get; init; } = string.Empty;
    public int SortOrder { get; init; }
    public byte[] RowVersion { get; init; } = [];
}
