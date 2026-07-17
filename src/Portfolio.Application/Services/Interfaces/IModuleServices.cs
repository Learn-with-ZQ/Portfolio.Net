using Portfolio.Application.Common.Models;
using Portfolio.Application.Common.Results;
using Portfolio.Application.DTOs.Awards;
using Portfolio.Application.DTOs.Certifications;
using Portfolio.Application.DTOs.Documents;
using Portfolio.Application.DTOs.Education;
using Portfolio.Application.DTOs.Projects;
using Portfolio.Application.DTOs.Skills;

namespace Portfolio.Application.Services.Interfaces;

public interface IProjectService
{
    Task<ServiceResult<ProjectDetailDto>> GetByIdAsync(int projectId, CancellationToken cancellationToken = default);
    Task<ServiceResult<CommandResultDto>> CreateAsync(CreateProjectRequest request, CancellationToken cancellationToken = default);
    Task<ServiceResult<CommandResultDto>> UpdateAsync(UpdateProjectRequest request, CancellationToken cancellationToken = default);
    Task<ServiceResult> DeleteAsync(int projectId, CancellationToken cancellationToken = default);
    Task<ServiceResult> SyncTechnologiesAsync(SyncProjectTechnologiesRequest request, CancellationToken cancellationToken = default);
    Task<ServiceResult<IReadOnlyList<ProjectDto>>> SearchAsync(SearchProjectRequest request, CancellationToken cancellationToken = default);
    Task<ServiceResult<PagedResult<ProjectDto>>> GetPagedAsync(GetProjectPagedRequest request, CancellationToken cancellationToken = default);
}

public interface IEducationService
{
    Task<ServiceResult<EducationDetailDto>> GetByIdAsync(int educationId, CancellationToken cancellationToken = default);
    Task<ServiceResult<CommandResultDto>> CreateAsync(CreateEducationRequest request, CancellationToken cancellationToken = default);
    Task<ServiceResult<CommandResultDto>> UpdateAsync(UpdateEducationRequest request, CancellationToken cancellationToken = default);
    Task<ServiceResult> DeleteAsync(int educationId, CancellationToken cancellationToken = default);
    Task<ServiceResult<IReadOnlyList<EducationDto>>> SearchAsync(SearchEducationRequest request, CancellationToken cancellationToken = default);
    Task<ServiceResult<PagedResult<EducationDto>>> GetPagedAsync(GetEducationPagedRequest request, CancellationToken cancellationToken = default);
}

public interface ISkillService
{
    Task<ServiceResult<SkillDetailDto>> GetByIdAsync(int skillId, CancellationToken cancellationToken = default);
    Task<ServiceResult<CommandResultDto>> CreateAsync(CreateSkillRequest request, CancellationToken cancellationToken = default);
    Task<ServiceResult<CommandResultDto>> UpdateAsync(UpdateSkillRequest request, CancellationToken cancellationToken = default);
    Task<ServiceResult> DeleteAsync(int skillId, CancellationToken cancellationToken = default);
    Task<ServiceResult<IReadOnlyList<SkillSearchResultDto>>> SearchAsync(SearchSkillRequest request, CancellationToken cancellationToken = default);
    Task<ServiceResult<PagedResult<SkillDto>>> GetPagedAsync(GetSkillPagedRequest request, CancellationToken cancellationToken = default);
}

public interface IAwardService
{
    Task<ServiceResult<AwardDetailDto>> GetByIdAsync(int awardId, CancellationToken cancellationToken = default);
    Task<ServiceResult<CommandResultDto>> CreateAsync(CreateAwardRequest request, CancellationToken cancellationToken = default);
    Task<ServiceResult<CommandResultDto>> UpdateAsync(UpdateAwardRequest request, CancellationToken cancellationToken = default);
    Task<ServiceResult> DeleteAsync(int awardId, CancellationToken cancellationToken = default);
    Task<ServiceResult<IReadOnlyList<AwardDto>>> SearchAsync(SearchAwardRequest request, CancellationToken cancellationToken = default);
    Task<ServiceResult<PagedResult<AwardDto>>> GetPagedAsync(GetAwardPagedRequest request, CancellationToken cancellationToken = default);
}

public interface ICertificationService
{
    Task<ServiceResult<CertificationDetailDto>> GetByIdAsync(int certificationId, CancellationToken cancellationToken = default);
    Task<ServiceResult<CommandResultDto>> CreateAsync(CreateCertificationRequest request, CancellationToken cancellationToken = default);
    Task<ServiceResult<CommandResultDto>> UpdateAsync(UpdateCertificationRequest request, CancellationToken cancellationToken = default);
    Task<ServiceResult> DeleteAsync(int certificationId, CancellationToken cancellationToken = default);
    Task<ServiceResult<IReadOnlyList<CertificationDto>>> SearchAsync(SearchCertificationRequest request, CancellationToken cancellationToken = default);
    Task<ServiceResult<PagedResult<CertificationDto>>> GetPagedAsync(GetCertificationPagedRequest request, CancellationToken cancellationToken = default);
}

public interface IDocumentService
{
    Task<ServiceResult<DocumentDetailDto>> GetByIdAsync(int documentId, CancellationToken cancellationToken = default);
    Task<ServiceResult<CommandResultDto>> CreateAsync(CreateDocumentRequest request, CancellationToken cancellationToken = default);
    Task<ServiceResult<CommandResultDto>> UpdateAsync(UpdateDocumentRequest request, CancellationToken cancellationToken = default);
    Task<ServiceResult> DeleteAsync(int documentId, CancellationToken cancellationToken = default);
    Task<ServiceResult<CommandResultDto>> CreateVersionAsync(CreateDocumentVersionRequest request, CancellationToken cancellationToken = default);
    Task<ServiceResult<IReadOnlyList<DocumentDto>>> SearchAsync(SearchDocumentRequest request, CancellationToken cancellationToken = default);
    Task<ServiceResult<PagedResult<DocumentDto>>> GetPagedAsync(GetDocumentPagedRequest request, CancellationToken cancellationToken = default);
}
