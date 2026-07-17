using Portfolio.Application.Common.Models;
using Portfolio.Application.Common.Results;
using Portfolio.Application.DTOs.Lookups;

namespace Portfolio.Application.Services.Interfaces;

public interface IDocumentTypeService
{
    Task<ServiceResult<IReadOnlyList<LookupItemDto>>> GetLookupAsync(CancellationToken cancellationToken = default);
    Task<ServiceResult<PagedResult<DocumentTypeDto>>> GetPagedAsync(GetLookupPagedRequest request, CancellationToken cancellationToken = default);
    Task<ServiceResult<DocumentTypeDto>> GetByIdAsync(int documentTypeId, CancellationToken cancellationToken = default);
    Task<ServiceResult<CommandResultDto>> CreateAsync(CreateDocumentTypeRequest request, CancellationToken cancellationToken = default);
    Task<ServiceResult<CommandResultDto>> UpdateAsync(UpdateDocumentTypeRequest request, CancellationToken cancellationToken = default);
    Task<ServiceResult> DeleteAsync(int documentTypeId, CancellationToken cancellationToken = default);
}
