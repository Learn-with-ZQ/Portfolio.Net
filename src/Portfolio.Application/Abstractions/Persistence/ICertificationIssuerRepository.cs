using Portfolio.Application.Common.Models;
using Portfolio.Application.DTOs.Lookups;
using Portfolio.Domain.Entities.Lookups;

namespace Portfolio.Application.Abstractions.Persistence;

public interface ICertificationIssuerRepository
{
    Task<IReadOnlyList<LookupItemDto>> GetLookupAsync(CancellationToken cancellationToken = default);
    Task<PagedResult<CertificationIssuerDto>> GetPagedAsync(GetLookupPagedRequest request, CancellationToken cancellationToken = default);
    Task<CertificationIssuer?> GetByIdAsync(int certificationIssuerId, CancellationToken cancellationToken = default);
    Task<SpExecutionResult> InsertAsync(CertificationIssuer certificationIssuer, int? createdBy, CancellationToken cancellationToken = default);
    Task<SpExecutionResult> UpdateAsync(CertificationIssuer certificationIssuer, int? updatedBy, CancellationToken cancellationToken = default);
    Task<SpExecutionResult> DeleteAsync(int certificationIssuerId, int? deletedBy, CancellationToken cancellationToken = default);
}
