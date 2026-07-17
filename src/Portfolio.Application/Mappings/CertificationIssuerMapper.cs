using Portfolio.Application.DTOs.Lookups;
using Portfolio.Domain.Entities.Lookups;

namespace Portfolio.Application.Mappings;

public static class CertificationIssuerMapper
{
    public static CertificationIssuerDto ToDto(CertificationIssuer entity) => new()
    {
        CertificationIssuerId = entity.CertificationIssuerId,
        IssuerName = entity.IssuerName,
        IssuerWebsite = entity.IssuerWebsite,
        RowVersion = entity.RowVersion
    };

    public static CertificationIssuer ToEntity(CreateCertificationIssuerRequest request) => new()
    {
        IssuerName = request.IssuerName,
        IssuerWebsite = request.IssuerWebsite
    };

    public static void ApplyUpdate(CertificationIssuer entity, UpdateCertificationIssuerRequest request)
    {
        entity.IssuerName = request.IssuerName;
        entity.IssuerWebsite = request.IssuerWebsite;
        entity.RowVersion = request.RowVersion;
    }
}
