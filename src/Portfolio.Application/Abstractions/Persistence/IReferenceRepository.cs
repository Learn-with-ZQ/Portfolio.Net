using Portfolio.Application.Common.Models;
using Portfolio.Application.DTOs.References;
using Portfolio.Domain.Entities.References;

namespace Portfolio.Application.Abstractions.Persistence;

public interface IReferenceRepository
{
    Task<Reference?> GetByIdAsync(int referenceId, CancellationToken cancellationToken = default);
    Task<SpExecutionResult> InsertAsync(Reference reference, int? createdBy, CancellationToken cancellationToken = default);
    Task<SpExecutionResult> UpdateAsync(Reference reference, int? updatedBy, CancellationToken cancellationToken = default);
    Task<SpExecutionResult> DeleteAsync(int referenceId, int? deletedBy, CancellationToken cancellationToken = default);
    Task<PagedResult<ReferenceDto>> GetPagedAsync(GetReferencePagedRequest request, CancellationToken cancellationToken = default);
}
