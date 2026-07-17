using Dapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Portfolio.Application.Abstractions.Persistence;
using Portfolio.Application.Common.Settings;
using Portfolio.Persistence.Common;
using Portfolio.Persistence.Connection;
using Portfolio.Persistence.Repositories;

namespace Portfolio.Persistence;

public static class DependencyInjection
{
    public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<DatabaseSettings>(configuration.GetSection(DatabaseSettings.SectionName));

        // Enable DateOnly/TimeOnly as Dapper stored-procedure parameters.
        SqlMapper.AddTypeHandler(new DateOnlyTypeHandler());
        SqlMapper.AddTypeHandler(new TimeOnlyTypeHandler());

        services.AddSingleton<StoredProcedureExecutor>();
        services.AddScoped<Database>();
        services.AddScoped<IDbConnectionFactory, SqlConnectionFactory>();

        services.AddScoped<IExperienceRepository, ExperienceRepository>();
        services.AddScoped<IProjectRepository, ProjectRepository>();
        services.AddScoped<IEducationRepository, EducationRepository>();
        services.AddScoped<ISkillRepository, SkillRepository>();
        services.AddScoped<IAwardRepository, AwardRepository>();
        services.AddScoped<ICertificationRepository, CertificationRepository>();
        services.AddScoped<IDocumentRepository, DocumentRepository>();
        services.AddScoped<ITestimonialRepository, TestimonialRepository>();
        services.AddScoped<IParagraphRepository, ParagraphRepository>();
        services.AddScoped<IReferenceRepository, ReferenceRepository>();
        services.AddScoped<IBlogRepository, BlogRepository>();
        services.AddScoped<IAnalyticsRepository, AnalyticsRepository>();
        services.AddScoped<IAuthRepository, AuthRepository>();
        services.AddScoped<ICompanyRepository, CompanyRepository>();
        services.AddScoped<IDeployDetailRepository, DeployDetailRepository>();
        services.AddScoped<IInstituteRepository, InstituteRepository>();
        services.AddScoped<IDegreeRepository, DegreeRepository>();
        services.AddScoped<IDegreeLevelRepository, DegreeLevelRepository>();
        services.AddScoped<ITechnologyRepository, TechnologyRepository>();
        services.AddScoped<ICertificationIssuerRepository, CertificationIssuerRepository>();
        services.AddScoped<IDocumentTypeRepository, DocumentTypeRepository>();
        services.AddScoped<ICourseRepository, CourseRepository>();

        return services;
    }
}
