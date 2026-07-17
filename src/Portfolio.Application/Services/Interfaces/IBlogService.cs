using Portfolio.Application.Common.Models;
using Portfolio.Application.Common.Results;
using Portfolio.Application.DTOs.Blog;

namespace Portfolio.Application.Services.Interfaces;

public interface IBlogService
{
    Task<ServiceResult<BlogPostDetailDto>> GetByIdAsync(int blogPostId, CancellationToken cancellationToken = default);
    Task<ServiceResult<BlogPostDetailDto>> GetBySlugAsync(int portfolioProfileId, string slug, CancellationToken cancellationToken = default);
    Task<ServiceResult<CommandResultDto>> CreateAsync(CreateBlogPostRequest request, CancellationToken cancellationToken = default);
    Task<ServiceResult<CommandResultDto>> UpdateAsync(UpdateBlogPostRequest request, CancellationToken cancellationToken = default);
    Task<ServiceResult> PublishAsync(int blogPostId, CancellationToken cancellationToken = default);
    Task<ServiceResult> UnpublishAsync(int blogPostId, CancellationToken cancellationToken = default);
    Task<ServiceResult> DeleteAsync(int blogPostId, CancellationToken cancellationToken = default);
    Task<ServiceResult<PagedResult<BlogPostDto>>> GetPagedAsync(GetBlogPagedRequest request, CancellationToken cancellationToken = default);
}
