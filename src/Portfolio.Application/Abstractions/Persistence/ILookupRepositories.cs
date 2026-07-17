using Portfolio.Application.Common.Models;
using Portfolio.Application.DTOs.Lookups;
using Portfolio.Domain.Entities.Lookups;

namespace Portfolio.Application.Abstractions.Persistence;

public interface ICompanyRepository
{
    Task<IReadOnlyList<LookupItemDto>> GetLookupAsync(CancellationToken cancellationToken = default);
    Task<PagedResult<CompanyDto>> GetPagedAsync(GetLookupPagedRequest request, CancellationToken cancellationToken = default);
    Task<Company?> GetByIdAsync(int companyId, CancellationToken cancellationToken = default);
    Task<SpExecutionResult> InsertAsync(Company company, int? createdBy, CancellationToken cancellationToken = default);
    Task<SpExecutionResult> UpdateAsync(Company company, int? updatedBy, CancellationToken cancellationToken = default);
    Task<SpExecutionResult> DeleteAsync(int companyId, int? deletedBy, CancellationToken cancellationToken = default);
}

public interface IDeployDetailRepository
{
    Task<IReadOnlyList<LookupItemDto>> GetLookupAsync(CancellationToken cancellationToken = default);
    Task<PagedResult<DeployDetailDto>> GetPagedAsync(GetLookupPagedRequest request, CancellationToken cancellationToken = default);
    Task<DeployDetail?> GetByIdAsync(int deployDetailId, CancellationToken cancellationToken = default);
    Task<SpExecutionResult> InsertAsync(DeployDetail deployDetail, int? createdBy, CancellationToken cancellationToken = default);
    Task<SpExecutionResult> UpdateAsync(DeployDetail deployDetail, int? updatedBy, CancellationToken cancellationToken = default);
    Task<SpExecutionResult> DeleteAsync(int deployDetailId, int? deletedBy, CancellationToken cancellationToken = default);
}
