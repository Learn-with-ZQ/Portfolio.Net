namespace Portfolio.Application.DTOs.Testimonials;

public sealed class TestimonialDto
{
    public int TestimonialId { get; init; }
    public string AuthorName { get; init; } = string.Empty;
    public string? AuthorTitle { get; init; }
    public string? AuthorCompany { get; init; }
    public string? Relationship { get; init; }
    public int? Rating { get; init; }
    public string Content { get; init; } = string.Empty;
    public string? LinkedInUrl { get; init; }
    public int Status { get; init; }
    public string StatusName { get; init; } = string.Empty;
    public int SortOrder { get; init; }
    public DateTime CreatedAt { get; init; }
}
