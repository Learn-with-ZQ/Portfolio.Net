using Portfolio.Application.DTOs.Lookups;
using Portfolio.Domain.Entities.Lookups;

namespace Portfolio.Application.Mappings;

public static class CompanyMapper
{
    public static CompanyDto ToDto(Company entity) => new()
    {
        CompanyId = entity.CompanyId,
        CompanyName = entity.CompanyName,
        WebsiteUrl = entity.WebsiteUrl,
        RowVersion = entity.RowVersion
    };

    public static Company ToEntity(CreateCompanyRequest request) => new()
    {
        CompanyName = request.CompanyName,
        WebsiteUrl = request.WebsiteUrl
    };

    public static void ApplyUpdate(Company entity, UpdateCompanyRequest request)
    {
        entity.CompanyName = request.CompanyName;
        entity.WebsiteUrl = request.WebsiteUrl;
        entity.RowVersion = request.RowVersion;
    }
}

public static class DeployDetailMapper
{
    public static DeployDetailDto ToDto(DeployDetail entity) => new()
    {
        DeployDetailId = entity.DeployDetailId,
        DeployDetailName = entity.DeployDetailName,
        DeployedTo = entity.DeployedTo,
        DeployedByCompanyId = entity.DeployedByCompanyId,
        RowVersion = entity.RowVersion
    };

    public static DeployDetail ToEntity(CreateDeployDetailRequest request) => new()
    {
        DeployDetailName = request.DeployDetailName,
        DeployedTo = request.DeployedTo,
        DeployedByCompanyId = request.DeployedByCompanyId
    };

    public static void ApplyUpdate(DeployDetail entity, UpdateDeployDetailRequest request)
    {
        entity.DeployDetailName = request.DeployDetailName;
        entity.DeployedTo = request.DeployedTo;
        entity.DeployedByCompanyId = request.DeployedByCompanyId;
        entity.RowVersion = request.RowVersion;
    }
}
