using Portfolio.Application.Common.Models;
using Portfolio.Application.DTOs.Blog;
using Portfolio.Domain.Entities.Blog;

namespace Portfolio.Application.Mappings;

public static class BlogMapper
{
    public static BlogPost ToEntity(CreateBlogPostRequest request) => new()
    {
        PortfolioProfileId = request.PortfolioProfileId,
        Title = request.Title,
        Slug = request.Slug,
        Summary = request.Summary,
        ContentMarkdown = request.ContentMarkdown,
        Category = request.Category,
        Tags = request.Tags,
        CoverImagePath = request.CoverImagePath,
        IsPublished = request.IsPublished,
        ReadTimeMinutes = request.ReadTimeMinutes,
        SortOrder = request.SortOrder
    };

    public static void ApplyUpdate(BlogPost entity, UpdateBlogPostRequest request)
    {
        entity.Title = request.Title;
        entity.Slug = request.Slug;
        entity.Summary = request.Summary;
        entity.ContentMarkdown = request.ContentMarkdown;
        entity.Category = request.Category;
        entity.Tags = request.Tags;
        entity.CoverImagePath = request.CoverImagePath;
        entity.ReadTimeMinutes = request.ReadTimeMinutes;
        entity.SortOrder = request.SortOrder;
        entity.RowVersion = request.RowVersion;
    }

    public static BlogPostDetailDto ToDetailDto(BlogPost entity) => new()
    {
        BlogPostId = entity.BlogPostId,
        PortfolioProfileId = entity.PortfolioProfileId,
        Title = entity.Title,
        Slug = entity.Slug,
        Summary = entity.Summary,
        ContentMarkdown = entity.ContentMarkdown,
        Category = entity.Category,
        Tags = entity.Tags,
        CoverImagePath = entity.CoverImagePath,
        IsPublished = entity.IsPublished,
        PublishedAt = entity.PublishedAt,
        ReadTimeMinutes = entity.ReadTimeMinutes,
        SortOrder = entity.SortOrder,
        RowVersion = entity.RowVersion,
        Audit = new AuditInfoDto
        {
            CreatedAt = entity.CreatedAt,
            CreatedBy = entity.CreatedBy,
            UpdatedAt = entity.UpdatedAt,
            UpdatedBy = entity.UpdatedBy
        }
    };
}
