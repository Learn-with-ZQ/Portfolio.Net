using Portfolio.Application.Common.Models;
using Portfolio.Application.Common.Results;
using Portfolio.Application.DTOs.Lookups;

namespace Portfolio.Application.Services.Interfaces;

public interface IDegreeLevelService
{
    Task<ServiceResult<IReadOnlyList<LookupItemDto>>> GetLookupAsync(CancellationToken cancellationToken = default);
    Task<ServiceResult<PagedResult<DegreeLevelDto>>> GetPagedAsync(GetLookupPagedRequest request, CancellationToken cancellationToken = default);
    Task<ServiceResult<DegreeLevelDto>> GetByIdAsync(int degreeLevelId, CancellationToken cancellationToken = default);
    Task<ServiceResult<CommandResultDto>> CreateAsync(CreateDegreeLevelRequest request, CancellationToken cancellationToken = default);
    Task<ServiceResult<CommandResultDto>> UpdateAsync(UpdateDegreeLevelRequest request, CancellationToken cancellationToken = default);
    Task<ServiceResult> DeleteAsync(int degreeLevelId, CancellationToken cancellationToken = default);
}
