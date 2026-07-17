using Portfolio.Application.Abstractions.Persistence;
using Portfolio.Application.Common.Models;
using Portfolio.Application.DTOs.Lookups;
using Portfolio.Domain.Entities.Lookups;
using Portfolio.Persistence.Common;

namespace Portfolio.Persistence.Repositories;

public sealed class TechnologyRepository : ITechnologyRepository
{
    private readonly Database _db;

    public TechnologyRepository(Database db) => _db = db;

    public async Task<IReadOnlyList<LookupItemDto>> GetLookupAsync(CancellationToken cancellationToken = default)
    {
        var (_, rows) = await _db.QueryAsync<LookupItemDto>(
            "dbo.usp_Technology_GetLookup", new Dictionary<string, object?>(), cancellationToken).ConfigureAwait(false);
        return rows;
    }

    public async Task<PagedResult<TechnologyDto>> GetPagedAsync(GetLookupPagedRequest request, CancellationToken cancellationToken = default)
    {
        var (_, rows, total, pages) = await _db.QueryPagedAsync<TechnologyDto>("dbo.usp_Technology_GetPaged", new Dictionary<string, object?>
        {
            ["PageNumber"] = request.PageNumber,
            ["PageSize"] = request.PageSize,
            ["SearchTerm"] = request.SearchTerm
        }, cancellationToken).ConfigureAwait(false);
        return new PagedResult<TechnologyDto> { Items = rows, PageNumber = request.PageNumber, PageSize = request.PageSize, TotalRecords = total, TotalPages = pages };
    }

    public async Task<Technology?> GetByIdAsync(int technologyId, CancellationToken cancellationToken = default)
    {
        var (_, item) = await _db.QuerySingleAsync<Technology>("dbo.usp_Technology_GetById", new Dictionary<string, object?>
        {
            ["TechnologyID_Pk"] = technologyId
        }, cancellationToken).ConfigureAwait(false);
        return item;
    }

    public Task<SpExecutionResult> InsertAsync(Technology technology, int? createdBy, CancellationToken cancellationToken = default) =>
        _db.ExecuteAsync("dbo.usp_Technology_Insert", new Dictionary<string, object?>
        {
            ["TechnologyName"] = technology.TechnologyName,
            ["Category"] = technology.Category,
            ["CreatedBy"] = createdBy
        }, outIdKey: "OutTechnologyID", cancellationToken);

    public Task<SpExecutionResult> UpdateAsync(Technology technology, int? updatedBy, CancellationToken cancellationToken = default) =>
        _db.ExecuteAsync("dbo.usp_Technology_Update", new Dictionary<string, object?>
        {
            ["TechnologyID_Pk"] = technology.TechnologyId,
            ["TechnologyName"] = technology.TechnologyName,
            ["Category"] = technology.Category,
            ["RowVersion"] = technology.RowVersion,
            ["UpdatedBy"] = updatedBy
        }, cancellationToken: cancellationToken);

    public Task<SpExecutionResult> DeleteAsync(int technologyId, int? deletedBy, CancellationToken cancellationToken = default) =>
        _db.ExecuteAsync("dbo.usp_Technology_Delete", new Dictionary<string, object?>
        {
            ["TechnologyID_Pk"] = technologyId,
            ["DeletedBy"] = deletedBy
        }, cancellationToken: cancellationToken);
}
