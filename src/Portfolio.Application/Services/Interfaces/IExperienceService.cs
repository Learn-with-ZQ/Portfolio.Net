using Portfolio.Application.Common.Models;
using Portfolio.Application.Common.Results;
using Portfolio.Application.DTOs.Experience;

namespace Portfolio.Application.Services.Interfaces;

public interface IExperienceService
{
    Task<ServiceResult<ExperienceDetailDto>> GetByIdAsync(int experienceId, CancellationToken cancellationToken = default);
    Task<ServiceResult<CommandResultDto>> CreateAsync(CreateExperienceRequest request, CancellationToken cancellationToken = default);
    Task<ServiceResult<CommandResultDto>> UpdateAsync(UpdateExperienceRequest request, CancellationToken cancellationToken = default);
    Task<ServiceResult> DeleteAsync(int experienceId, CancellationToken cancellationToken = default);
    Task<ServiceResult<IReadOnlyList<ExperienceDto>>> SearchAsync(SearchExperienceRequest request, CancellationToken cancellationToken = default);
    Task<ServiceResult<PagedResult<ExperienceDto>>> GetPagedAsync(GetExperiencePagedRequest request, CancellationToken cancellationToken = default);
    Task<ServiceResult<CommandResultDto>> CreateDetailAsync(CreateExperienceDetailRequest request, CancellationToken cancellationToken = default);
    Task<ServiceResult<CommandResultDto>> UpdateDetailAsync(UpdateExperienceDetailRequest request, CancellationToken cancellationToken = default);
    Task<ServiceResult> DeleteDetailAsync(int experienceDetailId, CancellationToken cancellationToken = default);
}
