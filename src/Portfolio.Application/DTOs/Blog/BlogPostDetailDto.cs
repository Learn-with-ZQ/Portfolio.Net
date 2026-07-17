using Portfolio.Application.Common.Models;

namespace Portfolio.Application.DTOs.Blog;

public sealed class BlogPostDetailDto
{
    public int BlogPostId { get; init; }
    public int PortfolioProfileId { get; init; }
    public string Title { get; init; } = string.Empty;
    public string Slug { get; init; } = string.Empty;
    public string? Summary { get; init; }
    public string ContentMarkdown { get; init; } = string.Empty;
    public string? Category { get; init; }
    public string? Tags { get; init; }
    public string? CoverImagePath { get; init; }
    public bool IsPublished { get; init; }
    public DateTime? PublishedAt { get; init; }
    public int? ReadTimeMinutes { get; init; }
    public int SortOrder { get; init; }
    public byte[] RowVersion { get; init; } = [];
    public AuditInfoDto Audit { get; init; } = new();
}
