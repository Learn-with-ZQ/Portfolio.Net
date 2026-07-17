using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Portfolio.Application.Services;
using Portfolio.Application.Services.Interfaces;
using Portfolio.Application.Validators;

namespace Portfolio.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddValidatorsFromAssemblyContaining<CreateExperienceRequestValidator>();

        services.AddScoped<IExperienceService, ExperienceService>();
        services.AddScoped<IProjectService, ProjectService>();
        services.AddScoped<IEducationService, EducationService>();
        services.AddScoped<ISkillService, SkillService>();
        services.AddScoped<IAwardService, AwardService>();
        services.AddScoped<ICertificationService, CertificationService>();
        services.AddScoped<IDocumentService, DocumentService>();
        services.AddScoped<ITestimonialService, TestimonialService>();
        services.AddScoped<IParagraphService, ParagraphService>();
        services.AddScoped<IReferenceService, ReferenceService>();
        services.AddScoped<IBlogService, BlogService>();
        services.AddScoped<IAnalyticsService, AnalyticsService>();
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<ICompanyService, CompanyService>();
        services.AddScoped<IDeployDetailService, DeployDetailService>();
        services.AddScoped<IInstituteService, InstituteService>();
        services.AddScoped<IDegreeService, DegreeService>();
        services.AddScoped<IDegreeLevelService, DegreeLevelService>();
        services.AddScoped<ITechnologyService, TechnologyService>();
        services.AddScoped<ICertificationIssuerService, CertificationIssuerService>();
        services.AddScoped<IDocumentTypeService, DocumentTypeService>();
        services.AddScoped<ICourseService, CourseService>();

        return services;
    }
}
