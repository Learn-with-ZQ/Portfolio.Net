using Portfolio.Application.Common.Models;
using Portfolio.Application.DTOs.Experience;
using Portfolio.Domain.Entities.Experience;

namespace Portfolio.Application.Abstractions.Persistence;

public interface IExperienceRepository
{
    Task<Experience?> GetByIdAsync(int experienceId, CancellationToken cancellationToken = default);
    Task<SpExecutionResult> InsertAsync(Experience experience, int? createdBy, CancellationToken cancellationToken = default);
    Task<SpExecutionResult> UpdateAsync(Experience experience, int? updatedBy, CancellationToken cancellationToken = default);
    Task<SpExecutionResult> DeleteAsync(int experienceId, int? deletedBy, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<ExperienceDto>> SearchAsync(SearchExperienceRequest request, CancellationToken cancellationToken = default);
    Task<PagedResult<ExperienceDto>> GetPagedAsync(GetExperiencePagedRequest request, CancellationToken cancellationToken = default);
    Task<SpExecutionResult> InsertDetailAsync(ExperienceDetail detail, int? createdBy, CancellationToken cancellationToken = default);
    Task<SpExecutionResult> UpdateDetailAsync(ExperienceDetail detail, int? updatedBy, CancellationToken cancellationToken = default);
    Task<SpExecutionResult> DeleteDetailAsync(int experienceDetailId, int? deletedBy, CancellationToken cancellationToken = default);
}
