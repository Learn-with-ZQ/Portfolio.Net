using Portfolio.Application.Common.Models;
using Portfolio.Application.DTOs.Awards;
using Portfolio.Domain.Entities.Awards;

namespace Portfolio.Application.Abstractions.Persistence;

public interface IAwardRepository
{
    Task<Award?> GetByIdAsync(int awardId, CancellationToken cancellationToken = default);
    Task<SpExecutionResult> InsertAsync(Award award, int? createdBy, CancellationToken cancellationToken = default);
    Task<SpExecutionResult> UpdateAsync(Award award, int? updatedBy, CancellationToken cancellationToken = default);
    Task<SpExecutionResult> DeleteAsync(int awardId, int? deletedBy, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<AwardDto>> SearchAsync(SearchAwardRequest request, CancellationToken cancellationToken = default);
    Task<PagedResult<AwardDto>> GetPagedAsync(GetAwardPagedRequest request, CancellationToken cancellationToken = default);
    Task<SpExecutionResult> InsertDetailAsync(AwardDetail detail, int? createdBy, CancellationToken cancellationToken = default);
    Task<SpExecutionResult> UpdateDetailAsync(AwardDetail detail, int? updatedBy, CancellationToken cancellationToken = default);
    Task<SpExecutionResult> DeleteDetailAsync(int awardDetailId, int? deletedBy, CancellationToken cancellationToken = default);
}
