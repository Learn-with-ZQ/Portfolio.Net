using Portfolio.Application.Common.Models;
using Portfolio.Application.Common.Results;
using Portfolio.Application.DTOs.Lookups;

namespace Portfolio.Application.Services.Interfaces;

public interface IDegreeService
{
    Task<ServiceResult<IReadOnlyList<LookupItemDto>>> GetLookupAsync(CancellationToken cancellationToken = default);
    Task<ServiceResult<PagedResult<DegreeDto>>> GetPagedAsync(GetLookupPagedRequest request, CancellationToken cancellationToken = default);
    Task<ServiceResult<DegreeDto>> GetByIdAsync(int degreeId, CancellationToken cancellationToken = default);
    Task<ServiceResult<CommandResultDto>> CreateAsync(CreateDegreeRequest request, CancellationToken cancellationToken = default);
    Task<ServiceResult<CommandResultDto>> UpdateAsync(UpdateDegreeRequest request, CancellationToken cancellationToken = default);
    Task<ServiceResult> DeleteAsync(int degreeId, CancellationToken cancellationToken = default);
}
