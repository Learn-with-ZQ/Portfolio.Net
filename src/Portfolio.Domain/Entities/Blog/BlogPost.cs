using Portfolio.Domain.Common;

namespace Portfolio.Domain.Entities.Blog;

public sealed class BlogPost : AuditableEntity
{
    public int BlogPostId { get; set; }
    public int PortfolioProfileId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public string? Summary { get; set; }
    public string ContentMarkdown { get; set; } = string.Empty;
    /// <summary>.NET, Angular, SQL Server, System Design, Data Science, Architecture…</summary>
    public string? Category { get; set; }
    /// <summary>Comma-separated tags.</summary>
    public string? Tags { get; set; }
    public string? CoverImagePath { get; set; }
    public bool IsPublished { get; set; }
    public DateTime? PublishedAt { get; set; }
    public int? ReadTimeMinutes { get; set; }
    public int SortOrder { get; set; }
}
