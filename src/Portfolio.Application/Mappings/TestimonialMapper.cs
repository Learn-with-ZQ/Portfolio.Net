using Portfolio.Application.Common.Models;
using Portfolio.Application.DTOs.Testimonials;
using Portfolio.Domain.Entities.Testimonials;

namespace Portfolio.Application.Mappings;

public static class TestimonialMapper
{
    public static string StatusName(int status) => status switch
    {
        1 => "Pending",
        2 => "Approved",
        3 => "Rejected",
        _ => "Unknown"
    };

    public static Testimonial ToEntity(SubmitTestimonialRequest request) => new()
    {
        PortfolioProfileId = request.PortfolioProfileId,
        AuthorName = request.AuthorName,
        AuthorTitle = request.AuthorTitle,
        AuthorCompany = request.AuthorCompany,
        AuthorEmail = request.AuthorEmail,
        Relationship = request.Relationship,
        Rating = request.Rating,
        Content = request.Content,
        LinkedInUrl = request.LinkedInUrl
    };

    public static TestimonialDetailDto ToDetailDto(Testimonial entity) => new()
    {
        TestimonialId = entity.TestimonialId,
        PortfolioProfileId = entity.PortfolioProfileId,
        AuthorName = entity.AuthorName,
        AuthorTitle = entity.AuthorTitle,
        AuthorCompany = entity.AuthorCompany,
        AuthorEmail = entity.AuthorEmail,
        Relationship = entity.Relationship,
        Rating = entity.Rating,
        Content = entity.Content,
        LinkedInUrl = entity.LinkedInUrl,
        Status = entity.Status,
        StatusName = StatusName(entity.Status),
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
}
