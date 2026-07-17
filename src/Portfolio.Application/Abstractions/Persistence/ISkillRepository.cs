using Portfolio.Application.Common.Models;
using Portfolio.Application.DTOs.Skills;
using Portfolio.Domain.Entities.Skills;

namespace Portfolio.Application.Abstractions.Persistence;

public interface ISkillRepository
{
    Task<Skill?> GetByIdAsync(int skillId, CancellationToken cancellationToken = default);
    Task<SpExecutionResult> InsertAsync(Skill skill, int? createdBy, CancellationToken cancellationToken = default);
    Task<SpExecutionResult> UpdateAsync(Skill skill, int? updatedBy, CancellationToken cancellationToken = default);
    Task<SpExecutionResult> DeleteAsync(int skillId, int? deletedBy, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<SkillSearchResultDto>> SearchAsync(SearchSkillRequest request, CancellationToken cancellationToken = default);
    Task<PagedResult<SkillDto>> GetPagedAsync(GetSkillPagedRequest request, CancellationToken cancellationToken = default);
    Task<SpExecutionResult> InsertDetailAsync(SkillDetail detail, int? createdBy, CancellationToken cancellationToken = default);
    Task<SpExecutionResult> UpdateDetailAsync(SkillDetail detail, int? updatedBy, CancellationToken cancellationToken = default);
    Task<SpExecutionResult> DeleteDetailAsync(int skillDetailId, int? deletedBy, CancellationToken cancellationToken = default);
}
