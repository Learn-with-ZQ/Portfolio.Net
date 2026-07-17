using Portfolio.Application.Common.Models;
using Portfolio.Application.DTOs.Awards;
using Portfolio.Application.DTOs.Certifications;
using Portfolio.Application.DTOs.Documents;
using Portfolio.Application.DTOs.Education;
using Portfolio.Application.DTOs.Projects;
using Portfolio.Application.DTOs.Skills;
using Portfolio.Domain.Entities.Awards;
using Portfolio.Domain.Entities.Certifications;
using Portfolio.Domain.Entities.Documents;
using Portfolio.Domain.Entities.Education;
using Portfolio.Domain.Entities.Projects;
using Portfolio.Domain.Entities.Skills;

namespace Portfolio.Application.Mappings;

public static class ProjectMapper
{
    public static ProjectDetailDto ToDetailDto(Project entity) => new()
    {
        ProjectId = entity.ProjectId,
        PortfolioProfileId = entity.PortfolioProfileId,
        ProjectName = entity.ProjectName,
        ProjectSummary = entity.ProjectSummary,
        Practice = entity.Practice,
        CompanyId = entity.CompanyId,
        CompanyName = entity.CompanyName,
        CourseId = entity.CourseId,
        CourseName = entity.CourseName,
        StartDate = entity.StartDate,
        EndDate = entity.EndDate,
        SortOrder = entity.SortOrder,
        RowVersion = entity.RowVersion,
        Audit = ToAudit(entity),
        Technologies = entity.Technologies.Select(t => new ProjectTechnologyDto
        {
            ProjectTechnologyId = t.ProjectTechnologyId,
            TechnologyId = t.TechnologyId,
            TechnologyName = t.TechnologyName ?? string.Empty,
            RowVersion = t.RowVersion
        }).ToList(),
        Details = entity.Details.Select(d => new ProjectDetailItemDto
        {
            ProjectDetailId = d.ProjectDetailId,
            ProjectId = d.ProjectId,
            ProjectDetailName = d.ProjectDetailName,
            SortOrder = d.SortOrder,
            RowVersion = d.RowVersion
        }).ToList()
    };

    public static Project ToEntity(CreateProjectRequest request) => new()
    {
        PortfolioProfileId = request.PortfolioProfileId,
        ProjectName = request.ProjectName,
        ProjectSummary = request.ProjectSummary,
        Practice = request.Practice,
        CompanyId = request.CompanyId,
        CourseId = request.CourseId,
        StartDate = request.StartDate,
        EndDate = request.EndDate,
        SortOrder = request.SortOrder
    };

    public static void ApplyUpdate(Project entity, UpdateProjectRequest request)
    {
        entity.ProjectName = request.ProjectName;
        entity.ProjectSummary = request.ProjectSummary;
        entity.Practice = request.Practice;
        entity.CompanyId = request.CompanyId;
        entity.CourseId = request.CourseId;
        entity.StartDate = request.StartDate;
        entity.EndDate = request.EndDate;
        entity.SortOrder = request.SortOrder;
        entity.RowVersion = request.RowVersion;
    }

    private static AuditInfoDto ToAudit(Portfolio.Domain.Common.AuditableEntity entity) => new()
    {
        CreatedAt = entity.CreatedAt,
        CreatedBy = entity.CreatedBy,
        UpdatedAt = entity.UpdatedAt,
        UpdatedBy = entity.UpdatedBy
    };
}

public static class EducationMapper
{
    public static EducationDetailDto ToDetailDto(EducationRecord entity) => new()
    {
        EducationId = entity.EducationId,
        PortfolioProfileId = entity.PortfolioProfileId,
        DegreeLevelId = entity.DegreeLevelId,
        DegreeLevelName = entity.DegreeLevelName ?? string.Empty,
        DegreePrefix = entity.DegreePrefix ?? string.Empty,
        DegreeId = entity.DegreeId,
        DegreeName = entity.DegreeName ?? string.Empty,
        InstituteId = entity.InstituteId,
        InstituteName = entity.InstituteName ?? string.Empty,
        Location = entity.Location,
        StartDate = entity.StartDate,
        EndDate = entity.EndDate,
        Gpa = entity.Gpa,
        Cgpa = entity.Cgpa,
        SortOrder = entity.SortOrder,
        RowVersion = entity.RowVersion,
        Audit = new AuditInfoDto
        {
            CreatedAt = entity.CreatedAt,
            CreatedBy = entity.CreatedBy,
            UpdatedAt = entity.UpdatedAt,
            UpdatedBy = entity.UpdatedBy
        },
        Courses = entity.Courses.Select(c => new CourseDto
        {
            CourseId = c.CourseId,
            InstituteId = c.InstituteId,
            CourseName = c.CourseName,
            SortOrder = c.SortOrder
        }).ToList()
    };

    public static EducationRecord ToEntity(CreateEducationRequest request) => new()
    {
        PortfolioProfileId = request.PortfolioProfileId,
        DegreeLevelId = request.DegreeLevelId,
        DegreeId = request.DegreeId,
        InstituteId = request.InstituteId,
        StartDate = request.StartDate,
        EndDate = request.EndDate,
        Gpa = request.Gpa,
        Cgpa = request.Cgpa,
        SortOrder = request.SortOrder
    };

    public static void ApplyUpdate(EducationRecord entity, UpdateEducationRequest request)
    {
        entity.DegreeLevelId = request.DegreeLevelId;
        entity.DegreeId = request.DegreeId;
        entity.InstituteId = request.InstituteId;
        entity.StartDate = request.StartDate;
        entity.EndDate = request.EndDate;
        entity.Gpa = request.Gpa;
        entity.Cgpa = request.Cgpa;
        entity.SortOrder = request.SortOrder;
        entity.RowVersion = request.RowVersion;
    }
}

public static class SkillMapper
{
    public static SkillDetailDto ToDetailDto(Skill entity) => new()
    {
        SkillId = entity.SkillId,
        PortfolioProfileId = entity.PortfolioProfileId,
        SkillName = entity.SkillName,
        SortOrder = entity.SortOrder,
        RowVersion = entity.RowVersion,
        Audit = new AuditInfoDto
        {
            CreatedAt = entity.CreatedAt,
            CreatedBy = entity.CreatedBy,
            UpdatedAt = entity.UpdatedAt,
            UpdatedBy = entity.UpdatedBy
        },
        Details = entity.Details.Select(d => new SkillDetailItemDto
        {
            SkillDetailId = d.SkillDetailId,
            SkillId = d.SkillId,
            SkillDetailName = d.SkillDetailName,
            ProficiencyLevel = d.ProficiencyLevel,
            SortOrder = d.SortOrder,
            RowVersion = d.RowVersion
        }).ToList()
    };

    public static Skill ToEntity(CreateSkillRequest request) => new()
    {
        PortfolioProfileId = request.PortfolioProfileId,
        SkillName = request.SkillName,
        SortOrder = request.SortOrder
    };
}

public static class AwardMapper
{
    public static AwardDetailDto ToDetailDto(Award entity) => new()
    {
        AwardId = entity.AwardId,
        PortfolioProfileId = entity.PortfolioProfileId,
        AwardName = entity.AwardName,
        StartDate = entity.StartDate,
        EndDate = entity.EndDate,
        SortOrder = entity.SortOrder,
        RowVersion = entity.RowVersion,
        Audit = new AuditInfoDto
        {
            CreatedAt = entity.CreatedAt,
            CreatedBy = entity.CreatedBy,
            UpdatedAt = entity.UpdatedAt,
            UpdatedBy = entity.UpdatedBy
        },
        Details = entity.Details.Select(d => new AwardDetailItemDto
        {
            AwardDetailId = d.AwardDetailId,
            AwardId = d.AwardId,
            AwardDetailName = d.AwardDetailName,
            SortOrder = d.SortOrder,
            RowVersion = d.RowVersion
        }).ToList()
    };

    public static Award ToEntity(CreateAwardRequest request) => new()
    {
        PortfolioProfileId = request.PortfolioProfileId,
        AwardName = request.AwardName,
        StartDate = request.StartDate,
        EndDate = request.EndDate,
        SortOrder = request.SortOrder
    };
}

public static class CertificationMapper
{
    public static CertificationDetailDto ToDetailDto(Certification entity) => new()
    {
        CertificationId = entity.CertificationId,
        PortfolioProfileId = entity.PortfolioProfileId,
        CertificationIssuerId = entity.CertificationIssuerId,
        IssuerName = entity.IssuerName ?? string.Empty,
        IssuerWebsite = entity.IssuerWebsite,
        CertificationName = entity.CertificationName,
        CredentialId = entity.CredentialId,
        CredentialUrl = entity.CredentialUrl,
        IssueDate = entity.IssueDate,
        ExpiryDate = entity.ExpiryDate,
        DoesNotExpire = entity.DoesNotExpire,
        SortOrder = entity.SortOrder,
        RowVersion = entity.RowVersion,
        Audit = new AuditInfoDto
        {
            CreatedAt = entity.CreatedAt,
            CreatedBy = entity.CreatedBy,
            UpdatedAt = entity.UpdatedAt,
            UpdatedBy = entity.UpdatedBy
        },
        Details = entity.Details.Select(d => new CertificationDetailItemDto
        {
            CertificationDetailId = d.CertificationDetailId,
            CertificationId = d.CertificationId,
            DetailText = d.DetailText,
            SortOrder = d.SortOrder,
            RowVersion = d.RowVersion
        }).ToList()
    };

    public static Certification ToEntity(CreateCertificationRequest request) => new()
    {
        PortfolioProfileId = request.PortfolioProfileId,
        CertificationIssuerId = request.CertificationIssuerId,
        CertificationName = request.CertificationName,
        CredentialId = request.CredentialId,
        CredentialUrl = request.CredentialUrl,
        IssueDate = request.IssueDate,
        ExpiryDate = request.ExpiryDate,
        DoesNotExpire = request.DoesNotExpire,
        SortOrder = request.SortOrder
    };
}

public static class DocumentMapper
{
    public static DocumentDetailDto ToDetailDto(Document entity) => new()
    {
        DocumentId = entity.DocumentId,
        PortfolioProfileId = entity.PortfolioProfileId,
        DocumentTypeId = entity.DocumentTypeId,
        DocumentType = entity.DocumentType ?? string.Empty,
        DocumentTitle = entity.DocumentTitle,
        FileName = entity.FileName,
        FileExtension = entity.FileExtension,
        FileSizeBytes = entity.FileSizeBytes,
        StoragePath = entity.StoragePath,
        MimeType = entity.MimeType,
        IsPublic = entity.IsPublic,
        IsDownloadable = entity.IsDownloadable,
        VersionNumber = entity.VersionNumber,
        SortOrder = entity.SortOrder,
        RowVersion = entity.RowVersion,
        Audit = new AuditInfoDto
        {
            CreatedAt = entity.CreatedAt,
            CreatedBy = entity.CreatedBy,
            UpdatedAt = entity.UpdatedAt,
            UpdatedBy = entity.UpdatedBy
        }
    };

    public static Document ToEntity(CreateDocumentRequest request) => new()
    {
        PortfolioProfileId = request.PortfolioProfileId,
        DocumentTypeId = request.DocumentTypeId,
        DocumentTitle = request.DocumentTitle,
        FileName = request.FileName,
        FileExtension = request.FileExtension,
        FileSizeBytes = request.FileSizeBytes,
        StoragePath = request.StoragePath,
        MimeType = request.MimeType,
        IsPublic = request.IsPublic,
        IsDownloadable = request.IsDownloadable,
        VersionNumber = request.VersionNumber,
        SortOrder = request.SortOrder
    };
}
