using Portfolio.Application.Abstractions.Persistence;
using Portfolio.Application.Common.Models;
using Portfolio.Application.DTOs.Lookups;
using Portfolio.Domain.Entities.Lookups;
using Portfolio.Persistence.Common;

namespace Portfolio.Persistence.Repositories;

public sealed class CompanyRepository : ICompanyRepository
{
    private readonly Database _db;

    public CompanyRepository(Database db) => _db = db;

    public async Task<IReadOnlyList<LookupItemDto>> GetLookupAsync(CancellationToken cancellationToken = default)
    {
        var (_, rows) = await _db.QueryAsync<LookupItemDto>(
            "dbo.usp_Company_GetLookup", new Dictionary<string, object?>(), cancellationToken).ConfigureAwait(false);
        return rows;
    }

    public async Task<PagedResult<CompanyDto>> GetPagedAsync(GetLookupPagedRequest request, CancellationToken cancellationToken = default)
    {
        var (_, rows, total, pages) = await _db.QueryPagedAsync<CompanyDto>("dbo.usp_Company_GetPaged", new Dictionary<string, object?>
        {
            ["PageNumber"] = request.PageNumber,
            ["PageSize"] = request.PageSize,
            ["SearchTerm"] = request.SearchTerm
        }, cancellationToken).ConfigureAwait(false);
        return new PagedResult<CompanyDto> { Items = rows, PageNumber = request.PageNumber, PageSize = request.PageSize, TotalRecords = total, TotalPages = pages };
    }

    public async Task<Company?> GetByIdAsync(int companyId, CancellationToken cancellationToken = default)
    {
        var (_, item) = await _db.QuerySingleAsync<Company>("dbo.usp_Company_GetById", new Dictionary<string, object?>
        {
            ["CompanyID_Pk"] = companyId
        }, cancellationToken).ConfigureAwait(false);
        return item;
    }

    public Task<SpExecutionResult> InsertAsync(Company company, int? createdBy, CancellationToken cancellationToken = default) =>
        _db.ExecuteAsync("dbo.usp_Company_Insert", new Dictionary<string, object?>
        {
            ["CompanyName"] = company.CompanyName,
            ["WebsiteUrl"] = company.WebsiteUrl,
            ["CreatedBy"] = createdBy
        }, outIdKey: "OutCompanyID", cancellationToken);

    public Task<SpExecutionResult> UpdateAsync(Company company, int? updatedBy, CancellationToken cancellationToken = default) =>
        _db.ExecuteAsync("dbo.usp_Company_Update", new Dictionary<string, object?>
        {
            ["CompanyID_Pk"] = company.CompanyId,
            ["CompanyName"] = company.CompanyName,
            ["WebsiteUrl"] = company.WebsiteUrl,
            ["RowVersion"] = company.RowVersion,
            ["UpdatedBy"] = updatedBy
        }, cancellationToken: cancellationToken);

    public Task<SpExecutionResult> DeleteAsync(int companyId, int? deletedBy, CancellationToken cancellationToken = default) =>
        _db.ExecuteAsync("dbo.usp_Company_Delete", new Dictionary<string, object?>
        {
            ["CompanyID_Pk"] = companyId,
            ["DeletedBy"] = deletedBy
        }, cancellationToken: cancellationToken);
}

public sealed class DeployDetailRepository : IDeployDetailRepository
{
    private readonly Database _db;

    public DeployDetailRepository(Database db) => _db = db;

    public async Task<IReadOnlyList<LookupItemDto>> GetLookupAsync(CancellationToken cancellationToken = default)
    {
        var (_, rows) = await _db.QueryAsync<LookupItemDto>(
            "dbo.usp_DeployDetails_GetLookup", new Dictionary<string, object?>(), cancellationToken).ConfigureAwait(false);
        return rows;
    }

    public async Task<PagedResult<DeployDetailDto>> GetPagedAsync(GetLookupPagedRequest request, CancellationToken cancellationToken = default)
    {
        var (_, rows, total, pages) = await _db.QueryPagedAsync<DeployDetailDto>("dbo.usp_DeployDetails_GetPaged", new Dictionary<string, object?>
        {
            ["PageNumber"] = request.PageNumber,
            ["PageSize"] = request.PageSize,
            ["SearchTerm"] = request.SearchTerm
        }, cancellationToken).ConfigureAwait(false);
        return new PagedResult<DeployDetailDto> { Items = rows, PageNumber = request.PageNumber, PageSize = request.PageSize, TotalRecords = total, TotalPages = pages };
    }

    public async Task<DeployDetail?> GetByIdAsync(int deployDetailId, CancellationToken cancellationToken = default)
    {
        var (_, item) = await _db.QuerySingleAsync<DeployDetail>("dbo.usp_DeployDetails_GetById", new Dictionary<string, object?>
        {
            ["DeployDetailID_Pk"] = deployDetailId
        }, cancellationToken).ConfigureAwait(false);
        return item;
    }

    public Task<SpExecutionResult> InsertAsync(DeployDetail deployDetail, int? createdBy, CancellationToken cancellationToken = default) =>
        _db.ExecuteAsync("dbo.usp_DeployDetails_Insert", new Dictionary<string, object?>
        {
            ["DeployDetailName"] = deployDetail.DeployDetailName,
            ["DeployedTo"] = deployDetail.DeployedTo,
            ["DeployedByCompanyID_Fk"] = deployDetail.DeployedByCompanyId,
            ["CreatedBy"] = createdBy
        }, outIdKey: "OutDeployDetailID", cancellationToken);

    public Task<SpExecutionResult> UpdateAsync(DeployDetail deployDetail, int? updatedBy, CancellationToken cancellationToken = default) =>
        _db.ExecuteAsync("dbo.usp_DeployDetails_Update", new Dictionary<string, object?>
        {
            ["DeployDetailID_Pk"] = deployDetail.DeployDetailId,
            ["DeployDetailName"] = deployDetail.DeployDetailName,
            ["DeployedTo"] = deployDetail.DeployedTo,
            ["DeployedByCompanyID_Fk"] = deployDetail.DeployedByCompanyId,
            ["RowVersion"] = deployDetail.RowVersion,
            ["UpdatedBy"] = updatedBy
        }, cancellationToken: cancellationToken);

    public Task<SpExecutionResult> DeleteAsync(int deployDetailId, int? deletedBy, CancellationToken cancellationToken = default) =>
        _db.ExecuteAsync("dbo.usp_DeployDetails_Delete", new Dictionary<string, object?>
        {
            ["DeployDetailID_Pk"] = deployDetailId,
            ["DeletedBy"] = deletedBy
        }, cancellationToken: cancellationToken);
}
