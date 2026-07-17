using Portfolio.Application.Common.Models;

namespace Portfolio.Application.DTOs.Testimonials;

/// <summary>Public submission — always created in the Pending state.</summary>
public sealed class SubmitTestimonialRequest : ModuleRequestBase
{
    public string AuthorName { get; set; } = string.Empty;
    public string? AuthorTitle { get; set; }
    public string? AuthorCompany { get; set; }
    public string? AuthorEmail { get; set; }
    public string? Relationship { get; set; }
    public int? Rating { get; set; }
    public string Content { get; set; } = string.Empty;
    public string? LinkedInUrl { get; set; }
}
