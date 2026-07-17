using System.Data;
using Dapper;
using Microsoft.Extensions.Options;
using Portfolio.Application.Abstractions.Persistence;
using Portfolio.Application.Common.Models;
using Portfolio.Application.Common.Settings;
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
using Portfolio.Persistence.Common;
using Portfolio.Persistence.Mapping;

namespace Portfolio.Persistence.Repositories;

public sealed class ProjectRepository : IProjectRepository
{
    private readonly Database _db;

    public ProjectRepository(Database db) => _db = db;

    private static DataTable TechTable(IEnumerable<int> ids)
    {
        var table = new DataTable();
        table.Columns.Add("Id", typeof(int));
        foreach (var id in ids) table.Rows.Add(id);
        return table;
    }

    public async Task<Project?> GetByIdAsync(int projectId, CancellationToken cancellationToken = default)
    {
        var (_, entity) = await _db.QueryMultipleAsync(
            "dbo.usp_Projects_GetById",
            new Dictionary<string, object?> { ["ProjectID_Pk"] = projectId, ["IncludeDeleted"] = false },
            async grid =>
            {
                var header = await grid.ReadFirstOrDefaultAsync().ConfigureAwait(false);
                if (header is null) return (Project?)null;
                var e = DapperColumnMapping.MapProject(header);
                e.Technologies = (await grid.ReadAsync().ConfigureAwait(false)).Select(DapperColumnMapping.MapProjectTechnology).ToList();
                e.Details = (await grid.ReadAsync().ConfigureAwait(false)).Select(DapperColumnMapping.MapProjectDetail).ToList();
                return e;
            },
            cancellationToken).ConfigureAwait(false);
        return entity;
    }

    public Task<SpExecutionResult> InsertAsync(Project project, IEnumerable<int> technologyIds, int? createdBy, CancellationToken cancellationToken = default) =>
        _db.ExecuteAsync("dbo.usp_Projects_Insert", new Dictionary<string, object?>
        {
            ["PortfolioProfileID_Fk"] = project.PortfolioProfileId,
            ["ProjectName"] = project.ProjectName,
            ["ProjectSummary"] = project.ProjectSummary,
            ["Practice"] = project.Practice,
            ["CompanyID_Fk"] = project.CompanyId,
            ["CourseID_Fk"] = project.CourseId,
            ["StartDate"] = project.StartDate,
            ["EndDate"] = project.EndDate,
            ["SortOrder"] = project.SortOrder,
            ["CreatedBy"] = createdBy,
            ["TechnologyIDs"] = TechTable(technologyIds).AsTableValuedParameter("dbo.IntListTableType")
        }, outIdKey: "OutProjectID", cancellationToken);

    public Task<SpExecutionResult> UpdateAsync(Project project, int? updatedBy, CancellationToken cancellationToken = default) =>
        _db.ExecuteAsync("dbo.usp_Projects_Update", new Dictionary<string, object?>
        {
            ["ProjectID_Pk"] = project.ProjectId,
            ["ProjectName"] = project.ProjectName,
            ["ProjectSummary"] = project.ProjectSummary,
            ["Practice"] = project.Practice,
            ["CompanyID_Fk"] = project.CompanyId,
            ["CourseID_Fk"] = project.CourseId,
            ["StartDate"] = project.StartDate,
            ["EndDate"] = project.EndDate,
            ["SortOrder"] = project.SortOrder,
            ["RowVersion"] = project.RowVersion,
            ["UpdatedBy"] = updatedBy
        }, cancellationToken: cancellationToken);

    public Task<SpExecutionResult> DeleteAsync(int projectId, int? deletedBy, CancellationToken cancellationToken = default) =>
        _db.ExecuteAsync("dbo.usp_Projects_Delete", new Dictionary<string, object?>
        {
            ["ProjectID_Pk"] = projectId,
            ["DeletedBy"] = deletedBy
        }, cancellationToken: cancellationToken);

    public Task<SpExecutionResult> SyncTechnologiesAsync(int projectId, IEnumerable<int> technologyIds, int? updatedBy, CancellationToken cancellationToken = default) =>
        _db.ExecuteAsync("dbo.usp_ProjectTechnology_Sync", new Dictionary<string, object?>
        {
            ["ProjectID_Fk"] = projectId,
            ["UpdatedBy"] = updatedBy,
            ["TechnologyIDs"] = TechTable(technologyIds).AsTableValuedParameter("dbo.IntListTableType")
        }, cancellationToken: cancellationToken);

    public async Task<IReadOnlyList<ProjectDto>> SearchAsync(SearchProjectRequest request, CancellationToken cancellationToken = default)
    {
        var (_, rows) = await _db.QueryAsync<dynamic>("dbo.usp_Projects_Search", new Dictionary<string, object?>
        {
            ["PortfolioProfileID_Fk"] = request.PortfolioProfileId,
            ["SearchTerm"] = request.SearchTerm,
            ["CompanyID_Fk"] = request.CompanyId,
            ["CourseID_Fk"] = request.CourseId,
            ["TechnologyID_Fk"] = request.TechnologyId
        }, cancellationToken).ConfigureAwait(false);
        return rows.Select(MapProjectDto).ToList();
    }

    public async Task<PagedResult<ProjectDto>> GetPagedAsync(GetProjectPagedRequest request, CancellationToken cancellationToken = default)
    {
        var (_, rows, total, pages) = await _db.QueryPagedAsync<dynamic>("dbo.usp_Projects_GetPaged", new Dictionary<string, object?>
        {
            ["PortfolioProfileID_Fk"] = request.PortfolioProfileId,
            ["PageNumber"] = request.PageNumber,
            ["PageSize"] = request.PageSize,
            ["SearchTerm"] = request.SearchTerm,
            ["CompanyID_Fk"] = request.CompanyId,
            ["TechnologyID_Fk"] = request.TechnologyId
        }, cancellationToken).ConfigureAwait(false);
        return new PagedResult<ProjectDto> { Items = rows.Select(MapProjectDto).ToList(), PageNumber = request.PageNumber, PageSize = request.PageSize, TotalRecords = total, TotalPages = pages };
    }

    public Task<SpExecutionResult> InsertDetailAsync(ProjectDetail detail, int? createdBy, CancellationToken cancellationToken = default) =>
        _db.ExecuteAsync("dbo.usp_ProjectDetail_Insert", new Dictionary<string, object?>
        {
            ["ProjectID_Fk"] = detail.ProjectId,
            ["ProjectDetailName"] = detail.ProjectDetailName,
            ["SortOrder"] = detail.SortOrder,
            ["CreatedBy"] = createdBy
        }, outIdKey: "OutProjectDetailID", cancellationToken);

    public Task<SpExecutionResult> UpdateDetailAsync(ProjectDetail detail, int? updatedBy, CancellationToken cancellationToken = default) =>
        _db.ExecuteAsync("dbo.usp_ProjectDetail_Update", new Dictionary<string, object?>
        {
            ["ProjectDetailID_Pk"] = detail.ProjectDetailId,
            ["ProjectDetailName"] = detail.ProjectDetailName,
            ["SortOrder"] = detail.SortOrder,
            ["RowVersion"] = detail.RowVersion,
            ["UpdatedBy"] = updatedBy
        }, cancellationToken: cancellationToken);

    public Task<SpExecutionResult> DeleteDetailAsync(int projectDetailId, int? deletedBy, CancellationToken cancellationToken = default) =>
        _db.ExecuteAsync("dbo.usp_ProjectDetail_Delete", new Dictionary<string, object?>
        {
            ["ProjectDetailID_Pk"] = projectDetailId,
            ["DeletedBy"] = deletedBy
        }, cancellationToken: cancellationToken);

    private static ProjectDto MapProjectDto(dynamic row)
    {
        var r = (IDictionary<string, object>)row;
        return new ProjectDto
        {
            ProjectId = Convert.ToInt32(r["ProjectID_Pk"]),
            ProjectName = Convert.ToString(r["ProjectName"]) ?? string.Empty,
            ContextName = r.ContainsKey("ContextName") ? Convert.ToString(r["ContextName"]) : null,
            Technologies = r.ContainsKey("Technologies") ? Convert.ToString(r["Technologies"]) ?? string.Empty : string.Empty,
            StartDate = DateOnly.FromDateTime(Convert.ToDateTime(r["StartDate"])),
            EndDate = r.TryGetValue("EndDate", out var e) && e is not DBNull ? DateOnly.FromDateTime(Convert.ToDateTime(e)) : null,
            SortOrder = Convert.ToInt32(r["SortOrder"])
        };
    }
}

public sealed class EducationRepository : IEducationRepository
{
    private readonly Database _db;

    public EducationRepository(Database db) => _db = db;

    public async Task<EducationRecord?> GetByIdAsync(int educationId, CancellationToken cancellationToken = default)
    {
        var (_, entity) = await _db.QueryMultipleAsync(
            "dbo.usp_Education_GetById",
            new Dictionary<string, object?> { ["EducationID_Pk"] = educationId, ["IncludeDeleted"] = false },
            async grid =>
            {
                var header = await grid.ReadFirstOrDefaultAsync().ConfigureAwait(false);
                if (header is null) return (EducationRecord?)null;
                var e = DapperColumnMapping.MapEducation(header);
                e.Courses = (await grid.ReadAsync().ConfigureAwait(false)).Select(DapperColumnMapping.MapCourse).ToList();
                return e;
            },
            cancellationToken).ConfigureAwait(false);
        return entity;
    }

    public Task<SpExecutionResult> InsertAsync(EducationRecord education, int? createdBy, CancellationToken cancellationToken = default) =>
        _db.ExecuteAsync("dbo.usp_Education_Insert", new Dictionary<string, object?>
        {
            ["PortfolioProfileID_Fk"] = education.PortfolioProfileId,
            ["DegreeLevelID_Fk"] = education.DegreeLevelId,
            ["DegreeID_Fk"] = education.DegreeId,
            ["InstituteID_Fk"] = education.InstituteId,
            ["StartDate"] = education.StartDate,
            ["EndDate"] = education.EndDate,
            ["GPA"] = education.Gpa,
            ["CGPA"] = education.Cgpa,
            ["SortOrder"] = education.SortOrder,
            ["CreatedBy"] = createdBy
        }, outIdKey: "OutEducationID", cancellationToken);

    public Task<SpExecutionResult> UpdateAsync(EducationRecord education, int? updatedBy, CancellationToken cancellationToken = default) =>
        _db.ExecuteAsync("dbo.usp_Education_Update", new Dictionary<string, object?>
        {
            ["EducationID_Pk"] = education.EducationId,
            ["DegreeLevelID_Fk"] = education.DegreeLevelId,
            ["DegreeID_Fk"] = education.DegreeId,
            ["InstituteID_Fk"] = education.InstituteId,
            ["StartDate"] = education.StartDate,
            ["EndDate"] = education.EndDate,
            ["GPA"] = education.Gpa,
            ["CGPA"] = education.Cgpa,
            ["SortOrder"] = education.SortOrder,
            ["RowVersion"] = education.RowVersion,
            ["UpdatedBy"] = updatedBy
        }, cancellationToken: cancellationToken);

    public Task<SpExecutionResult> DeleteAsync(int educationId, int? deletedBy, CancellationToken cancellationToken = default) =>
        _db.ExecuteAsync("dbo.usp_Education_Delete", new Dictionary<string, object?>
        {
            ["EducationID_Pk"] = educationId,
            ["DeletedBy"] = deletedBy
        }, cancellationToken: cancellationToken);

    public async Task<IReadOnlyList<EducationDto>> SearchAsync(SearchEducationRequest request, CancellationToken cancellationToken = default)
    {
        var (_, rows) = await _db.QueryAsync<EducationDto>("dbo.usp_Education_Search", new Dictionary<string, object?>
        {
            ["PortfolioProfileID_Fk"] = request.PortfolioProfileId,
            ["SearchTerm"] = request.SearchTerm,
            ["InstituteID_Fk"] = request.InstituteId,
            ["DegreeLevelID_Fk"] = request.DegreeLevelId
        }, cancellationToken).ConfigureAwait(false);
        return rows;
    }

    public async Task<PagedResult<EducationDto>> GetPagedAsync(GetEducationPagedRequest request, CancellationToken cancellationToken = default)
    {
        var (_, rows, total, pages) = await _db.QueryPagedAsync<EducationDto>("dbo.usp_Education_GetPaged", new Dictionary<string, object?>
        {
            ["PortfolioProfileID_Fk"] = request.PortfolioProfileId,
            ["PageNumber"] = request.PageNumber,
            ["PageSize"] = request.PageSize,
            ["SearchTerm"] = request.SearchTerm,
            ["InstituteID_Fk"] = request.InstituteId
        }, cancellationToken).ConfigureAwait(false);
        return new PagedResult<EducationDto> { Items = rows, PageNumber = request.PageNumber, PageSize = request.PageSize, TotalRecords = total, TotalPages = pages };
    }
}

public sealed class SkillRepository : ISkillRepository
{
    private readonly Database _db;

    public SkillRepository(Database db) => _db = db;

    public async Task<Skill?> GetByIdAsync(int skillId, CancellationToken cancellationToken = default)
    {
        var (_, entity) = await _db.QueryMultipleAsync(
            "dbo.usp_Skills_GetById",
            new Dictionary<string, object?> { ["SkillID_Pk"] = skillId, ["IncludeDeleted"] = false },
            async grid =>
            {
                var header = await grid.ReadFirstOrDefaultAsync().ConfigureAwait(false);
                if (header is null) return (Skill?)null;
                var e = DapperColumnMapping.MapSkill(header);
                e.Details = (await grid.ReadAsync().ConfigureAwait(false)).Select(DapperColumnMapping.MapSkillDetail).ToList();
                return e;
            },
            cancellationToken).ConfigureAwait(false);
        return entity;
    }

    public Task<SpExecutionResult> InsertAsync(Skill skill, int? createdBy, CancellationToken cancellationToken = default) =>
        _db.ExecuteAsync("dbo.usp_Skills_Insert", new Dictionary<string, object?>
        {
            ["PortfolioProfileID_Fk"] = skill.PortfolioProfileId,
            ["SkillName"] = skill.SkillName,
            ["SortOrder"] = skill.SortOrder,
            ["CreatedBy"] = createdBy
        }, outIdKey: "OutSkillID", cancellationToken);

    public Task<SpExecutionResult> UpdateAsync(Skill skill, int? updatedBy, CancellationToken cancellationToken = default) =>
        _db.ExecuteAsync("dbo.usp_Skills_Update", new Dictionary<string, object?>
        {
            ["SkillID_Pk"] = skill.SkillId,
            ["SkillName"] = skill.SkillName,
            ["SortOrder"] = skill.SortOrder,
            ["RowVersion"] = skill.RowVersion,
            ["UpdatedBy"] = updatedBy
        }, cancellationToken: cancellationToken);

    public Task<SpExecutionResult> DeleteAsync(int skillId, int? deletedBy, CancellationToken cancellationToken = default) =>
        _db.ExecuteAsync("dbo.usp_Skills_Delete", new Dictionary<string, object?>
        {
            ["SkillID_Pk"] = skillId,
            ["DeletedBy"] = deletedBy
        }, cancellationToken: cancellationToken);

    public async Task<IReadOnlyList<SkillSearchResultDto>> SearchAsync(SearchSkillRequest request, CancellationToken cancellationToken = default)
    {
        var (_, rows) = await _db.QueryAsync<SkillSearchResultDto>("dbo.usp_Skills_Search", new Dictionary<string, object?>
        {
            ["PortfolioProfileID_Fk"] = request.PortfolioProfileId,
            ["SearchTerm"] = request.SearchTerm,
            ["MinProficiencyLevel"] = request.MinProficiencyLevel
        }, cancellationToken).ConfigureAwait(false);
        return rows;
    }

    public async Task<PagedResult<SkillDto>> GetPagedAsync(GetSkillPagedRequest request, CancellationToken cancellationToken = default)
    {
        var (_, rows, total, pages) = await _db.QueryPagedAsync<SkillDto>("dbo.usp_Skills_GetPaged", new Dictionary<string, object?>
        {
            ["PortfolioProfileID_Fk"] = request.PortfolioProfileId,
            ["PageNumber"] = request.PageNumber,
            ["PageSize"] = request.PageSize,
            ["SearchTerm"] = request.SearchTerm
        }, cancellationToken).ConfigureAwait(false);
        return new PagedResult<SkillDto> { Items = rows, PageNumber = request.PageNumber, PageSize = request.PageSize, TotalRecords = total, TotalPages = pages };
    }

    public Task<SpExecutionResult> InsertDetailAsync(SkillDetail detail, int? createdBy, CancellationToken cancellationToken = default) =>
        _db.ExecuteAsync("dbo.usp_SkillDetail_Insert", new Dictionary<string, object?>
        {
            ["SkillID_Fk"] = detail.SkillId,
            ["SkillDetailName"] = detail.SkillDetailName,
            ["ProficiencyLevel"] = detail.ProficiencyLevel,
            ["SortOrder"] = detail.SortOrder,
            ["CreatedBy"] = createdBy
        }, outIdKey: "OutSkillDetailID", cancellationToken);

    public Task<SpExecutionResult> UpdateDetailAsync(SkillDetail detail, int? updatedBy, CancellationToken cancellationToken = default) =>
        _db.ExecuteAsync("dbo.usp_SkillDetail_Update", new Dictionary<string, object?>
        {
            ["SkillDetailID_Pk"] = detail.SkillDetailId,
            ["SkillDetailName"] = detail.SkillDetailName,
            ["ProficiencyLevel"] = detail.ProficiencyLevel,
            ["SortOrder"] = detail.SortOrder,
            ["RowVersion"] = detail.RowVersion,
            ["UpdatedBy"] = updatedBy
        }, cancellationToken: cancellationToken);

    public Task<SpExecutionResult> DeleteDetailAsync(int skillDetailId, int? deletedBy, CancellationToken cancellationToken = default) =>
        _db.ExecuteAsync("dbo.usp_SkillDetail_Delete", new Dictionary<string, object?>
        {
            ["SkillDetailID_Pk"] = skillDetailId,
            ["DeletedBy"] = deletedBy
        }, cancellationToken: cancellationToken);
}

public sealed class AwardRepository : IAwardRepository
{
    private readonly Database _db;

    public AwardRepository(Database db) => _db = db;

    public async Task<Award?> GetByIdAsync(int awardId, CancellationToken cancellationToken = default)
    {
        var (_, entity) = await _db.QueryMultipleAsync(
            "dbo.usp_Awards_GetById",
            new Dictionary<string, object?> { ["AwardID_Pk"] = awardId, ["IncludeDeleted"] = false },
            async grid =>
            {
                var header = await grid.ReadFirstOrDefaultAsync().ConfigureAwait(false);
                if (header is null) return (Award?)null;
                var e = DapperColumnMapping.MapAward(header);
                e.Details = (await grid.ReadAsync().ConfigureAwait(false)).Select(DapperColumnMapping.MapAwardDetail).ToList();
                return e;
            },
            cancellationToken).ConfigureAwait(false);
        return entity;
    }

    public Task<SpExecutionResult> InsertAsync(Award award, int? createdBy, CancellationToken cancellationToken = default) =>
        _db.ExecuteAsync("dbo.usp_Awards_Insert", new Dictionary<string, object?>
        {
            ["PortfolioProfileID_Fk"] = award.PortfolioProfileId,
            ["AwardName"] = award.AwardName,
            ["StartDate"] = award.StartDate,
            ["EndDate"] = award.EndDate,
            ["SortOrder"] = award.SortOrder,
            ["CreatedBy"] = createdBy
        }, outIdKey: "OutAwardID", cancellationToken);

    public Task<SpExecutionResult> UpdateAsync(Award award, int? updatedBy, CancellationToken cancellationToken = default) =>
        _db.ExecuteAsync("dbo.usp_Awards_Update", new Dictionary<string, object?>
        {
            ["AwardID_Pk"] = award.AwardId,
            ["AwardName"] = award.AwardName,
            ["StartDate"] = award.StartDate,
            ["EndDate"] = award.EndDate,
            ["SortOrder"] = award.SortOrder,
            ["RowVersion"] = award.RowVersion,
            ["UpdatedBy"] = updatedBy
        }, cancellationToken: cancellationToken);

    public Task<SpExecutionResult> DeleteAsync(int awardId, int? deletedBy, CancellationToken cancellationToken = default) =>
        _db.ExecuteAsync("dbo.usp_Awards_Delete", new Dictionary<string, object?>
        {
            ["AwardID_Pk"] = awardId,
            ["DeletedBy"] = deletedBy
        }, cancellationToken: cancellationToken);

    public async Task<IReadOnlyList<AwardDto>> SearchAsync(SearchAwardRequest request, CancellationToken cancellationToken = default)
    {
        var (_, rows) = await _db.QueryAsync<AwardDto>("dbo.usp_Awards_Search", new Dictionary<string, object?>
        {
            ["PortfolioProfileID_Fk"] = request.PortfolioProfileId,
            ["SearchTerm"] = request.SearchTerm,
            ["StartDateFrom"] = request.StartDateFrom,
            ["StartDateTo"] = request.StartDateTo
        }, cancellationToken).ConfigureAwait(false);
        return rows;
    }

    public async Task<PagedResult<AwardDto>> GetPagedAsync(GetAwardPagedRequest request, CancellationToken cancellationToken = default)
    {
        var (_, rows, total, pages) = await _db.QueryPagedAsync<AwardDto>("dbo.usp_Awards_GetPaged", new Dictionary<string, object?>
        {
            ["PortfolioProfileID_Fk"] = request.PortfolioProfileId,
            ["PageNumber"] = request.PageNumber,
            ["PageSize"] = request.PageSize,
            ["SearchTerm"] = request.SearchTerm,
            ["StartDateFrom"] = request.StartDateFrom,
            ["StartDateTo"] = request.StartDateTo
        }, cancellationToken).ConfigureAwait(false);
        return new PagedResult<AwardDto> { Items = rows, PageNumber = request.PageNumber, PageSize = request.PageSize, TotalRecords = total, TotalPages = pages };
    }

    public Task<SpExecutionResult> InsertDetailAsync(AwardDetail detail, int? createdBy, CancellationToken cancellationToken = default) =>
        _db.ExecuteAsync("dbo.usp_AwardDetail_Insert", new Dictionary<string, object?>
        {
            ["AwardID_Fk"] = detail.AwardId,
            ["AwardDetailName"] = detail.AwardDetailName,
            ["SortOrder"] = detail.SortOrder,
            ["CreatedBy"] = createdBy
        }, outIdKey: "OutAwardDetailID", cancellationToken);

    public Task<SpExecutionResult> UpdateDetailAsync(AwardDetail detail, int? updatedBy, CancellationToken cancellationToken = default) =>
        _db.ExecuteAsync("dbo.usp_AwardDetail_Update", new Dictionary<string, object?>
        {
            ["AwardDetailID_Pk"] = detail.AwardDetailId,
            ["AwardDetailName"] = detail.AwardDetailName,
            ["SortOrder"] = detail.SortOrder,
            ["RowVersion"] = detail.RowVersion,
            ["UpdatedBy"] = updatedBy
        }, cancellationToken: cancellationToken);

    public Task<SpExecutionResult> DeleteDetailAsync(int awardDetailId, int? deletedBy, CancellationToken cancellationToken = default) =>
        _db.ExecuteAsync("dbo.usp_AwardDetail_Delete", new Dictionary<string, object?>
        {
            ["AwardDetailID_Pk"] = awardDetailId,
            ["DeletedBy"] = deletedBy
        }, cancellationToken: cancellationToken);
}

public sealed class CertificationRepository : ICertificationRepository
{
    private readonly Database _db;

    public CertificationRepository(Database db) => _db = db;

    public async Task<Certification?> GetByIdAsync(int certificationId, CancellationToken cancellationToken = default)
    {
        var (_, entity) = await _db.QueryMultipleAsync(
            "dbo.usp_Certifications_GetById",
            new Dictionary<string, object?> { ["CertificationID_Pk"] = certificationId, ["IncludeDeleted"] = false },
            async grid =>
            {
                var header = await grid.ReadFirstOrDefaultAsync().ConfigureAwait(false);
                if (header is null) return (Certification?)null;
                var e = DapperColumnMapping.MapCertification(header);
                e.Details = (await grid.ReadAsync().ConfigureAwait(false)).Select(DapperColumnMapping.MapCertificationDetail).ToList();
                return e;
            },
            cancellationToken).ConfigureAwait(false);
        return entity;
    }

    public Task<SpExecutionResult> InsertAsync(Certification certification, int? createdBy, CancellationToken cancellationToken = default) =>
        _db.ExecuteAsync("dbo.usp_Certifications_Insert", new Dictionary<string, object?>
        {
            ["PortfolioProfileID_Fk"] = certification.PortfolioProfileId,
            ["CertificationIssuerID_Fk"] = certification.CertificationIssuerId,
            ["CertificationName"] = certification.CertificationName,
            ["CredentialId"] = certification.CredentialId,
            ["CredentialUrl"] = certification.CredentialUrl,
            ["IssueDate"] = certification.IssueDate,
            ["ExpiryDate"] = certification.ExpiryDate,
            ["DoesNotExpire"] = certification.DoesNotExpire,
            ["SortOrder"] = certification.SortOrder,
            ["CreatedBy"] = createdBy
        }, outIdKey: "OutCertificationID", cancellationToken);

    public Task<SpExecutionResult> UpdateAsync(Certification certification, int? updatedBy, CancellationToken cancellationToken = default) =>
        _db.ExecuteAsync("dbo.usp_Certifications_Update", new Dictionary<string, object?>
        {
            ["CertificationID_Pk"] = certification.CertificationId,
            ["CertificationIssuerID_Fk"] = certification.CertificationIssuerId,
            ["CertificationName"] = certification.CertificationName,
            ["CredentialId"] = certification.CredentialId,
            ["CredentialUrl"] = certification.CredentialUrl,
            ["IssueDate"] = certification.IssueDate,
            ["ExpiryDate"] = certification.ExpiryDate,
            ["DoesNotExpire"] = certification.DoesNotExpire,
            ["SortOrder"] = certification.SortOrder,
            ["RowVersion"] = certification.RowVersion,
            ["UpdatedBy"] = updatedBy
        }, cancellationToken: cancellationToken);

    public Task<SpExecutionResult> DeleteAsync(int certificationId, int? deletedBy, CancellationToken cancellationToken = default) =>
        _db.ExecuteAsync("dbo.usp_Certifications_Delete", new Dictionary<string, object?>
        {
            ["CertificationID_Pk"] = certificationId,
            ["DeletedBy"] = deletedBy
        }, cancellationToken: cancellationToken);

    public async Task<IReadOnlyList<CertificationDto>> SearchAsync(SearchCertificationRequest request, CancellationToken cancellationToken = default)
    {
        var (_, rows) = await _db.QueryAsync<CertificationDto>("dbo.usp_Certifications_Search", new Dictionary<string, object?>
        {
            ["PortfolioProfileID_Fk"] = request.PortfolioProfileId,
            ["SearchTerm"] = request.SearchTerm,
            ["CertificationIssuerID_Fk"] = request.CertificationIssuerId,
            ["ActiveOnly"] = request.ActiveOnly
        }, cancellationToken).ConfigureAwait(false);
        return rows;
    }

    public async Task<PagedResult<CertificationDto>> GetPagedAsync(GetCertificationPagedRequest request, CancellationToken cancellationToken = default)
    {
        var (_, rows, total, pages) = await _db.QueryPagedAsync<CertificationDto>("dbo.usp_Certifications_GetPaged", new Dictionary<string, object?>
        {
            ["PortfolioProfileID_Fk"] = request.PortfolioProfileId,
            ["PageNumber"] = request.PageNumber,
            ["PageSize"] = request.PageSize,
            ["SearchTerm"] = request.SearchTerm,
            ["CertificationIssuerID_Fk"] = request.CertificationIssuerId,
            ["ActiveOnly"] = request.ActiveOnly
        }, cancellationToken).ConfigureAwait(false);
        return new PagedResult<CertificationDto> { Items = rows, PageNumber = request.PageNumber, PageSize = request.PageSize, TotalRecords = total, TotalPages = pages };
    }

    public Task<SpExecutionResult> InsertDetailAsync(CertificationDetail detail, int? createdBy, CancellationToken cancellationToken = default) =>
        _db.ExecuteAsync("dbo.usp_CertificationDetail_Insert", new Dictionary<string, object?>
        {
            ["CertificationID_Fk"] = detail.CertificationId,
            ["DetailText"] = detail.DetailText,
            ["SortOrder"] = detail.SortOrder,
            ["CreatedBy"] = createdBy
        }, outIdKey: "OutCertificationDetailID", cancellationToken);

    public Task<SpExecutionResult> UpdateDetailAsync(CertificationDetail detail, int? updatedBy, CancellationToken cancellationToken = default) =>
        _db.ExecuteAsync("dbo.usp_CertificationDetail_Update", new Dictionary<string, object?>
        {
            ["CertificationDetailID_Pk"] = detail.CertificationDetailId,
            ["DetailText"] = detail.DetailText,
            ["SortOrder"] = detail.SortOrder,
            ["RowVersion"] = detail.RowVersion,
            ["UpdatedBy"] = updatedBy
        }, cancellationToken: cancellationToken);

    public Task<SpExecutionResult> DeleteDetailAsync(int certificationDetailId, int? deletedBy, CancellationToken cancellationToken = default) =>
        _db.ExecuteAsync("dbo.usp_CertificationDetail_Delete", new Dictionary<string, object?>
        {
            ["CertificationDetailID_Pk"] = certificationDetailId,
            ["DeletedBy"] = deletedBy
        }, cancellationToken: cancellationToken);
}

public sealed class DocumentRepository : IDocumentRepository
{
    private readonly Database _db;

    public DocumentRepository(Database db) => _db = db;

    public async Task<Document?> GetByIdAsync(int documentId, CancellationToken cancellationToken = default)
    {
        var (_, row) = await _db.QuerySingleAsync<dynamic>("dbo.usp_Documents_GetById", new Dictionary<string, object?>
        {
            ["DocumentID_Pk"] = documentId,
            ["IncludeDeleted"] = false
        }, cancellationToken).ConfigureAwait(false);
        return row is null ? null : DapperColumnMapping.MapDocument(row);
    }

    public Task<SpExecutionResult> InsertAsync(Document document, int? createdBy, CancellationToken cancellationToken = default) =>
        _db.ExecuteAsync("dbo.usp_Documents_Insert", new Dictionary<string, object?>
        {
            ["PortfolioProfileID_Fk"] = document.PortfolioProfileId,
            ["DocumentTypeID_Fk"] = document.DocumentTypeId,
            ["DocumentTitle"] = document.DocumentTitle,
            ["FileName"] = document.FileName,
            ["FileExtension"] = document.FileExtension,
            ["FileSizeBytes"] = document.FileSizeBytes,
            ["StoragePath"] = document.StoragePath,
            ["MimeType"] = document.MimeType,
            ["IsPublic"] = document.IsPublic,
            ["IsDownloadable"] = document.IsDownloadable,
            ["VersionNumber"] = document.VersionNumber,
            ["SortOrder"] = document.SortOrder,
            ["CreatedBy"] = createdBy
        }, outIdKey: "OutDocumentID", cancellationToken);

    public Task<SpExecutionResult> UpdateAsync(Document document, int? updatedBy, CancellationToken cancellationToken = default) =>
        _db.ExecuteAsync("dbo.usp_Documents_Update", new Dictionary<string, object?>
        {
            ["DocumentID_Pk"] = document.DocumentId,
            ["DocumentTypeID_Fk"] = document.DocumentTypeId,
            ["DocumentTitle"] = document.DocumentTitle,
            ["FileName"] = document.FileName,
            ["FileExtension"] = document.FileExtension,
            ["FileSizeBytes"] = document.FileSizeBytes,
            ["StoragePath"] = document.StoragePath,
            ["MimeType"] = document.MimeType,
            ["IsPublic"] = document.IsPublic,
            ["IsDownloadable"] = document.IsDownloadable,
            ["VersionNumber"] = document.VersionNumber,
            ["SortOrder"] = document.SortOrder,
            ["RowVersion"] = document.RowVersion,
            ["UpdatedBy"] = updatedBy
        }, cancellationToken: cancellationToken);

    public Task<SpExecutionResult> DeleteAsync(int documentId, int? deletedBy, CancellationToken cancellationToken = default) =>
        _db.ExecuteAsync("dbo.usp_Documents_Delete", new Dictionary<string, object?>
        {
            ["DocumentID_Pk"] = documentId,
            ["DeletedBy"] = deletedBy
        }, cancellationToken: cancellationToken);

    public Task<SpExecutionResult> CreateNewVersionAsync(int sourceDocumentId, Document document, int? createdBy, CancellationToken cancellationToken = default) =>
        _db.ExecuteAsync("dbo.usp_Documents_CreateNewVersion", new Dictionary<string, object?>
        {
            ["SourceDocumentID_Pk"] = sourceDocumentId,
            ["FileName"] = document.FileName,
            ["FileExtension"] = document.FileExtension,
            ["FileSizeBytes"] = document.FileSizeBytes,
            ["StoragePath"] = document.StoragePath,
            ["MimeType"] = document.MimeType,
            ["CreatedBy"] = createdBy
        }, outIdKey: "OutDocumentID", cancellationToken);

    public async Task<IReadOnlyList<DocumentDto>> SearchAsync(SearchDocumentRequest request, CancellationToken cancellationToken = default)
    {
        var (_, rows) = await _db.QueryAsync<DocumentDto>("dbo.usp_Documents_Search", new Dictionary<string, object?>
        {
            ["PortfolioProfileID_Fk"] = request.PortfolioProfileId,
            ["SearchTerm"] = request.SearchTerm,
            ["DocumentTypeID_Fk"] = request.DocumentTypeId,
            ["IsPublic"] = request.IsPublic,
            ["FileExtension"] = request.FileExtension
        }, cancellationToken).ConfigureAwait(false);
        return rows;
    }

    public async Task<PagedResult<DocumentDto>> GetPagedAsync(GetDocumentPagedRequest request, CancellationToken cancellationToken = default)
    {
        var (_, rows, total, pages) = await _db.QueryPagedAsync<DocumentDto>("dbo.usp_Documents_GetPaged", new Dictionary<string, object?>
        {
            ["PortfolioProfileID_Fk"] = request.PortfolioProfileId,
            ["PageNumber"] = request.PageNumber,
            ["PageSize"] = request.PageSize,
            ["SearchTerm"] = request.SearchTerm,
            ["DocumentTypeID_Fk"] = request.DocumentTypeId,
            ["IsPublic"] = request.IsPublic
        }, cancellationToken).ConfigureAwait(false);
        return new PagedResult<DocumentDto> { Items = rows, PageNumber = request.PageNumber, PageSize = request.PageSize, TotalRecords = total, TotalPages = pages };
    }
}
