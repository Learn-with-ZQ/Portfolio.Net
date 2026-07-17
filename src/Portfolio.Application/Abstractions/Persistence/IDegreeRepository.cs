using Portfolio.Application.Common.Models;
using Portfolio.Application.DTOs.Lookups;
using Portfolio.Domain.Entities.Lookups;

namespace Portfolio.Application.Abstractions.Persistence;

public interface IDegreeRepository
{
    Task<IReadOnlyList<LookupItemDto>> GetLookupAsync(CancellationToken cancellationToken = default);
    Task<PagedResult<DegreeDto>> GetPagedAsync(GetLookupPagedRequest request, CancellationToken cancellationToken = default);
    Task<Degree?> GetByIdAsync(int degreeId, CancellationToken cancellationToken = default);
    Task<SpExecutionResult> InsertAsync(Degree degree, int? createdBy, CancellationToken cancellationToken = default);
    Task<SpExecutionResult> UpdateAsync(Degree degree, int? updatedBy, CancellationToken cancellationToken = default);
    Task<SpExecutionResult> DeleteAsync(int degreeId, int? deletedBy, CancellationToken cancellationToken = default);
}
