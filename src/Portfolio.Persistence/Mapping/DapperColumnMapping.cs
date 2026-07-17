using Portfolio.Domain.Entities.Identity;
using Portfolio.Domain.Entities.Awards;
using Portfolio.Domain.Entities.Certifications;
using Portfolio.Domain.Entities.Documents;
using Portfolio.Domain.Entities.Education;
using Portfolio.Domain.Entities.Experience;
using Portfolio.Domain.Entities.Projects;
using Portfolio.Domain.Entities.Skills;

namespace Portfolio.Persistence.Mapping;

internal static class DapperColumnMapping
{
    public static Experience MapExperience(dynamic row)
    {
        var r = (IDictionary<string, object>)row;
        return new Experience
        {
            ExperienceId = GetInt(r, "ExperienceID_Pk"),
            PortfolioProfileId = GetInt(r, "PortfolioProfileID_Fk"),
            Designation = GetString(r, "Designation"),
            CompanyId = GetNullableInt(r, "CompanyID_Fk"),
            CompanyName = GetString(r, "CompanyName"),
            DeployDetailId = GetNullableInt(r, "DeployDetailID_Fk"),
            DeployDetailName = GetString(r, "DeployDetailName"),
            DeployedTo = GetString(r, "DeployedTo"),
            StartDate = GetDate(r, "StartDate"),
            EndDate = GetNullableDate(r, "EndDate"),
            SortOrder = GetInt(r, "SortOrder"),
            IsCurrent = GetBool(r, "IsCurrent"),
            CreatedAt = GetDateTime(r, "CreatedAt"),
            CreatedBy = GetNullableInt(r, "CreatedBy"),
            UpdatedAt = GetDateTime(r, "UpdatedAt"),
            UpdatedBy = GetNullableInt(r, "UpdatedBy"),
            IsDeleted = GetBool(r, "IsDeleted"),
            RowVersion = GetBytes(r, "RowVersion")
        };
    }

    public static ExperienceDetail MapExperienceDetail(dynamic row)
    {
        var r = (IDictionary<string, object>)row;
        return new ExperienceDetail
        {
            ExperienceDetailId = GetInt(r, "ExperienceDetailID_Pk"),
            ExperienceId = GetInt(r, "ExperienceID_Fk"),
            ExperienceDetailName = GetString(r, "ExperienceDetailName"),
            SortOrder = GetInt(r, "SortOrder"),
            RowVersion = GetBytes(r, "RowVersion")
        };
    }

    public static Project MapProject(dynamic row)
    {
        var r = (IDictionary<string, object>)row;
        return new Project
        {
            ProjectId = GetInt(r, "ProjectID_Pk"),
            PortfolioProfileId = GetInt(r, "PortfolioProfileID_Fk"),
            ProjectName = GetString(r, "ProjectName"),
            ProjectSummary = GetString(r, "ProjectSummary"),
            Practice = GetString(r, "Practice"),
            CompanyId = GetNullableInt(r, "CompanyID_Fk"),
            CompanyName = GetString(r, "CompanyName"),
            CourseId = GetNullableInt(r, "CourseID_Fk"),
            CourseName = GetString(r, "CourseName"),
            StartDate = GetDate(r, "StartDate"),
            EndDate = GetNullableDate(r, "EndDate"),
            SortOrder = GetInt(r, "SortOrder"),
            CreatedAt = GetDateTime(r, "CreatedAt"),
            CreatedBy = GetNullableInt(r, "CreatedBy"),
            UpdatedAt = GetDateTime(r, "UpdatedAt"),
            UpdatedBy = GetNullableInt(r, "UpdatedBy"),
            IsDeleted = GetBool(r, "IsDeleted"),
            RowVersion = GetBytes(r, "RowVersion")
        };
    }

    public static ProjectDetail MapProjectDetail(dynamic row)
    {
        var r = (IDictionary<string, object>)row;
        return new ProjectDetail
        {
            ProjectDetailId = GetInt(r, "ProjectDetailID_Pk"),
            ProjectId = GetInt(r, "ProjectID_Fk"),
            ProjectDetailName = GetString(r, "ProjectDetailName"),
            SortOrder = GetInt(r, "SortOrder"),
            RowVersion = GetBytes(r, "RowVersion")
        };
    }

    public static ProjectTechnology MapProjectTechnology(dynamic row)
    {
        var r = (IDictionary<string, object>)row;
        return new ProjectTechnology
        {
            ProjectTechnologyId = GetInt(r, "ProjectTechnologyID_Pk"),
            ProjectId = GetInt(r, "ProjectID_Fk"),
            TechnologyId = GetInt(r, "TechnologyID_Pk"),
            TechnologyName = GetString(r, "TechnologyName"),
            RowVersion = GetBytes(r, "RowVersion")
        };
    }

    public static EducationRecord MapEducation(dynamic row)
    {
        var r = (IDictionary<string, object>)row;
        return new EducationRecord
        {
            EducationId = GetInt(r, "EducationID_Pk"),
            PortfolioProfileId = GetInt(r, "PortfolioProfileID_Fk"),
            DegreeLevelId = GetInt(r, "DegreeLevelID_Fk"),
            DegreeLevelName = GetString(r, "DegreeLevelName"),
            DegreePrefix = GetString(r, "DegreePrefix"),
            DegreeId = GetInt(r, "DegreeID_Fk"),
            DegreeName = GetString(r, "DegreeName"),
            InstituteId = GetInt(r, "InstituteID_Fk"),
            InstituteName = GetString(r, "InstituteName"),
            Location = GetString(r, "Location"),
            StartDate = GetDate(r, "StartDate"),
            EndDate = GetNullableDate(r, "EndDate"),
            Gpa = GetNullableDecimal(r, "GPA"),
            Cgpa = GetNullableDecimal(r, "CGPA"),
            SortOrder = GetInt(r, "SortOrder"),
            CreatedAt = GetDateTime(r, "CreatedAt"),
            CreatedBy = GetNullableInt(r, "CreatedBy"),
            UpdatedAt = GetDateTime(r, "UpdatedAt"),
            UpdatedBy = GetNullableInt(r, "UpdatedBy"),
            IsDeleted = GetBool(r, "IsDeleted"),
            RowVersion = GetBytes(r, "RowVersion")
        };
    }

    public static CourseRecord MapCourse(dynamic row)
    {
        var r = (IDictionary<string, object>)row;
        return new CourseRecord
        {
            CourseId = GetInt(r, "CourseID_Pk"),
            InstituteId = GetNullableInt(r, "InstituteID_Fk"),
            CourseName = GetString(r, "CourseName"),
            SortOrder = GetInt(r, "SortOrder")
        };
    }

    public static Skill MapSkill(dynamic row)
    {
        var r = (IDictionary<string, object>)row;
        return new Skill
        {
            SkillId = GetInt(r, "SkillID_Pk"),
            PortfolioProfileId = GetInt(r, "PortfolioProfileID_Fk"),
            SkillName = GetString(r, "SkillName"),
            SortOrder = GetInt(r, "SortOrder"),
            CreatedAt = GetDateTime(r, "CreatedAt"),
            CreatedBy = GetNullableInt(r, "CreatedBy"),
            UpdatedAt = GetDateTime(r, "UpdatedAt"),
            UpdatedBy = GetNullableInt(r, "UpdatedBy"),
            IsDeleted = GetBool(r, "IsDeleted"),
            RowVersion = GetBytes(r, "RowVersion")
        };
    }

    public static SkillDetail MapSkillDetail(dynamic row)
    {
        var r = (IDictionary<string, object>)row;
        return new SkillDetail
        {
            SkillDetailId = GetInt(r, "SkillDetailID_Pk"),
            SkillId = GetInt(r, "SkillID_Fk"),
            SkillDetailName = GetString(r, "SkillDetailName"),
            ProficiencyLevel = GetNullableByte(r, "ProficiencyLevel"),
            SortOrder = GetInt(r, "SortOrder"),
            RowVersion = GetBytes(r, "RowVersion")
        };
    }

    public static Award MapAward(dynamic row)
    {
        var r = (IDictionary<string, object>)row;
        return new Award
        {
            AwardId = GetInt(r, "AwardID_Pk"),
            PortfolioProfileId = GetInt(r, "PortfolioProfileID_Fk"),
            AwardName = GetString(r, "AwardName"),
            StartDate = GetDate(r, "StartDate"),
            EndDate = GetNullableDate(r, "EndDate"),
            SortOrder = GetInt(r, "SortOrder"),
            CreatedAt = GetDateTime(r, "CreatedAt"),
            CreatedBy = GetNullableInt(r, "CreatedBy"),
            UpdatedAt = GetDateTime(r, "UpdatedAt"),
            UpdatedBy = GetNullableInt(r, "UpdatedBy"),
            IsDeleted = GetBool(r, "IsDeleted"),
            RowVersion = GetBytes(r, "RowVersion")
        };
    }

    public static AwardDetail MapAwardDetail(dynamic row)
    {
        var r = (IDictionary<string, object>)row;
        return new AwardDetail
        {
            AwardDetailId = GetInt(r, "AwardDetailID_Pk"),
            AwardId = GetInt(r, "AwardID_Fk"),
            AwardDetailName = GetString(r, "AwardDetailName"),
            SortOrder = GetInt(r, "SortOrder"),
            RowVersion = GetBytes(r, "RowVersion")
        };
    }

    public static Certification MapCertification(dynamic row)
    {
        var r = (IDictionary<string, object>)row;
        return new Certification
        {
            CertificationId = GetInt(r, "CertificationID_Pk"),
            PortfolioProfileId = GetInt(r, "PortfolioProfileID_Fk"),
            CertificationIssuerId = GetInt(r, "CertificationIssuerID_Fk"),
            IssuerName = GetString(r, "IssuerName"),
            IssuerWebsite = GetString(r, "IssuerWebsite"),
            CertificationName = GetString(r, "CertificationName"),
            CredentialId = GetString(r, "CredentialId"),
            CredentialUrl = GetString(r, "CredentialUrl"),
            IssueDate = GetDate(r, "IssueDate"),
            ExpiryDate = GetNullableDate(r, "ExpiryDate"),
            DoesNotExpire = GetBool(r, "DoesNotExpire"),
            SortOrder = GetInt(r, "SortOrder"),
            CreatedAt = GetDateTime(r, "CreatedAt"),
            CreatedBy = GetNullableInt(r, "CreatedBy"),
            UpdatedAt = GetDateTime(r, "UpdatedAt"),
            UpdatedBy = GetNullableInt(r, "UpdatedBy"),
            IsDeleted = GetBool(r, "IsDeleted"),
            RowVersion = GetBytes(r, "RowVersion")
        };
    }

    public static CertificationDetail MapCertificationDetail(dynamic row)
    {
        var r = (IDictionary<string, object>)row;
        return new CertificationDetail
        {
            CertificationDetailId = GetInt(r, "CertificationDetailID_Pk"),
            CertificationId = GetInt(r, "CertificationID_Fk"),
            DetailText = GetString(r, "DetailText"),
            SortOrder = GetInt(r, "SortOrder"),
            RowVersion = GetBytes(r, "RowVersion")
        };
    }

    public static Document MapDocument(dynamic row)
    {
        var r = (IDictionary<string, object>)row;
        return new Document
        {
            DocumentId = GetInt(r, "DocumentID_Pk"),
            PortfolioProfileId = GetInt(r, "PortfolioProfileID_Fk"),
            DocumentTypeId = GetInt(r, "DocumentTypeID_Fk"),
            DocumentType = GetString(r, "DocumentType"),
            DocumentTitle = GetString(r, "DocumentTitle"),
            FileName = GetString(r, "FileName"),
            FileExtension = GetString(r, "FileExtension"),
            FileSizeBytes = GetNullableLong(r, "FileSizeBytes"),
            StoragePath = GetString(r, "StoragePath"),
            MimeType = GetString(r, "MimeType"),
            IsPublic = GetBool(r, "IsPublic"),
            IsDownloadable = GetBool(r, "IsDownloadable"),
            VersionNumber = GetInt(r, "VersionNumber"),
            SortOrder = GetInt(r, "SortOrder"),
            CreatedAt = GetDateTime(r, "CreatedAt"),
            CreatedBy = GetNullableInt(r, "CreatedBy"),
            UpdatedAt = GetDateTime(r, "UpdatedAt"),
            UpdatedBy = GetNullableInt(r, "UpdatedBy"),
            IsDeleted = GetBool(r, "IsDeleted"),
            RowVersion = GetBytes(r, "RowVersion")
        };
    }

    public static UserAccount MapUserAccount(dynamic row)
    {
        var r = (IDictionary<string, object>)row;
        return new UserAccount
        {
            UserId = GetInt(r, "UserID_Pk"),
            UserName = GetString(r, "UserName"),
            Email = GetString(r, "Email"),
            DisplayName = GetNullableString(r, "DisplayName"),
            PasswordHash = GetString(r, "PasswordHash"),
            IsActive = GetBool(r, "IsActive"),
            IsDeleted = GetBool(r, "IsDeleted")
        };
    }

    public static RefreshTokenRecord MapRefreshToken(dynamic row)
    {
        var r = (IDictionary<string, object>)row;
        return new RefreshTokenRecord
        {
            RefreshTokenId = GetInt(r, "RefreshTokenID_Pk"),
            UserId = GetInt(r, "UserID_Fk"),
            TokenHash = GetString(r, "TokenHash"),
            JwtId = GetString(r, "JwtId"),
            ExpiresAt = GetDateTime(r, "ExpiresAt"),
            RevokedAt = GetNullableDateTime(r, "RevokedAt"),
            ReplacedByTokenHash = GetNullableString(r, "ReplacedByTokenHash"),
            UserName = GetString(r, "UserName"),
            IsActive = GetBool(r, "IsActive"),
            IsDeleted = GetBool(r, "IsDeleted")
        };
    }

    private static int GetInt(IDictionary<string, object> row, string key)
        => row.TryGetValue(key, out var value) && value is not DBNull ? Convert.ToInt32(value) : 0;

    private static int? GetNullableInt(IDictionary<string, object> row, string key)
        => row.TryGetValue(key, out var value) && value is not DBNull ? Convert.ToInt32(value) : null;

    private static long? GetNullableLong(IDictionary<string, object> row, string key)
        => row.TryGetValue(key, out var value) && value is not DBNull ? Convert.ToInt64(value) : null;

    private static byte? GetNullableByte(IDictionary<string, object> row, string key)
        => row.TryGetValue(key, out var value) && value is not DBNull ? Convert.ToByte(value) : null;

    private static decimal? GetNullableDecimal(IDictionary<string, object> row, string key)
        => row.TryGetValue(key, out var value) && value is not DBNull ? Convert.ToDecimal(value) : null;

    private static bool GetBool(IDictionary<string, object> row, string key)
        => row.TryGetValue(key, out var value) && value is not DBNull && Convert.ToBoolean(value);

    private static string? GetNullableString(IDictionary<string, object> row, string key)
        => row.TryGetValue(key, out var value) && value is not DBNull ? Convert.ToString(value) : null;

    private static string GetString(IDictionary<string, object> row, string key)
        => row.TryGetValue(key, out var value) && value is not DBNull ? Convert.ToString(value) ?? string.Empty : string.Empty;

    private static DateOnly GetDate(IDictionary<string, object> row, string key)
        => row.TryGetValue(key, out var value) && value is not DBNull
            ? DateOnly.FromDateTime(Convert.ToDateTime(value))
            : default;

    private static DateOnly? GetNullableDate(IDictionary<string, object> row, string key)
        => row.TryGetValue(key, out var value) && value is not DBNull
            ? DateOnly.FromDateTime(Convert.ToDateTime(value))
            : null;

    private static DateTime GetDateTime(IDictionary<string, object> row, string key)
        => row.TryGetValue(key, out var value) && value is not DBNull
            ? Convert.ToDateTime(value)
            : default;

    private static DateTime? GetNullableDateTime(IDictionary<string, object> row, string key)
        => row.TryGetValue(key, out var value) && value is not DBNull
            ? Convert.ToDateTime(value)
            : null;

    private static byte[] GetBytes(IDictionary<string, object> row, string key)
        => row.TryGetValue(key, out var value) && value is not DBNull
            ? (byte[])value
            : [];
}
