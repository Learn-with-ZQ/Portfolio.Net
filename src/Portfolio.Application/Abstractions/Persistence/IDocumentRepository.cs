using Portfolio.Application.Common.Models;
using Portfolio.Application.DTOs.Documents;
using Portfolio.Domain.Entities.Documents;

namespace Portfolio.Application.Abstractions.Persistence;

public interface IDocumentRepository
{
    Task<Document?> GetByIdAsync(int documentId, CancellationToken cancellationToken = default);
    Task<SpExecutionResult> InsertAsync(Document document, int? createdBy, CancellationToken cancellationToken = default);
    Task<SpExecutionResult> UpdateAsync(Document document, int? updatedBy, CancellationToken cancellationToken = default);
    Task<SpExecutionResult> DeleteAsync(int documentId, int? deletedBy, CancellationToken cancellationToken = default);
    Task<SpExecutionResult> CreateNewVersionAsync(int sourceDocumentId, Document document, int? createdBy, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<DocumentDto>> SearchAsync(SearchDocumentRequest request, CancellationToken cancellationToken = default);
    Task<PagedResult<DocumentDto>> GetPagedAsync(GetDocumentPagedRequest request, CancellationToken cancellationToken = default);
}
