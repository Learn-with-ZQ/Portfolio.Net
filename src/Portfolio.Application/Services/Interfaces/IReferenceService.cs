using Portfolio.Application.Common.Models;
using Portfolio.Application.Common.Results;
using Portfolio.Application.DTOs.References;

namespace Portfolio.Application.Services.Interfaces;

public interface IReferenceService
{
    Task<ServiceResult<ReferenceDetailDto>> GetByIdAsync(int referenceId, CancellationToken cancellationToken = default);
    Task<ServiceResult<CommandResultDto>> CreateAsync(CreateReferenceRequest request, CancellationToken cancellationToken = default);
    Task<ServiceResult<CommandResultDto>> UpdateAsync(UpdateReferenceRequest request, CancellationToken cancellationToken = default);
    Task<ServiceResult> DeleteAsync(int referenceId, CancellationToken cancellationToken = default);
    Task<ServiceResult<PagedResult<ReferenceDto>>> GetPagedAsync(GetReferencePagedRequest request, CancellationToken cancellationToken = default);
}
