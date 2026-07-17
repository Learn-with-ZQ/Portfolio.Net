using Portfolio.Application.Common.Models;
using Portfolio.Application.DTOs.Projects;
using Portfolio.Domain.Entities.Projects;

namespace Portfolio.Application.Abstractions.Persistence;

public interface IProjectRepository
{
    Task<Project?> GetByIdAsync(int projectId, CancellationToken cancellationToken = default);
    Task<SpExecutionResult> InsertAsync(Project project, IEnumerable<int> technologyIds, int? createdBy, CancellationToken cancellationToken = default);
    Task<SpExecutionResult> UpdateAsync(Project project, int? updatedBy, CancellationToken cancellationToken = default);
    Task<SpExecutionResult> DeleteAsync(int projectId, int? deletedBy, CancellationToken cancellationToken = default);
    Task<SpExecutionResult> SyncTechnologiesAsync(int projectId, IEnumerable<int> technologyIds, int? updatedBy, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<ProjectDto>> SearchAsync(SearchProjectRequest request, CancellationToken cancellationToken = default);
    Task<PagedResult<ProjectDto>> GetPagedAsync(GetProjectPagedRequest request, CancellationToken cancellationToken = default);
    Task<SpExecutionResult> InsertDetailAsync(ProjectDetail detail, int? createdBy, CancellationToken cancellationToken = default);
    Task<SpExecutionResult> UpdateDetailAsync(ProjectDetail detail, int? updatedBy, CancellationToken cancellationToken = default);
    Task<SpExecutionResult> DeleteDetailAsync(int projectDetailId, int? deletedBy, CancellationToken cancellationToken = default);
}
