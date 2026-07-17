using Portfolio.Application.Common.Models;

namespace Portfolio.Application.DTOs.Blog;

public sealed class GetBlogPagedRequest : PagedRequestBase
{
    public string? Category { get; set; }
    public bool? IsPublished { get; set; }
}
