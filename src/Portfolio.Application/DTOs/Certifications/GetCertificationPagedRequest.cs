using Portfolio.Application.Common.Models;

namespace Portfolio.Application.DTOs.Certifications;

public sealed class GetCertificationPagedRequest : PagedRequestBase
{
    public int? CertificationIssuerId { get; set; }
    public bool? ActiveOnly { get; set; }
}
