using Portfolio.Application.Common.Models;
using Portfolio.Application.Common.Results;
using Portfolio.Application.DTOs.Lookups;

namespace Portfolio.Application.Services.Interfaces;

public interface IInstituteService
{
    Task<ServiceResult<IReadOnlyList<LookupItemDto>>> GetLookupAsync(CancellationToken cancellationToken = default);
    Task<ServiceResult<PagedResult<InstituteDto>>> GetPagedAsync(GetLookupPagedRequest request, CancellationToken cancellationToken = default);
    Task<ServiceResult<InstituteDto>> GetByIdAsync(int instituteId, CancellationToken cancellationToken = default);
    Task<ServiceResult<CommandResultDto>> CreateAsync(CreateInstituteRequest request, CancellationToken cancellationToken = default);
    Task<ServiceResult<CommandResultDto>> UpdateAsync(UpdateInstituteRequest request, CancellationToken cancellationToken = default);
    Task<ServiceResult> DeleteAsync(int instituteId, CancellationToken cancellationToken = default);
}
