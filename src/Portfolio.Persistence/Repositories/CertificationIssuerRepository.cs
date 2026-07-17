using Portfolio.Application.Abstractions.Persistence;
using Portfolio.Application.Common.Models;
using Portfolio.Application.DTOs.Lookups;
using Portfolio.Domain.Entities.Lookups;
using Portfolio.Persistence.Common;

namespace Portfolio.Persistence.Repositories;

public sealed class CertificationIssuerRepository : ICertificationIssuerRepository
{
    private readonly Database _db;

    public CertificationIssuerRepository(Database db) => _db = db;

    public async Task<IReadOnlyList<LookupItemDto>> GetLookupAsync(CancellationToken cancellationToken = default)
    {
        var (_, rows) = await _db.QueryAsync<LookupItemDto>(
            "dbo.usp_CertificationIssuer_GetLookup", new Dictionary<string, object?>(), cancellationToken).ConfigureAwait(false);
        return rows;
    }

    public async Task<PagedResult<CertificationIssuerDto>> GetPagedAsync(GetLookupPagedRequest request, CancellationToken cancellationToken = default)
    {
        var (_, rows, total, pages) = await _db.QueryPagedAsync<CertificationIssuerDto>("dbo.usp_CertificationIssuer_GetPaged", new Dictionary<string, object?>
        {
            ["PageNumber"] = request.PageNumber,
            ["PageSize"] = request.PageSize,
            ["SearchTerm"] = request.SearchTerm
        }, cancellationToken).ConfigureAwait(false);
        return new PagedResult<CertificationIssuerDto> { Items = rows, PageNumber = request.PageNumber, PageSize = request.PageSize, TotalRecords = total, TotalPages = pages };
    }

    public async Task<CertificationIssuer?> GetByIdAsync(int certificationIssuerId, CancellationToken cancellationToken = default)
    {
        var (_, item) = await _db.QuerySingleAsync<CertificationIssuer>("dbo.usp_CertificationIssuer_GetById", new Dictionary<string, object?>
        {
            ["CertificationIssuerID_Pk"] = certificationIssuerId
        }, cancellationToken).ConfigureAwait(false);
        return item;
    }

    public Task<SpExecutionResult> InsertAsync(CertificationIssuer certificationIssuer, int? createdBy, CancellationToken cancellationToken = default) =>
        _db.ExecuteAsync("dbo.usp_CertificationIssuer_Insert", new Dictionary<string, object?>
        {
            ["IssuerName"] = certificationIssuer.IssuerName,
            ["IssuerWebsite"] = certificationIssuer.IssuerWebsite,
            ["CreatedBy"] = createdBy
        }, outIdKey: "OutCertificationIssuerID", cancellationToken);

    public Task<SpExecutionResult> UpdateAsync(CertificationIssuer certificationIssuer, int? updatedBy, CancellationToken cancellationToken = default) =>
        _db.ExecuteAsync("dbo.usp_CertificationIssuer_Update", new Dictionary<string, object?>
        {
            ["CertificationIssuerID_Pk"] = certificationIssuer.CertificationIssuerId,
            ["IssuerName"] = certificationIssuer.IssuerName,
            ["IssuerWebsite"] = certificationIssuer.IssuerWebsite,
            ["RowVersion"] = certificationIssuer.RowVersion,
            ["UpdatedBy"] = updatedBy
        }, cancellationToken: cancellationToken);

    public Task<SpExecutionResult> DeleteAsync(int certificationIssuerId, int? deletedBy, CancellationToken cancellationToken = default) =>
        _db.ExecuteAsync("dbo.usp_CertificationIssuer_Delete", new Dictionary<string, object?>
        {
            ["CertificationIssuerID_Pk"] = certificationIssuerId,
            ["DeletedBy"] = deletedBy
        }, cancellationToken: cancellationToken);
}
