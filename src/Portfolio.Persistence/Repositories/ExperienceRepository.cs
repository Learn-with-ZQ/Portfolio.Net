using Dapper;
using Portfolio.Application.Abstractions.Persistence;
using Portfolio.Application.Common.Models;
using Portfolio.Application.DTOs.Experience;
using Portfolio.Domain.Entities.Experience;
using Portfolio.Persistence.Common;
using Portfolio.Persistence.Mapping;

namespace Portfolio.Persistence.Repositories;

/// <summary>
/// Experience data access. Parameters are passed as plain hashtables (no prefix);
/// <see cref="Database"/> adds the <c>@P_</c> prefix and the status outputs.
/// </summary>
public sealed class ExperienceRepository : IExperienceRepository
{
    private readonly Database _db;

    public ExperienceRepository(Database db) => _db = db;

    public async Task<Experience?> GetByIdAsync(int experienceId, CancellationToken cancellationToken = default)
    {
        var (_, entity) = await _db.QueryMultipleAsync(
            "dbo.usp_Experience_GetById",
            new Dictionary<string, object?>
            {
                ["ExperienceID_Pk"] = experienceId,
                ["IncludeDeleted"] = false
            },
            async grid =>
            {
                var header = await grid.ReadFirstOrDefaultAsync().ConfigureAwait(false);
                if (header is null)
                {
                    return (Experience?)null;
                }

                var details = (await grid.ReadAsync().ConfigureAwait(false))
                    .Select(DapperColumnMapping.MapExperienceDetail)
                    .ToList();

                var entity = DapperColumnMapping.MapExperience(header);
                entity.Details = details;
                return entity;
            },
            cancellationToken).ConfigureAwait(false);

        return entity;
    }

    public Task<SpExecutionResult> InsertAsync(Experience experience, int? createdBy, CancellationToken cancellationToken = default) =>
        _db.ExecuteAsync("dbo.usp_Experience_Insert", new Dictionary<string, object?>
        {
            ["PortfolioProfileID_Fk"] = experience.PortfolioProfileId,
            ["Designation"] = experience.Designation,
            ["CompanyID_Fk"] = experience.CompanyId,
            ["DeployDetailID_Fk"] = experience.DeployDetailId,
            ["StartDate"] = experience.StartDate,
            ["EndDate"] = experience.EndDate,
            ["SortOrder"] = experience.SortOrder,
            ["CreatedBy"] = createdBy
        }, outIdKey: "OutExperienceID", cancellationToken);

    public Task<SpExecutionResult> UpdateAsync(Experience experience, int? updatedBy, CancellationToken cancellationToken = default) =>
        _db.ExecuteAsync("dbo.usp_Experience_Update", new Dictionary<string, object?>
        {
            ["ExperienceID_Pk"] = experience.ExperienceId,
            ["Designation"] = experience.Designation,
            ["CompanyID_Fk"] = experience.CompanyId,
            ["DeployDetailID_Fk"] = experience.DeployDetailId,
            ["StartDate"] = experience.StartDate,
            ["EndDate"] = experience.EndDate,
            ["SortOrder"] = experience.SortOrder,
            ["RowVersion"] = experience.RowVersion,
            ["UpdatedBy"] = updatedBy
        }, cancellationToken: cancellationToken);

    public Task<SpExecutionResult> DeleteAsync(int experienceId, int? deletedBy, CancellationToken cancellationToken = default) =>
        _db.ExecuteAsync("dbo.usp_Experience_Delete", new Dictionary<string, object?>
        {
            ["ExperienceID_Pk"] = experienceId,
            ["DeletedBy"] = deletedBy
        }, cancellationToken: cancellationToken);

    public async Task<IReadOnlyList<ExperienceDto>> SearchAsync(SearchExperienceRequest request, CancellationToken cancellationToken = default)
    {
        var (status, rows) = await _db.QueryAsync<dynamic>("dbo.usp_Experience_Search", new Dictionary<string, object?>
        {
            ["PortfolioProfileID_Fk"] = request.PortfolioProfileId,
            ["SearchTerm"] = request.SearchTerm,
            ["CompanyID_Fk"] = request.CompanyId,
            ["IsCurrentOnly"] = request.IsCurrentOnly,
            ["StartDateFrom"] = request.StartDateFrom,
            ["StartDateTo"] = request.StartDateTo
        }, cancellationToken).ConfigureAwait(false);

        if (!status.IsSuccess)
        {
            throw new InvalidOperationException(status.StatusMessage);
        }

        return rows.Select(MapListDto).ToList();
    }

    public async Task<PagedResult<ExperienceDto>> GetPagedAsync(GetExperiencePagedRequest request, CancellationToken cancellationToken = default)
    {
        var (status, rows, totalRecords, totalPages) = await _db.QueryPagedAsync<dynamic>(
            "dbo.usp_Experience_GetPaged", new Dictionary<string, object?>
            {
                ["PortfolioProfileID_Fk"] = request.PortfolioProfileId,
                ["PageNumber"] = request.PageNumber,
                ["PageSize"] = request.PageSize,
                ["SearchTerm"] = request.SearchTerm,
                ["CompanyID_Fk"] = request.CompanyId,
                ["IsCurrentOnly"] = request.IsCurrentOnly
            }, cancellationToken).ConfigureAwait(false);

        if (!status.IsSuccess)
        {
            throw new InvalidOperationException(status.StatusMessage);
        }

        return new PagedResult<ExperienceDto>
        {
            Items = rows.Select(MapListDto).ToList(),
            PageNumber = request.PageNumber,
            PageSize = request.PageSize,
            TotalRecords = totalRecords,
            TotalPages = totalPages
        };
    }

    public Task<SpExecutionResult> InsertDetailAsync(ExperienceDetail detail, int? createdBy, CancellationToken cancellationToken = default) =>
        _db.ExecuteAsync("dbo.usp_ExperienceDetail_Insert", new Dictionary<string, object?>
        {
            ["ExperienceID_Fk"] = detail.ExperienceId,
            ["ExperienceDetailName"] = detail.ExperienceDetailName,
            ["SortOrder"] = detail.SortOrder,
            ["CreatedBy"] = createdBy
        }, outIdKey: "OutExperienceDetailID", cancellationToken);

    public Task<SpExecutionResult> UpdateDetailAsync(ExperienceDetail detail, int? updatedBy, CancellationToken cancellationToken = default) =>
        _db.ExecuteAsync("dbo.usp_ExperienceDetail_Update", new Dictionary<string, object?>
        {
            ["ExperienceDetailID_Pk"] = detail.ExperienceDetailId,
            ["ExperienceDetailName"] = detail.ExperienceDetailName,
            ["SortOrder"] = detail.SortOrder,
            ["RowVersion"] = detail.RowVersion,
            ["UpdatedBy"] = updatedBy
        }, cancellationToken: cancellationToken);

    public Task<SpExecutionResult> DeleteDetailAsync(int experienceDetailId, int? deletedBy, CancellationToken cancellationToken = default) =>
        _db.ExecuteAsync("dbo.usp_ExperienceDetail_Delete", new Dictionary<string, object?>
        {
            ["ExperienceDetailID_Pk"] = experienceDetailId,
            ["DeletedBy"] = deletedBy
        }, cancellationToken: cancellationToken);

    private static ExperienceDto MapListDto(dynamic row)
    {
        var r = (IDictionary<string, object>)row;
        return new ExperienceDto
        {
            ExperienceId = Convert.ToInt32(r["ExperienceID_Pk"]),
            Designation = Convert.ToString(r["Designation"]) ?? string.Empty,
            CompanyName = r.TryGetValue("CompanyName", out var company) && company is not DBNull
                ? Convert.ToString(company) : null,
            StartDate = DateOnly.FromDateTime(Convert.ToDateTime(r["StartDate"])),
            EndDate = r.TryGetValue("EndDate", out var end) && end is not DBNull
                ? DateOnly.FromDateTime(Convert.ToDateTime(end)) : null,
            IsCurrent = Convert.ToBoolean(r["IsCurrent"]),
            SortOrder = Convert.ToInt32(r["SortOrder"])
        };
    }
}
