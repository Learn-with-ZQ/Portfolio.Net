using Portfolio.Application.Common.Models;
using Portfolio.Application.Common.Results;
using Portfolio.Application.DTOs.Lookups;

namespace Portfolio.Application.Services.Interfaces;

public interface ICertificationIssuerService
{
    Task<ServiceResult<IReadOnlyList<LookupItemDto>>> GetLookupAsync(CancellationToken cancellationToken = default);
    Task<ServiceResult<PagedResult<CertificationIssuerDto>>> GetPagedAsync(GetLookupPagedRequest request, CancellationToken cancellationToken = default);
    Task<ServiceResult<CertificationIssuerDto>> GetByIdAsync(int certificationIssuerId, CancellationToken cancellationToken = default);
    Task<ServiceResult<CommandResultDto>> CreateAsync(CreateCertificationIssuerRequest request, CancellationToken cancellationToken = default);
    Task<ServiceResult<CommandResultDto>> UpdateAsync(UpdateCertificationIssuerRequest request, CancellationToken cancellationToken = default);
    Task<ServiceResult> DeleteAsync(int certificationIssuerId, CancellationToken cancellationToken = default);
}
