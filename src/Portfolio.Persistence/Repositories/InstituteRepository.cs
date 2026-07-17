using Portfolio.Application.Abstractions.Persistence;
using Portfolio.Application.Common.Models;
using Portfolio.Application.DTOs.Lookups;
using Portfolio.Domain.Entities.Lookups;
using Portfolio.Persistence.Common;

namespace Portfolio.Persistence.Repositories;

public sealed class InstituteRepository : IInstituteRepository
{
    private readonly Database _db;

    public InstituteRepository(Database db) => _db = db;

    public async Task<IReadOnlyList<LookupItemDto>> GetLookupAsync(CancellationToken cancellationToken = default)
    {
        var (_, rows) = await _db.QueryAsync<LookupItemDto>(
            "dbo.usp_Institute_GetLookup", new Dictionary<string, object?>(), cancellationToken).ConfigureAwait(false);
        return rows;
    }

    public async Task<PagedResult<InstituteDto>> GetPagedAsync(GetLookupPagedRequest request, CancellationToken cancellationToken = default)
    {
        var (_, rows, total, pages) = await _db.QueryPagedAsync<InstituteDto>("dbo.usp_Institute_GetPaged", new Dictionary<string, object?>
        {
            ["PageNumber"] = request.PageNumber,
            ["PageSize"] = request.PageSize,
            ["SearchTerm"] = request.SearchTerm
        }, cancellationToken).ConfigureAwait(false);
        return new PagedResult<InstituteDto> { Items = rows, PageNumber = request.PageNumber, PageSize = request.PageSize, TotalRecords = total, TotalPages = pages };
    }

    public async Task<Institute?> GetByIdAsync(int instituteId, CancellationToken cancellationToken = default)
    {
        var (_, item) = await _db.QuerySingleAsync<Institute>("dbo.usp_Institute_GetById", new Dictionary<string, object?>
        {
            ["InstituteID_Pk"] = instituteId
        }, cancellationToken).ConfigureAwait(false);
        return item;
    }

    public Task<SpExecutionResult> InsertAsync(Institute institute, int? createdBy, CancellationToken cancellationToken = default) =>
        _db.ExecuteAsync("dbo.usp_Institute_Insert", new Dictionary<string, object?>
        {
            ["InstituteName"] = institute.InstituteName,
            ["Location"] = institute.Location,
            ["CreatedBy"] = createdBy
        }, outIdKey: "OutInstituteID", cancellationToken);

    public Task<SpExecutionResult> UpdateAsync(Institute institute, int? updatedBy, CancellationToken cancellationToken = default) =>
        _db.ExecuteAsync("dbo.usp_Institute_Update", new Dictionary<string, object?>
        {
            ["InstituteID_Pk"] = institute.InstituteId,
            ["InstituteName"] = institute.InstituteName,
            ["Location"] = institute.Location,
            ["RowVersion"] = institute.RowVersion,
            ["UpdatedBy"] = updatedBy
        }, cancellationToken: cancellationToken);

    public Task<SpExecutionResult> DeleteAsync(int instituteId, int? deletedBy, CancellationToken cancellationToken = default) =>
        _db.ExecuteAsync("dbo.usp_Institute_Delete", new Dictionary<string, object?>
        {
            ["InstituteID_Pk"] = instituteId,
            ["DeletedBy"] = deletedBy
        }, cancellationToken: cancellationToken);
}
