using Portfolio.Application.Abstractions.Persistence;
using Portfolio.Application.Common.Models;
using Portfolio.Application.DTOs.Lookups;
using Portfolio.Domain.Entities.Lookups;
using Portfolio.Persistence.Common;

namespace Portfolio.Persistence.Repositories;

public sealed class DocumentTypeRepository : IDocumentTypeRepository
{
    private readonly Database _db;

    public DocumentTypeRepository(Database db) => _db = db;

    public async Task<IReadOnlyList<LookupItemDto>> GetLookupAsync(CancellationToken cancellationToken = default)
    {
        var (_, rows) = await _db.QueryAsync<LookupItemDto>(
            "dbo.usp_DocumentType_GetLookup", new Dictionary<string, object?>(), cancellationToken).ConfigureAwait(false);
        return rows;
    }

    public async Task<PagedResult<DocumentTypeDto>> GetPagedAsync(GetLookupPagedRequest request, CancellationToken cancellationToken = default)
    {
        var (_, rows, total, pages) = await _db.QueryPagedAsync<DocumentTypeDto>("dbo.usp_DocumentType_GetPaged", new Dictionary<string, object?>
        {
            ["PageNumber"] = request.PageNumber,
            ["PageSize"] = request.PageSize,
            ["SearchTerm"] = request.SearchTerm
        }, cancellationToken).ConfigureAwait(false);
        return new PagedResult<DocumentTypeDto> { Items = rows, PageNumber = request.PageNumber, PageSize = request.PageSize, TotalRecords = total, TotalPages = pages };
    }

    public async Task<DocumentType?> GetByIdAsync(int documentTypeId, CancellationToken cancellationToken = default)
    {
        var (_, item) = await _db.QuerySingleAsync<DocumentType>("dbo.usp_DocumentType_GetById", new Dictionary<string, object?>
        {
            ["DocumentTypeID_Pk"] = documentTypeId
        }, cancellationToken).ConfigureAwait(false);
        return item;
    }

    public Task<SpExecutionResult> InsertAsync(DocumentType documentType, int? createdBy, CancellationToken cancellationToken = default) =>
        _db.ExecuteAsync("dbo.usp_DocumentType_Insert", new Dictionary<string, object?>
        {
            ["TypeName"] = documentType.TypeName,
            ["CreatedBy"] = createdBy
        }, outIdKey: "OutDocumentTypeID", cancellationToken);

    public Task<SpExecutionResult> UpdateAsync(DocumentType documentType, int? updatedBy, CancellationToken cancellationToken = default) =>
        _db.ExecuteAsync("dbo.usp_DocumentType_Update", new Dictionary<string, object?>
        {
            ["DocumentTypeID_Pk"] = documentType.DocumentTypeId,
            ["TypeName"] = documentType.TypeName,
            ["RowVersion"] = documentType.RowVersion,
            ["UpdatedBy"] = updatedBy
        }, cancellationToken: cancellationToken);

    public Task<SpExecutionResult> DeleteAsync(int documentTypeId, int? deletedBy, CancellationToken cancellationToken = default) =>
        _db.ExecuteAsync("dbo.usp_DocumentType_Delete", new Dictionary<string, object?>
        {
            ["DocumentTypeID_Pk"] = documentTypeId,
            ["DeletedBy"] = deletedBy
        }, cancellationToken: cancellationToken);
}
