using Portfolio.Application.Common.Models;
using Portfolio.Application.DTOs.References;
using Portfolio.Domain.Entities.References;

namespace Portfolio.Application.Mappings;

public static class ReferenceMapper
{
    public static Reference ToEntity(CreateReferenceRequest request) => new()
    {
        PortfolioProfileId = request.PortfolioProfileId,
        FullName = request.FullName,
        Organization = request.Organization,
        Designation = request.Designation,
        Relationship = request.Relationship,
        Email = request.Email,
        Phone = request.Phone,
        LinkedInUrl = request.LinkedInUrl,
        IsContactVisible = request.IsContactVisible,
        IsPublic = request.IsPublic,
        SortOrder = request.SortOrder
    };

    public static void ApplyUpdate(Reference entity, UpdateReferenceRequest request)
    {
        entity.FullName = request.FullName;
        entity.Organization = request.Organization;
        entity.Designation = request.Designation;
        entity.Relationship = request.Relationship;
        entity.Email = request.Email;
        entity.Phone = request.Phone;
        entity.LinkedInUrl = request.LinkedInUrl;
        entity.IsContactVisible = request.IsContactVisible;
        entity.IsPublic = request.IsPublic;
        entity.SortOrder = request.SortOrder;
        entity.RowVersion = request.RowVersion;
    }

    public static ReferenceDetailDto ToDetailDto(Reference entity) => new()
    {
        ReferenceId = entity.ReferenceId,
        PortfolioProfileId = entity.PortfolioProfileId,
        FullName = entity.FullName,
        Organization = entity.Organization,
        Designation = entity.Designation,
        Relationship = entity.Relationship,
        Email = entity.Email,
        Phone = entity.Phone,
        LinkedInUrl = entity.LinkedInUrl,
        IsContactVisible = entity.IsContactVisible,
        IsPublic = entity.IsPublic,
        SortOrder = entity.SortOrder,
        RowVersion = entity.RowVersion,
        Audit = new AuditInfoDto
        {
            CreatedAt = entity.CreatedAt,
            CreatedBy = entity.CreatedBy,
            UpdatedAt = entity.UpdatedAt,
            UpdatedBy = entity.UpdatedBy
        }
    };
}
