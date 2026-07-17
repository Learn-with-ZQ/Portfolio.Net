using Portfolio.Application.Common.Models;
using Portfolio.Application.DTOs.Lookups;
using Portfolio.Domain.Entities.Lookups;

namespace Portfolio.Application.Abstractions.Persistence;

public interface IDegreeLevelRepository
{
    Task<IReadOnlyList<LookupItemDto>> GetLookupAsync(CancellationToken cancellationToken = default);
    Task<PagedResult<DegreeLevelDto>> GetPagedAsync(GetLookupPagedRequest request, CancellationToken cancellationToken = default);
    Task<DegreeLevel?> GetByIdAsync(int degreeLevelId, CancellationToken cancellationToken = default);
    Task<SpExecutionResult> InsertAsync(DegreeLevel degreeLevel, int? createdBy, CancellationToken cancellationToken = default);
    Task<SpExecutionResult> UpdateAsync(DegreeLevel degreeLevel, int? updatedBy, CancellationToken cancellationToken = default);
    Task<SpExecutionResult> DeleteAsync(int degreeLevelId, int? deletedBy, CancellationToken cancellationToken = default);
}
