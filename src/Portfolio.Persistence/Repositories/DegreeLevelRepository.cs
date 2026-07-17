using Portfolio.Application.Abstractions.Persistence;
using Portfolio.Application.Common.Models;
using Portfolio.Application.DTOs.Lookups;
using Portfolio.Domain.Entities.Lookups;
using Portfolio.Persistence.Common;

namespace Portfolio.Persistence.Repositories;

public sealed class DegreeLevelRepository : IDegreeLevelRepository
{
    private readonly Database _db;

    public DegreeLevelRepository(Database db) => _db = db;

    public async Task<IReadOnlyList<LookupItemDto>> GetLookupAsync(CancellationToken cancellationToken = default)
    {
        var (_, rows) = await _db.QueryAsync<LookupItemDto>(
            "dbo.usp_DegreeLevel_GetLookup", new Dictionary<string, object?>(), cancellationToken).ConfigureAwait(false);
        return rows;
    }

    public async Task<PagedResult<DegreeLevelDto>> GetPagedAsync(GetLookupPagedRequest request, CancellationToken cancellationToken = default)
    {
        var (_, rows, total, pages) = await _db.QueryPagedAsync<DegreeLevelDto>("dbo.usp_DegreeLevel_GetPaged", new Dictionary<string, object?>
        {
            ["PageNumber"] = request.PageNumber,
            ["PageSize"] = request.PageSize,
            ["SearchTerm"] = request.SearchTerm
        }, cancellationToken).ConfigureAwait(false);
        return new PagedResult<DegreeLevelDto> { Items = rows, PageNumber = request.PageNumber, PageSize = request.PageSize, TotalRecords = total, TotalPages = pages };
    }

    public async Task<DegreeLevel?> GetByIdAsync(int degreeLevelId, CancellationToken cancellationToken = default)
    {
        var (_, item) = await _db.QuerySingleAsync<DegreeLevel>("dbo.usp_DegreeLevel_GetById", new Dictionary<string, object?>
        {
            ["DegreeLevelID_Pk"] = degreeLevelId
        }, cancellationToken).ConfigureAwait(false);
        return item;
    }

    public Task<SpExecutionResult> InsertAsync(DegreeLevel degreeLevel, int? createdBy, CancellationToken cancellationToken = default) =>
        _db.ExecuteAsync("dbo.usp_DegreeLevel_Insert", new Dictionary<string, object?>
        {
            ["DegreeLevelName"] = degreeLevel.DegreeLevelName,
            ["DegreePrefix"] = degreeLevel.DegreePrefix,
            ["SortOrder"] = degreeLevel.SortOrder,
            ["CreatedBy"] = createdBy
        }, outIdKey: "OutDegreeLevelID", cancellationToken);

    public Task<SpExecutionResult> UpdateAsync(DegreeLevel degreeLevel, int? updatedBy, CancellationToken cancellationToken = default) =>
        _db.ExecuteAsync("dbo.usp_DegreeLevel_Update", new Dictionary<string, object?>
        {
            ["DegreeLevelID_Pk"] = degreeLevel.DegreeLevelId,
            ["DegreeLevelName"] = degreeLevel.DegreeLevelName,
            ["DegreePrefix"] = degreeLevel.DegreePrefix,
            ["SortOrder"] = degreeLevel.SortOrder,
            ["RowVersion"] = degreeLevel.RowVersion,
            ["UpdatedBy"] = updatedBy
        }, cancellationToken: cancellationToken);

    public Task<SpExecutionResult> DeleteAsync(int degreeLevelId, int? deletedBy, CancellationToken cancellationToken = default) =>
        _db.ExecuteAsync("dbo.usp_DegreeLevel_Delete", new Dictionary<string, object?>
        {
            ["DegreeLevelID_Pk"] = degreeLevelId,
            ["DeletedBy"] = deletedBy
        }, cancellationToken: cancellationToken);
}
