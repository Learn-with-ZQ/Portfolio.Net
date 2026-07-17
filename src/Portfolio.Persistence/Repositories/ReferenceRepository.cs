using Portfolio.Application.Abstractions.Persistence;
using Portfolio.Application.Common.Models;
using Portfolio.Application.DTOs.References;
using Portfolio.Domain.Entities.References;
using Portfolio.Persistence.Common;

namespace Portfolio.Persistence.Repositories;

public sealed class ReferenceRepository : IReferenceRepository
{
    private readonly Database _db;
    public ReferenceRepository(Database db) => _db = db;

    public async Task<Reference?> GetByIdAsync(int referenceId, CancellationToken cancellationToken = default)
    {
        var (_, item) = await _db.QuerySingleAsync<Reference>("dbo.usp_References_GetById",
            new Dictionary<string, object?> { ["ReferenceID_Pk"] = referenceId, ["IncludeDeleted"] = false },
            cancellationToken).ConfigureAwait(false);
        return item;
    }

    public Task<SpExecutionResult> InsertAsync(Reference r, int? createdBy, CancellationToken cancellationToken = default) =>
        _db.ExecuteAsync("dbo.usp_References_Insert", new Dictionary<string, object?>
        {
            ["PortfolioProfileID_Fk"] = r.PortfolioProfileId,
            ["FullName"] = r.FullName,
            ["Organization"] = r.Organization,
            ["Designation"] = r.Designation,
            ["Relationship"] = r.Relationship,
            ["Email"] = r.Email,
            ["Phone"] = r.Phone,
            ["LinkedInUrl"] = r.LinkedInUrl,
            ["IsContactVisible"] = r.IsContactVisible,
            ["IsPublic"] = r.IsPublic,
            ["SortOrder"] = r.SortOrder,
            ["CreatedBy"] = createdBy
        }, outIdKey: "OutReferenceID", cancellationToken);

    public Task<SpExecutionResult> UpdateAsync(Reference r, int? updatedBy, CancellationToken cancellationToken = default) =>
        _db.ExecuteAsync("dbo.usp_References_Update", new Dictionary<string, object?>
        {
            ["ReferenceID_Pk"] = r.ReferenceId,
            ["PortfolioProfileID_Fk"] = r.PortfolioProfileId,
            ["FullName"] = r.FullName,
            ["Organization"] = r.Organization,
            ["Designation"] = r.Designation,
            ["Relationship"] = r.Relationship,
            ["Email"] = r.Email,
            ["Phone"] = r.Phone,
            ["LinkedInUrl"] = r.LinkedInUrl,
            ["IsContactVisible"] = r.IsContactVisible,
            ["IsPublic"] = r.IsPublic,
            ["SortOrder"] = r.SortOrder,
            ["RowVersion"] = r.RowVersion,
            ["UpdatedBy"] = updatedBy
        }, cancellationToken: cancellationToken);

    public Task<SpExecutionResult> DeleteAsync(int referenceId, int? deletedBy, CancellationToken cancellationToken = default) =>
        _db.ExecuteAsync("dbo.usp_References_Delete", new Dictionary<string, object?>
        {
            ["ReferenceID_Pk"] = referenceId,
            ["DeletedBy"] = deletedBy
        }, cancellationToken: cancellationToken);

    public async Task<PagedResult<ReferenceDto>> GetPagedAsync(GetReferencePagedRequest request, CancellationToken cancellationToken = default)
    {
        var (_, rows, total, pages) = await _db.QueryPagedAsync<ReferenceDto>("dbo.usp_References_GetPaged",
            new Dictionary<string, object?>
            {
                ["PortfolioProfileID_Fk"] = request.PortfolioProfileId,
                ["PageNumber"] = request.PageNumber,
                ["PageSize"] = request.PageSize,
                ["SearchTerm"] = request.SearchTerm,
                ["IsPublic"] = request.IsPublic
            }, cancellationToken).ConfigureAwait(false);

        return new PagedResult<ReferenceDto>
        {
            Items = rows,
            PageNumber = request.PageNumber,
            PageSize = request.PageSize,
            TotalRecords = total,
            TotalPages = pages
        };
    }
}
