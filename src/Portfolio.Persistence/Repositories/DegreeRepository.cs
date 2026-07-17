using Portfolio.Application.Abstractions.Persistence;
using Portfolio.Application.Common.Models;
using Portfolio.Application.DTOs.Lookups;
using Portfolio.Domain.Entities.Lookups;
using Portfolio.Persistence.Common;

namespace Portfolio.Persistence.Repositories;

public sealed class DegreeRepository : IDegreeRepository
{
    private readonly Database _db;

    public DegreeRepository(Database db) => _db = db;

    public async Task<IReadOnlyList<LookupItemDto>> GetLookupAsync(CancellationToken cancellationToken = default)
    {
        var (_, rows) = await _db.QueryAsync<LookupItemDto>(
            "dbo.usp_Degree_GetLookup", new Dictionary<string, object?>(), cancellationToken).ConfigureAwait(false);
        return rows;
    }

    public async Task<PagedResult<DegreeDto>> GetPagedAsync(GetLookupPagedRequest request, CancellationToken cancellationToken = default)
    {
        var (_, rows, total, pages) = await _db.QueryPagedAsync<DegreeDto>("dbo.usp_Degree_GetPaged", new Dictionary<string, object?>
        {
            ["PageNumber"] = request.PageNumber,
            ["PageSize"] = request.PageSize,
            ["SearchTerm"] = request.SearchTerm
        }, cancellationToken).ConfigureAwait(false);
        return new PagedResult<DegreeDto> { Items = rows, PageNumber = request.PageNumber, PageSize = request.PageSize, TotalRecords = total, TotalPages = pages };
    }

    public async Task<Degree?> GetByIdAsync(int degreeId, CancellationToken cancellationToken = default)
    {
        var (_, item) = await _db.QuerySingleAsync<Degree>("dbo.usp_Degree_GetById", new Dictionary<string, object?>
        {
            ["DegreeID_Pk"] = degreeId
        }, cancellationToken).ConfigureAwait(false);
        return item;
    }

    public Task<SpExecutionResult> InsertAsync(Degree degree, int? createdBy, CancellationToken cancellationToken = default) =>
        _db.ExecuteAsync("dbo.usp_Degree_Insert", new Dictionary<string, object?>
        {
            ["DegreeName"] = degree.DegreeName,
            ["CreatedBy"] = createdBy
        }, outIdKey: "OutDegreeID", cancellationToken);

    public Task<SpExecutionResult> UpdateAsync(Degree degree, int? updatedBy, CancellationToken cancellationToken = default) =>
        _db.ExecuteAsync("dbo.usp_Degree_Update", new Dictionary<string, object?>
        {
            ["DegreeID_Pk"] = degree.DegreeId,
            ["DegreeName"] = degree.DegreeName,
            ["RowVersion"] = degree.RowVersion,
            ["UpdatedBy"] = updatedBy
        }, cancellationToken: cancellationToken);

    public Task<SpExecutionResult> DeleteAsync(int degreeId, int? deletedBy, CancellationToken cancellationToken = default) =>
        _db.ExecuteAsync("dbo.usp_Degree_Delete", new Dictionary<string, object?>
        {
            ["DegreeID_Pk"] = degreeId,
            ["DeletedBy"] = deletedBy
        }, cancellationToken: cancellationToken);
}
