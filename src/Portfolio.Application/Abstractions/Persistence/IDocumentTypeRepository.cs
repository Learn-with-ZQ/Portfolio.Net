using Portfolio.Application.Common.Models;
using Portfolio.Application.DTOs.Lookups;
using Portfolio.Domain.Entities.Lookups;

namespace Portfolio.Application.Abstractions.Persistence;

public interface IDocumentTypeRepository
{
    Task<IReadOnlyList<LookupItemDto>> GetLookupAsync(CancellationToken cancellationToken = default);
    Task<PagedResult<DocumentTypeDto>> GetPagedAsync(GetLookupPagedRequest request, CancellationToken cancellationToken = default);
    Task<DocumentType?> GetByIdAsync(int documentTypeId, CancellationToken cancellationToken = default);
    Task<SpExecutionResult> InsertAsync(DocumentType documentType, int? createdBy, CancellationToken cancellationToken = default);
    Task<SpExecutionResult> UpdateAsync(DocumentType documentType, int? updatedBy, CancellationToken cancellationToken = default);
    Task<SpExecutionResult> DeleteAsync(int documentTypeId, int? deletedBy, CancellationToken cancellationToken = default);
}
