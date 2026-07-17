using Portfolio.Application.Common.Models;

namespace Portfolio.Application.DTOs.Certifications;

public sealed class SearchCertificationRequest : SearchRequestBase
{
    public int? CertificationIssuerId { get; set; }
    public bool? ActiveOnly { get; set; }
}
