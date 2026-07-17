using Portfolio.Application.Common.Models;
using Portfolio.Application.DTOs.Lookups;
using Portfolio.Domain.Entities.Lookups;

namespace Portfolio.Application.Abstractions.Persistence;

public interface IInstituteRepository
{
    Task<IReadOnlyList<LookupItemDto>> GetLookupAsync(CancellationToken cancellationToken = default);
    Task<PagedResult<InstituteDto>> GetPagedAsync(GetLookupPagedRequest request, CancellationToken cancellationToken = default);
    Task<Institute?> GetByIdAsync(int instituteId, CancellationToken cancellationToken = default);
    Task<SpExecutionResult> InsertAsync(Institute institute, int? createdBy, CancellationToken cancellationToken = default);
    Task<SpExecutionResult> UpdateAsync(Institute institute, int? updatedBy, CancellationToken cancellationToken = default);
    Task<SpExecutionResult> DeleteAsync(int instituteId, int? deletedBy, CancellationToken cancellationToken = default);
}
