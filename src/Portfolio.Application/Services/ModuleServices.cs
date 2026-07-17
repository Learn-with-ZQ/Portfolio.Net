using FluentValidation;
using Portfolio.Application.Abstractions.Persistence;
using Portfolio.Application.Common.BusinessRules;
using Portfolio.Application.Common.Helpers;
using Portfolio.Application.Common.Interfaces;
using Portfolio.Application.Common.Models;
using Portfolio.Application.Common.Results;
using Portfolio.Application.DTOs.Awards;
using Portfolio.Application.DTOs.Certifications;
using Portfolio.Application.DTOs.Documents;
using Portfolio.Application.DTOs.Education;
using Portfolio.Application.DTOs.Projects;
using Portfolio.Application.DTOs.Skills;
using Portfolio.Application.Mappings;
using Portfolio.Application.Services.Interfaces;
using Portfolio.Domain.Entities.Awards;
using Portfolio.Domain.Entities.Certifications;
using Portfolio.Domain.Entities.Documents;
using Portfolio.Domain.Entities.Education;
using Portfolio.Domain.Entities.Projects;
using Portfolio.Domain.Entities.Skills;
using Portfolio.Domain.Exceptions;

namespace Portfolio.Application.Services;

public sealed class ProjectService : ApplicationServiceBase, IProjectService
{
    private readonly IProjectRepository _repository;
    private readonly ICurrentUserService _currentUser;
    private readonly IValidator<CreateProjectRequest> _createValidator;
    private readonly IValidator<UpdateProjectRequest> _updateValidator;

    public ProjectService(IProjectRepository repository, ICurrentUserService currentUser,
        IValidator<CreateProjectRequest> createValidator, IValidator<UpdateProjectRequest> updateValidator)
    {
        _repository = repository;
        _currentUser = currentUser;
        _createValidator = createValidator;
        _updateValidator = updateValidator;
    }

    public async Task<ServiceResult<ProjectDetailDto>> GetByIdAsync(int projectId, CancellationToken cancellationToken = default)
    {
        try
        {
            var entity = await _repository.GetByIdAsync(projectId, cancellationToken).ConfigureAwait(false);
            if (entity is null) throw new NotFoundException("Project", projectId);
            return ServiceResult<ProjectDetailDto>.Success(ProjectMapper.ToDetailDto(entity));
        }
        catch (Exception ex) { return HandleException<ProjectDetailDto>(ex); }
    }

    public async Task<ServiceResult<CommandResultDto>> CreateAsync(CreateProjectRequest request, CancellationToken cancellationToken = default)
    {
        try
        {
            await ValidateAsync(_createValidator, request, cancellationToken).ConfigureAwait(false);
            PortfolioProfileRules.EnsureValidProfileId(request.PortfolioProfileId);
            ProjectContextRules.EnsureSingleContext(request.Practice, request.CompanyId, request.CourseId);
            DateRangeRules.EnsureValidRange(request.StartDate, request.EndDate, "Project");

            var result = await _repository.InsertAsync(ProjectMapper.ToEntity(request), request.TechnologyIds,
                _currentUser.UserId, cancellationToken).ConfigureAwait(false);
            SpResultHelper.EnsureSuccess(result);
            return ServiceResult<CommandResultDto>.Success(SpResultHelper.ToCommandResult(result));
        }
        catch (Exception ex) { return HandleException<CommandResultDto>(ex); }
    }

    public async Task<ServiceResult<CommandResultDto>> UpdateAsync(UpdateProjectRequest request, CancellationToken cancellationToken = default)
    {
        try
        {
            await ValidateAsync(_updateValidator, request, cancellationToken).ConfigureAwait(false);
            ProjectContextRules.EnsureSingleContext(request.Practice, request.CompanyId, request.CourseId);
            DateRangeRules.EnsureValidRange(request.StartDate, request.EndDate, "Project");

            var entity = await _repository.GetByIdAsync(request.ProjectId, cancellationToken).ConfigureAwait(false);
            if (entity is null) throw new NotFoundException("Project", request.ProjectId);

            ProjectMapper.ApplyUpdate(entity, request);
            var result = await _repository.UpdateAsync(entity, _currentUser.UserId, cancellationToken).ConfigureAwait(false);
            SpResultHelper.EnsureSuccess(result);
            return ServiceResult<CommandResultDto>.Success(SpResultHelper.ToCommandResult(result));
        }
        catch (Exception ex) { return HandleException<CommandResultDto>(ex); }
    }

    public async Task<ServiceResult> DeleteAsync(int projectId, CancellationToken cancellationToken = default)
    {
        try
        {
            var result = await _repository.DeleteAsync(projectId, _currentUser.UserId, cancellationToken).ConfigureAwait(false);
            SpResultHelper.EnsureSuccess(result);
            return ServiceResult.Success();
        }
        catch (Exception ex) { return HandleException(ex); }
    }

    public async Task<ServiceResult> SyncTechnologiesAsync(SyncProjectTechnologiesRequest request, CancellationToken cancellationToken = default)
    {
        try
        {
            var result = await _repository.SyncTechnologiesAsync(request.ProjectId, request.TechnologyIds,
                _currentUser.UserId, cancellationToken).ConfigureAwait(false);
            SpResultHelper.EnsureSuccess(result);
            return ServiceResult.Success();
        }
        catch (Exception ex) { return HandleException(ex); }
    }

    public async Task<ServiceResult<IReadOnlyList<ProjectDto>>> SearchAsync(SearchProjectRequest request, CancellationToken cancellationToken = default)
    {
        try
        {
            PortfolioProfileRules.EnsureValidProfileId(request.PortfolioProfileId);
            var items = await _repository.SearchAsync(request, cancellationToken).ConfigureAwait(false);
            return ServiceResult<IReadOnlyList<ProjectDto>>.Success(items);
        }
        catch (Exception ex) { return HandleException<IReadOnlyList<ProjectDto>>(ex); }
    }

    public async Task<ServiceResult<PagedResult<ProjectDto>>> GetPagedAsync(GetProjectPagedRequest request, CancellationToken cancellationToken = default)
    {
        try
        {
            PortfolioProfileRules.EnsureValidProfileId(request.PortfolioProfileId);
            var page = await _repository.GetPagedAsync(request, cancellationToken).ConfigureAwait(false);
            return ServiceResult<PagedResult<ProjectDto>>.Success(page);
        }
        catch (Exception ex) { return HandleException<PagedResult<ProjectDto>>(ex); }
    }
}

public sealed class EducationService : ApplicationServiceBase, IEducationService
{
    private readonly IEducationRepository _repository;
    private readonly ICurrentUserService _currentUser;
    private readonly IValidator<CreateEducationRequest> _createValidator;

    public EducationService(IEducationRepository repository, ICurrentUserService currentUser,
        IValidator<CreateEducationRequest> createValidator)
    {
        _repository = repository;
        _currentUser = currentUser;
        _createValidator = createValidator;
    }

    public async Task<ServiceResult<EducationDetailDto>> GetByIdAsync(int educationId, CancellationToken cancellationToken = default)
    {
        try
        {
            var entity = await _repository.GetByIdAsync(educationId, cancellationToken).ConfigureAwait(false);
            if (entity is null) throw new NotFoundException("Education", educationId);
            return ServiceResult<EducationDetailDto>.Success(EducationMapper.ToDetailDto(entity));
        }
        catch (Exception ex) { return HandleException<EducationDetailDto>(ex); }
    }

    public async Task<ServiceResult<CommandResultDto>> CreateAsync(CreateEducationRequest request, CancellationToken cancellationToken = default)
    {
        try
        {
            await ValidateAsync(_createValidator, request, cancellationToken).ConfigureAwait(false);
            PortfolioProfileRules.EnsureValidProfileId(request.PortfolioProfileId);
            DateRangeRules.EnsureValidRange(request.StartDate, request.EndDate, "Education");
            GpaRules.EnsureValidGpa(request.Gpa, request.Cgpa);

            var result = await _repository.InsertAsync(EducationMapper.ToEntity(request), _currentUser.UserId, cancellationToken).ConfigureAwait(false);
            SpResultHelper.EnsureSuccess(result);
            return ServiceResult<CommandResultDto>.Success(SpResultHelper.ToCommandResult(result));
        }
        catch (Exception ex) { return HandleException<CommandResultDto>(ex); }
    }

    public async Task<ServiceResult<CommandResultDto>> UpdateAsync(UpdateEducationRequest request, CancellationToken cancellationToken = default)
    {
        try
        {
            var entity = await _repository.GetByIdAsync(request.EducationId, cancellationToken).ConfigureAwait(false);
            if (entity is null) throw new NotFoundException("Education", request.EducationId);

            DateRangeRules.EnsureValidRange(request.StartDate, request.EndDate, "Education");
            GpaRules.EnsureValidGpa(request.Gpa, request.Cgpa);
            EducationMapper.ApplyUpdate(entity, request);

            var result = await _repository.UpdateAsync(entity, _currentUser.UserId, cancellationToken).ConfigureAwait(false);
            SpResultHelper.EnsureSuccess(result);
            return ServiceResult<CommandResultDto>.Success(SpResultHelper.ToCommandResult(result));
        }
        catch (Exception ex) { return HandleException<CommandResultDto>(ex); }
    }

    public async Task<ServiceResult> DeleteAsync(int educationId, CancellationToken cancellationToken = default)
    {
        try
        {
            var result = await _repository.DeleteAsync(educationId, _currentUser.UserId, cancellationToken).ConfigureAwait(false);
            SpResultHelper.EnsureSuccess(result);
            return ServiceResult.Success();
        }
        catch (Exception ex) { return HandleException(ex); }
    }

    public async Task<ServiceResult<IReadOnlyList<EducationDto>>> SearchAsync(SearchEducationRequest request, CancellationToken cancellationToken = default)
    {
        try
        {
            PortfolioProfileRules.EnsureValidProfileId(request.PortfolioProfileId);
            return ServiceResult<IReadOnlyList<EducationDto>>.Success(await _repository.SearchAsync(request, cancellationToken).ConfigureAwait(false));
        }
        catch (Exception ex) { return HandleException<IReadOnlyList<EducationDto>>(ex); }
    }

    public async Task<ServiceResult<PagedResult<EducationDto>>> GetPagedAsync(GetEducationPagedRequest request, CancellationToken cancellationToken = default)
    {
        try
        {
            PortfolioProfileRules.EnsureValidProfileId(request.PortfolioProfileId);
            return ServiceResult<PagedResult<EducationDto>>.Success(await _repository.GetPagedAsync(request, cancellationToken).ConfigureAwait(false));
        }
        catch (Exception ex) { return HandleException<PagedResult<EducationDto>>(ex); }
    }
}

public sealed class SkillService : ApplicationServiceBase, ISkillService
{
    private readonly ISkillRepository _repository;
    private readonly ICurrentUserService _currentUser;
    private readonly IValidator<CreateSkillRequest> _createValidator;

    public SkillService(ISkillRepository repository, ICurrentUserService currentUser, IValidator<CreateSkillRequest> createValidator)
    {
        _repository = repository;
        _currentUser = currentUser;
        _createValidator = createValidator;
    }

    public async Task<ServiceResult<SkillDetailDto>> GetByIdAsync(int skillId, CancellationToken cancellationToken = default)
    {
        try
        {
            var entity = await _repository.GetByIdAsync(skillId, cancellationToken).ConfigureAwait(false);
            if (entity is null) throw new NotFoundException("Skill", skillId);
            return ServiceResult<SkillDetailDto>.Success(SkillMapper.ToDetailDto(entity));
        }
        catch (Exception ex) { return HandleException<SkillDetailDto>(ex); }
    }

    public async Task<ServiceResult<CommandResultDto>> CreateAsync(CreateSkillRequest request, CancellationToken cancellationToken = default)
    {
        try
        {
            await ValidateAsync(_createValidator, request, cancellationToken).ConfigureAwait(false);
            PortfolioProfileRules.EnsureValidProfileId(request.PortfolioProfileId);
            var result = await _repository.InsertAsync(SkillMapper.ToEntity(request), _currentUser.UserId, cancellationToken).ConfigureAwait(false);
            SpResultHelper.EnsureSuccess(result);
            return ServiceResult<CommandResultDto>.Success(SpResultHelper.ToCommandResult(result));
        }
        catch (Exception ex) { return HandleException<CommandResultDto>(ex); }
    }

    public async Task<ServiceResult<CommandResultDto>> UpdateAsync(UpdateSkillRequest request, CancellationToken cancellationToken = default)
    {
        try
        {
            var entity = await _repository.GetByIdAsync(request.SkillId, cancellationToken).ConfigureAwait(false);
            if (entity is null) throw new NotFoundException("Skill", request.SkillId);
            entity.SkillName = request.SkillName;
            entity.SortOrder = request.SortOrder;
            entity.RowVersion = request.RowVersion;
            var result = await _repository.UpdateAsync(entity, _currentUser.UserId, cancellationToken).ConfigureAwait(false);
            SpResultHelper.EnsureSuccess(result);
            return ServiceResult<CommandResultDto>.Success(SpResultHelper.ToCommandResult(result));
        }
        catch (Exception ex) { return HandleException<CommandResultDto>(ex); }
    }

    public async Task<ServiceResult> DeleteAsync(int skillId, CancellationToken cancellationToken = default)
    {
        try
        {
            SpResultHelper.EnsureSuccess(await _repository.DeleteAsync(skillId, _currentUser.UserId, cancellationToken).ConfigureAwait(false));
            return ServiceResult.Success();
        }
        catch (Exception ex) { return HandleException(ex); }
    }

    public async Task<ServiceResult<IReadOnlyList<SkillSearchResultDto>>> SearchAsync(SearchSkillRequest request, CancellationToken cancellationToken = default)
    {
        try
        {
            PortfolioProfileRules.EnsureValidProfileId(request.PortfolioProfileId);
            return ServiceResult<IReadOnlyList<SkillSearchResultDto>>.Success(await _repository.SearchAsync(request, cancellationToken).ConfigureAwait(false));
        }
        catch (Exception ex) { return HandleException<IReadOnlyList<SkillSearchResultDto>>(ex); }
    }

    public async Task<ServiceResult<PagedResult<SkillDto>>> GetPagedAsync(GetSkillPagedRequest request, CancellationToken cancellationToken = default)
    {
        try
        {
            PortfolioProfileRules.EnsureValidProfileId(request.PortfolioProfileId);
            return ServiceResult<PagedResult<SkillDto>>.Success(await _repository.GetPagedAsync(request, cancellationToken).ConfigureAwait(false));
        }
        catch (Exception ex) { return HandleException<PagedResult<SkillDto>>(ex); }
    }
}

public sealed class AwardService : ApplicationServiceBase, IAwardService
{
    private readonly IAwardRepository _repository;
    private readonly ICurrentUserService _currentUser;
    private readonly IValidator<CreateAwardRequest> _createValidator;

    public AwardService(IAwardRepository repository, ICurrentUserService currentUser, IValidator<CreateAwardRequest> createValidator)
    {
        _repository = repository;
        _currentUser = currentUser;
        _createValidator = createValidator;
    }

    public async Task<ServiceResult<AwardDetailDto>> GetByIdAsync(int awardId, CancellationToken cancellationToken = default)
    {
        try
        {
            var entity = await _repository.GetByIdAsync(awardId, cancellationToken).ConfigureAwait(false);
            if (entity is null) throw new NotFoundException("Award", awardId);
            return ServiceResult<AwardDetailDto>.Success(AwardMapper.ToDetailDto(entity));
        }
        catch (Exception ex) { return HandleException<AwardDetailDto>(ex); }
    }

    public async Task<ServiceResult<CommandResultDto>> CreateAsync(CreateAwardRequest request, CancellationToken cancellationToken = default)
    {
        try
        {
            await ValidateAsync(_createValidator, request, cancellationToken).ConfigureAwait(false);
            PortfolioProfileRules.EnsureValidProfileId(request.PortfolioProfileId);
            DateRangeRules.EnsureValidRange(request.StartDate, request.EndDate, "Award");
            var result = await _repository.InsertAsync(AwardMapper.ToEntity(request), _currentUser.UserId, cancellationToken).ConfigureAwait(false);
            SpResultHelper.EnsureSuccess(result);
            return ServiceResult<CommandResultDto>.Success(SpResultHelper.ToCommandResult(result));
        }
        catch (Exception ex) { return HandleException<CommandResultDto>(ex); }
    }

    public async Task<ServiceResult<CommandResultDto>> UpdateAsync(UpdateAwardRequest request, CancellationToken cancellationToken = default)
    {
        try
        {
            var entity = await _repository.GetByIdAsync(request.AwardId, cancellationToken).ConfigureAwait(false);
            if (entity is null) throw new NotFoundException("Award", request.AwardId);
            entity.AwardName = request.AwardName;
            entity.StartDate = request.StartDate;
            entity.EndDate = request.EndDate;
            entity.SortOrder = request.SortOrder;
            entity.RowVersion = request.RowVersion;
            var result = await _repository.UpdateAsync(entity, _currentUser.UserId, cancellationToken).ConfigureAwait(false);
            SpResultHelper.EnsureSuccess(result);
            return ServiceResult<CommandResultDto>.Success(SpResultHelper.ToCommandResult(result));
        }
        catch (Exception ex) { return HandleException<CommandResultDto>(ex); }
    }

    public async Task<ServiceResult> DeleteAsync(int awardId, CancellationToken cancellationToken = default)
    {
        try
        {
            SpResultHelper.EnsureSuccess(await _repository.DeleteAsync(awardId, _currentUser.UserId, cancellationToken).ConfigureAwait(false));
            return ServiceResult.Success();
        }
        catch (Exception ex) { return HandleException(ex); }
    }

    public async Task<ServiceResult<IReadOnlyList<AwardDto>>> SearchAsync(SearchAwardRequest request, CancellationToken cancellationToken = default)
    {
        try
        {
            PortfolioProfileRules.EnsureValidProfileId(request.PortfolioProfileId);
            return ServiceResult<IReadOnlyList<AwardDto>>.Success(await _repository.SearchAsync(request, cancellationToken).ConfigureAwait(false));
        }
        catch (Exception ex) { return HandleException<IReadOnlyList<AwardDto>>(ex); }
    }

    public async Task<ServiceResult<PagedResult<AwardDto>>> GetPagedAsync(GetAwardPagedRequest request, CancellationToken cancellationToken = default)
    {
        try
        {
            PortfolioProfileRules.EnsureValidProfileId(request.PortfolioProfileId);
            return ServiceResult<PagedResult<AwardDto>>.Success(await _repository.GetPagedAsync(request, cancellationToken).ConfigureAwait(false));
        }
        catch (Exception ex) { return HandleException<PagedResult<AwardDto>>(ex); }
    }
}

public sealed class CertificationService : ApplicationServiceBase, ICertificationService
{
    private readonly ICertificationRepository _repository;
    private readonly ICurrentUserService _currentUser;
    private readonly IValidator<CreateCertificationRequest> _createValidator;

    public CertificationService(ICertificationRepository repository, ICurrentUserService currentUser,
        IValidator<CreateCertificationRequest> createValidator)
    {
        _repository = repository;
        _currentUser = currentUser;
        _createValidator = createValidator;
    }

    public async Task<ServiceResult<CertificationDetailDto>> GetByIdAsync(int certificationId, CancellationToken cancellationToken = default)
    {
        try
        {
            var entity = await _repository.GetByIdAsync(certificationId, cancellationToken).ConfigureAwait(false);
            if (entity is null) throw new NotFoundException("Certification", certificationId);
            return ServiceResult<CertificationDetailDto>.Success(CertificationMapper.ToDetailDto(entity));
        }
        catch (Exception ex) { return HandleException<CertificationDetailDto>(ex); }
    }

    public async Task<ServiceResult<CommandResultDto>> CreateAsync(CreateCertificationRequest request, CancellationToken cancellationToken = default)
    {
        try
        {
            await ValidateAsync(_createValidator, request, cancellationToken).ConfigureAwait(false);
            PortfolioProfileRules.EnsureValidProfileId(request.PortfolioProfileId);
            if (request.DoesNotExpire) request.ExpiryDate = null;
            var result = await _repository.InsertAsync(CertificationMapper.ToEntity(request), _currentUser.UserId, cancellationToken).ConfigureAwait(false);
            SpResultHelper.EnsureSuccess(result);
            return ServiceResult<CommandResultDto>.Success(SpResultHelper.ToCommandResult(result));
        }
        catch (Exception ex) { return HandleException<CommandResultDto>(ex); }
    }

    public async Task<ServiceResult<CommandResultDto>> UpdateAsync(UpdateCertificationRequest request, CancellationToken cancellationToken = default)
    {
        try
        {
            var entity = await _repository.GetByIdAsync(request.CertificationId, cancellationToken).ConfigureAwait(false);
            if (entity is null) throw new NotFoundException("Certification", request.CertificationId);
            entity.CertificationIssuerId = request.CertificationIssuerId;
            entity.CertificationName = request.CertificationName;
            entity.CredentialId = request.CredentialId;
            entity.CredentialUrl = request.CredentialUrl;
            entity.IssueDate = request.IssueDate;
            entity.ExpiryDate = request.DoesNotExpire ? null : request.ExpiryDate;
            entity.DoesNotExpire = request.DoesNotExpire;
            entity.SortOrder = request.SortOrder;
            entity.RowVersion = request.RowVersion;
            var result = await _repository.UpdateAsync(entity, _currentUser.UserId, cancellationToken).ConfigureAwait(false);
            SpResultHelper.EnsureSuccess(result);
            return ServiceResult<CommandResultDto>.Success(SpResultHelper.ToCommandResult(result));
        }
        catch (Exception ex) { return HandleException<CommandResultDto>(ex); }
    }

    public async Task<ServiceResult> DeleteAsync(int certificationId, CancellationToken cancellationToken = default)
    {
        try
        {
            SpResultHelper.EnsureSuccess(await _repository.DeleteAsync(certificationId, _currentUser.UserId, cancellationToken).ConfigureAwait(false));
            return ServiceResult.Success();
        }
        catch (Exception ex) { return HandleException(ex); }
    }

    public async Task<ServiceResult<IReadOnlyList<CertificationDto>>> SearchAsync(SearchCertificationRequest request, CancellationToken cancellationToken = default)
    {
        try
        {
            PortfolioProfileRules.EnsureValidProfileId(request.PortfolioProfileId);
            return ServiceResult<IReadOnlyList<CertificationDto>>.Success(await _repository.SearchAsync(request, cancellationToken).ConfigureAwait(false));
        }
        catch (Exception ex) { return HandleException<IReadOnlyList<CertificationDto>>(ex); }
    }

    public async Task<ServiceResult<PagedResult<CertificationDto>>> GetPagedAsync(GetCertificationPagedRequest request, CancellationToken cancellationToken = default)
    {
        try
        {
            PortfolioProfileRules.EnsureValidProfileId(request.PortfolioProfileId);
            return ServiceResult<PagedResult<CertificationDto>>.Success(await _repository.GetPagedAsync(request, cancellationToken).ConfigureAwait(false));
        }
        catch (Exception ex) { return HandleException<PagedResult<CertificationDto>>(ex); }
    }
}

public sealed class DocumentService : ApplicationServiceBase, IDocumentService
{
    private readonly IDocumentRepository _repository;
    private readonly ICurrentUserService _currentUser;
    private readonly IValidator<CreateDocumentRequest> _createValidator;

    public DocumentService(IDocumentRepository repository, ICurrentUserService currentUser,
        IValidator<CreateDocumentRequest> createValidator)
    {
        _repository = repository;
        _currentUser = currentUser;
        _createValidator = createValidator;
    }

    public async Task<ServiceResult<DocumentDetailDto>> GetByIdAsync(int documentId, CancellationToken cancellationToken = default)
    {
        try
        {
            var entity = await _repository.GetByIdAsync(documentId, cancellationToken).ConfigureAwait(false);
            if (entity is null) throw new NotFoundException("Document", documentId);
            return ServiceResult<DocumentDetailDto>.Success(DocumentMapper.ToDetailDto(entity));
        }
        catch (Exception ex) { return HandleException<DocumentDetailDto>(ex); }
    }

    public async Task<ServiceResult<CommandResultDto>> CreateAsync(CreateDocumentRequest request, CancellationToken cancellationToken = default)
    {
        try
        {
            await ValidateAsync(_createValidator, request, cancellationToken).ConfigureAwait(false);
            PortfolioProfileRules.EnsureValidProfileId(request.PortfolioProfileId);
            var result = await _repository.InsertAsync(DocumentMapper.ToEntity(request), _currentUser.UserId, cancellationToken).ConfigureAwait(false);
            SpResultHelper.EnsureSuccess(result);
            return ServiceResult<CommandResultDto>.Success(SpResultHelper.ToCommandResult(result));
        }
        catch (Exception ex) { return HandleException<CommandResultDto>(ex); }
    }

    public async Task<ServiceResult<CommandResultDto>> UpdateAsync(UpdateDocumentRequest request, CancellationToken cancellationToken = default)
    {
        try
        {
            var entity = await _repository.GetByIdAsync(request.DocumentId, cancellationToken).ConfigureAwait(false);
            if (entity is null) throw new NotFoundException("Document", request.DocumentId);
            entity.DocumentTypeId = request.DocumentTypeId;
            entity.DocumentTitle = request.DocumentTitle;
            entity.FileName = request.FileName;
            entity.FileExtension = request.FileExtension;
            entity.FileSizeBytes = request.FileSizeBytes;
            entity.StoragePath = request.StoragePath;
            entity.MimeType = request.MimeType;
            entity.IsPublic = request.IsPublic;
            entity.IsDownloadable = request.IsDownloadable;
            entity.VersionNumber = request.VersionNumber;
            entity.SortOrder = request.SortOrder;
            entity.RowVersion = request.RowVersion;
            var result = await _repository.UpdateAsync(entity, _currentUser.UserId, cancellationToken).ConfigureAwait(false);
            SpResultHelper.EnsureSuccess(result);
            return ServiceResult<CommandResultDto>.Success(SpResultHelper.ToCommandResult(result));
        }
        catch (Exception ex) { return HandleException<CommandResultDto>(ex); }
    }

    public async Task<ServiceResult> DeleteAsync(int documentId, CancellationToken cancellationToken = default)
    {
        try
        {
            SpResultHelper.EnsureSuccess(await _repository.DeleteAsync(documentId, _currentUser.UserId, cancellationToken).ConfigureAwait(false));
            return ServiceResult.Success();
        }
        catch (Exception ex) { return HandleException(ex); }
    }

    public async Task<ServiceResult<CommandResultDto>> CreateVersionAsync(CreateDocumentVersionRequest request, CancellationToken cancellationToken = default)
    {
        try
        {
            var doc = new Document
            {
                FileName = request.FileName,
                FileExtension = request.FileExtension,
                FileSizeBytes = request.FileSizeBytes,
                StoragePath = request.StoragePath,
                MimeType = request.MimeType
            };
            var result = await _repository.CreateNewVersionAsync(request.SourceDocumentId, doc, _currentUser.UserId, cancellationToken).ConfigureAwait(false);
            SpResultHelper.EnsureSuccess(result);
            return ServiceResult<CommandResultDto>.Success(SpResultHelper.ToCommandResult(result));
        }
        catch (Exception ex) { return HandleException<CommandResultDto>(ex); }
    }

    public async Task<ServiceResult<IReadOnlyList<DocumentDto>>> SearchAsync(SearchDocumentRequest request, CancellationToken cancellationToken = default)
    {
        try
        {
            PortfolioProfileRules.EnsureValidProfileId(request.PortfolioProfileId);
            return ServiceResult<IReadOnlyList<DocumentDto>>.Success(await _repository.SearchAsync(request, cancellationToken).ConfigureAwait(false));
        }
        catch (Exception ex) { return HandleException<IReadOnlyList<DocumentDto>>(ex); }
    }

    public async Task<ServiceResult<PagedResult<DocumentDto>>> GetPagedAsync(GetDocumentPagedRequest request, CancellationToken cancellationToken = default)
    {
        try
        {
            PortfolioProfileRules.EnsureValidProfileId(request.PortfolioProfileId);
            return ServiceResult<PagedResult<DocumentDto>>.Success(await _repository.GetPagedAsync(request, cancellationToken).ConfigureAwait(false));
        }
        catch (Exception ex) { return HandleException<PagedResult<DocumentDto>>(ex); }
    }
}
