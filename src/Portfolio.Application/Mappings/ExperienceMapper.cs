using Portfolio.Application.Common.Models;
using Portfolio.Application.DTOs.Experience;
using Portfolio.Domain.Entities.Experience;

namespace Portfolio.Application.Mappings;

public static class ExperienceMapper
{
    public static ExperienceDto ToDto(Experience entity) => new()
    {
        ExperienceId = entity.ExperienceId,
        Designation = entity.Designation,
        CompanyName = entity.CompanyName,
        StartDate = entity.StartDate,
        EndDate = entity.EndDate,
        IsCurrent = entity.IsCurrent,
        SortOrder = entity.SortOrder
    };

    public static ExperienceDetailDto ToDetailDto(Experience entity) => new()
    {
        ExperienceId = entity.ExperienceId,
        PortfolioProfileId = entity.PortfolioProfileId,
        Designation = entity.Designation,
        CompanyId = entity.CompanyId,
        CompanyName = entity.CompanyName,
        DeployDetailId = entity.DeployDetailId,
        DeployDetailName = entity.DeployDetailName,
        DeployedTo = entity.DeployedTo,
        StartDate = entity.StartDate,
        EndDate = entity.EndDate,
        IsCurrent = entity.IsCurrent,
        SortOrder = entity.SortOrder,
        RowVersion = entity.RowVersion,
        Audit = new AuditInfoDto
        {
            CreatedAt = entity.CreatedAt,
            CreatedBy = entity.CreatedBy,
            UpdatedAt = entity.UpdatedAt,
            UpdatedBy = entity.UpdatedBy
        },
        Details = entity.Details.Select(ToDetailItemDto).ToList()
    };

    public static ExperienceDetailItemDto ToDetailItemDto(ExperienceDetail detail) => new()
    {
        ExperienceDetailId = detail.ExperienceDetailId,
        ExperienceId = detail.ExperienceId,
        ExperienceDetailName = detail.ExperienceDetailName,
        SortOrder = detail.SortOrder,
        RowVersion = detail.RowVersion
    };

    public static Experience ToEntity(CreateExperienceRequest request) => new()
    {
        PortfolioProfileId = request.PortfolioProfileId,
        Designation = request.Designation,
        CompanyId = request.CompanyId,
        DeployDetailId = request.DeployDetailId,
        StartDate = request.StartDate,
        EndDate = request.EndDate,
        SortOrder = request.SortOrder
    };

    public static void ApplyUpdate(Experience entity, UpdateExperienceRequest request)
    {
        entity.Designation = request.Designation;
        entity.CompanyId = request.CompanyId;
        entity.DeployDetailId = request.DeployDetailId;
        entity.StartDate = request.StartDate;
        entity.EndDate = request.EndDate;
        entity.SortOrder = request.SortOrder;
        entity.RowVersion = request.RowVersion;
    }

    public static ExperienceDetail ToDetailEntity(CreateExperienceDetailRequest request) => new()
    {
        ExperienceId = request.ExperienceId,
        ExperienceDetailName = request.ExperienceDetailName,
        SortOrder = request.SortOrder
    };

    public static void ApplyDetailUpdate(ExperienceDetail entity, UpdateExperienceDetailRequest request)
    {
        entity.ExperienceDetailName = request.ExperienceDetailName;
        entity.SortOrder = request.SortOrder;
        entity.RowVersion = request.RowVersion;
    }
}
