using Portfolio.Application.Common.Models;
using Portfolio.Application.DTOs.Certifications;
using Portfolio.Domain.Entities.Certifications;

namespace Portfolio.Application.Abstractions.Persistence;

public interface ICertificationRepository
{
    Task<Certification?> GetByIdAsync(int certificationId, CancellationToken cancellationToken = default);
    Task<SpExecutionResult> InsertAsync(Certification certification, int? createdBy, CancellationToken cancellationToken = default);
    Task<SpExecutionResult> UpdateAsync(Certification certification, int? updatedBy, CancellationToken cancellationToken = default);
    Task<SpExecutionResult> DeleteAsync(int certificationId, int? deletedBy, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<CertificationDto>> SearchAsync(SearchCertificationRequest request, CancellationToken cancellationToken = default);
    Task<PagedResult<CertificationDto>> GetPagedAsync(GetCertificationPagedRequest request, CancellationToken cancellationToken = default);
    Task<SpExecutionResult> InsertDetailAsync(CertificationDetail detail, int? createdBy, CancellationToken cancellationToken = default);
    Task<SpExecutionResult> UpdateDetailAsync(CertificationDetail detail, int? updatedBy, CancellationToken cancellationToken = default);
    Task<SpExecutionResult> DeleteDetailAsync(int certificationDetailId, int? deletedBy, CancellationToken cancellationToken = default);
}
