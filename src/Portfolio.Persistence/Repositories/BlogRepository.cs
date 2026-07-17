using Portfolio.Application.Abstractions.Persistence;
using Portfolio.Application.Common.Models;
using Portfolio.Application.DTOs.Blog;
using Portfolio.Domain.Entities.Blog;
using Portfolio.Persistence.Common;

namespace Portfolio.Persistence.Repositories;

public sealed class BlogRepository : IBlogRepository
{
    private readonly Database _db;
    public BlogRepository(Database db) => _db = db;

    public async Task<BlogPost?> GetByIdAsync(int blogPostId, CancellationToken cancellationToken = default)
    {
        var (_, item) = await _db.QuerySingleAsync<BlogPost>("dbo.usp_Blog_GetById", new Dictionary<string, object?>
        {
            ["BlogPostID_Pk"] = blogPostId,
            ["IncludeDeleted"] = false
        }, cancellationToken).ConfigureAwait(false);
        return item;
    }

    public async Task<BlogPost?> GetBySlugAsync(int portfolioProfileId, string slug, CancellationToken cancellationToken = default)
    {
        var (_, item) = await _db.QuerySingleAsync<BlogPost>("dbo.usp_Blog_GetBySlug", new Dictionary<string, object?>
        {
            ["PortfolioProfileID_Fk"] = portfolioProfileId,
            ["Slug"] = slug
        }, cancellationToken).ConfigureAwait(false);
        return item;
    }

    public Task<SpExecutionResult> InsertAsync(BlogPost post, int? createdBy, CancellationToken cancellationToken = default) =>
        _db.ExecuteAsync("dbo.usp_Blog_Insert", new Dictionary<string, object?>
        {
            ["PortfolioProfileID_Fk"] = post.PortfolioProfileId,
            ["Title"] = post.Title,
            ["Slug"] = post.Slug,
            ["Summary"] = post.Summary,
            ["ContentMarkdown"] = post.ContentMarkdown,
            ["Category"] = post.Category,
            ["Tags"] = post.Tags,
            ["CoverImagePath"] = post.CoverImagePath,
            ["ReadTimeMinutes"] = post.ReadTimeMinutes,
            ["SortOrder"] = post.SortOrder,
            ["IsPublished"] = post.IsPublished,
            ["CreatedBy"] = createdBy
        }, outIdKey: "OutBlogPostID", cancellationToken);

    public Task<SpExecutionResult> UpdateAsync(BlogPost post, int? updatedBy, CancellationToken cancellationToken = default) =>
        _db.ExecuteAsync("dbo.usp_Blog_Update", new Dictionary<string, object?>
        {
            ["BlogPostID_Pk"] = post.BlogPostId,
            ["PortfolioProfileID_Fk"] = post.PortfolioProfileId,
            ["Title"] = post.Title,
            ["Slug"] = post.Slug,
            ["Summary"] = post.Summary,
            ["ContentMarkdown"] = post.ContentMarkdown,
            ["Category"] = post.Category,
            ["Tags"] = post.Tags,
            ["CoverImagePath"] = post.CoverImagePath,
            ["ReadTimeMinutes"] = post.ReadTimeMinutes,
            ["SortOrder"] = post.SortOrder,
            ["RowVersion"] = post.RowVersion,
            ["UpdatedBy"] = updatedBy
        }, cancellationToken: cancellationToken);

    public Task<SpExecutionResult> SetPublishedAsync(int blogPostId, bool isPublished, int? updatedBy, CancellationToken cancellationToken = default) =>
        _db.ExecuteAsync("dbo.usp_Blog_SetPublished", new Dictionary<string, object?>
        {
            ["BlogPostID_Pk"] = blogPostId,
            ["IsPublished"] = isPublished,
            ["UpdatedBy"] = updatedBy
        }, cancellationToken: cancellationToken);

    public Task<SpExecutionResult> DeleteAsync(int blogPostId, int? deletedBy, CancellationToken cancellationToken = default) =>
        _db.ExecuteAsync("dbo.usp_Blog_Delete", new Dictionary<string, object?>
        {
            ["BlogPostID_Pk"] = blogPostId,
            ["DeletedBy"] = deletedBy
        }, cancellationToken: cancellationToken);

    public async Task<PagedResult<BlogPostDto>> GetPagedAsync(GetBlogPagedRequest request, CancellationToken cancellationToken = default)
    {
        var (_, rows, total, pages) = await _db.QueryPagedAsync<BlogPostDto>("dbo.usp_Blog_GetPaged", new Dictionary<string, object?>
        {
            ["PortfolioProfileID_Fk"] = request.PortfolioProfileId,
            ["PageNumber"] = request.PageNumber,
            ["PageSize"] = request.PageSize,
            ["SearchTerm"] = request.SearchTerm,
            ["Category"] = request.Category,
            ["IsPublished"] = request.IsPublished
        }, cancellationToken).ConfigureAwait(false);

        return new PagedResult<BlogPostDto>
        {
            Items = rows,
            PageNumber = request.PageNumber,
            PageSize = request.PageSize,
            TotalRecords = total,
            TotalPages = pages
        };
    }
}
