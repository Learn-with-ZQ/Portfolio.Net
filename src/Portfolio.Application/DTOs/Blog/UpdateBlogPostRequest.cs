namespace Portfolio.Application.DTOs.Blog;

public sealed class UpdateBlogPostRequest
{
    public int BlogPostId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public string? Summary { get; set; }
    public string ContentMarkdown { get; set; } = string.Empty;
    public string? Category { get; set; }
    public string? Tags { get; set; }
    public string? CoverImagePath { get; set; }
    public int? ReadTimeMinutes { get; set; }
    public int SortOrder { get; set; }
    public byte[] RowVersion { get; set; } = [];
}
