using Portfolio.Domain.Common;

namespace Portfolio.Domain.Entities.Testimonials;

public sealed class Testimonial : AuditableEntity
{
    public int TestimonialId { get; set; }
    public int PortfolioProfileId { get; set; }
    public string AuthorName { get; set; } = string.Empty;
    public string? AuthorTitle { get; set; }
    public string? AuthorCompany { get; set; }
    public string? AuthorEmail { get; set; }
    public string? Relationship { get; set; }
    public int? Rating { get; set; }
    public string Content { get; set; } = string.Empty;
    public string? LinkedInUrl { get; set; }

    /// <summary>1 = Pending, 2 = Approved (published), 3 = Rejected.</summary>
    public int Status { get; set; } = 1;
    public int SortOrder { get; set; }
}
