using Portfolio.Application.Common.Models;

namespace Portfolio.Application.DTOs.Blog;

public sealed class CreateBlogPostRequest : ModuleRequestBase
{
    public string Title { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public string? Summary { get; set; }
    public string ContentMarkdown { get; set; } = string.Empty;
    public string? Category { get; set; }
    public string? Tags { get; set; }
    public string? CoverImagePath { get; set; }
    public bool IsPublished { get; set; }
    public int? ReadTimeMinutes { get; set; }
    public int SortOrder { get; set; }
}
