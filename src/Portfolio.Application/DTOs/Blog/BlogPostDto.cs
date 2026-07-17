namespace Portfolio.Application.DTOs.Blog;

public sealed class BlogPostDto
{
    public int BlogPostId { get; init; }
    public string Title { get; init; } = string.Empty;
    public string Slug { get; init; } = string.Empty;
    public string? Summary { get; init; }
    public string? Category { get; init; }
    public string? Tags { get; init; }
    public string? CoverImagePath { get; init; }
    public bool IsPublished { get; init; }
    public DateTime? PublishedAt { get; init; }
    public int? ReadTimeMinutes { get; init; }
    public int SortOrder { get; init; }
}
