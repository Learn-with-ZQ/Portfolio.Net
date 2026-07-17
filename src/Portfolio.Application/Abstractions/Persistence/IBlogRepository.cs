using Portfolio.Application.Common.Models;
using Portfolio.Application.DTOs.Blog;
using Portfolio.Domain.Entities.Blog;

namespace Portfolio.Application.Abstractions.Persistence;

public interface IBlogRepository
{
    Task<BlogPost?> GetByIdAsync(int blogPostId, CancellationToken cancellationToken = default);
    Task<BlogPost?> GetBySlugAsync(int portfolioProfileId, string slug, CancellationToken cancellationToken = default);
    Task<SpExecutionResult> InsertAsync(BlogPost post, int? createdBy, CancellationToken cancellationToken = default);
    Task<SpExecutionResult> UpdateAsync(BlogPost post, int? updatedBy, CancellationToken cancellationToken = default);
    Task<SpExecutionResult> SetPublishedAsync(int blogPostId, bool isPublished, int? updatedBy, CancellationToken cancellationToken = default);
    Task<SpExecutionResult> DeleteAsync(int blogPostId, int? deletedBy, CancellationToken cancellationToken = default);
    Task<PagedResult<BlogPostDto>> GetPagedAsync(GetBlogPagedRequest request, CancellationToken cancellationToken = default);
}
