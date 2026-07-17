using Portfolio.Application.Common.Models;
using Portfolio.Application.DTOs.Education;
using Portfolio.Domain.Entities.Education;

namespace Portfolio.Application.Abstractions.Persistence;

public interface IEducationRepository
{
    Task<EducationRecord?> GetByIdAsync(int educationId, CancellationToken cancellationToken = default);
    Task<SpExecutionResult> InsertAsync(EducationRecord education, int? createdBy, CancellationToken cancellationToken = default);
    Task<SpExecutionResult> UpdateAsync(EducationRecord education, int? updatedBy, CancellationToken cancellationToken = default);
    Task<SpExecutionResult> DeleteAsync(int educationId, int? deletedBy, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<EducationDto>> SearchAsync(SearchEducationRequest request, CancellationToken cancellationToken = default);
    Task<PagedResult<EducationDto>> GetPagedAsync(GetEducationPagedRequest request, CancellationToken cancellationToken = default);
}
