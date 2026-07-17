using Portfolio.Application.Common.Models;
using Portfolio.Application.DTOs.Paragraphs;
using Portfolio.Domain.Entities.Paragraphs;

namespace Portfolio.Application.Mappings;

public static class ParagraphMapper
{
    public static Paragraph ToEntity(CreateParagraphRequest request) => new()
    {
        PortfolioProfileId = request.PortfolioProfileId,
        ParagraphTypeId = request.ParagraphTypeId,
        ParagraphTitle = request.ParagraphTitle,
        ParagraphText = request.ParagraphText,
        SortOrder = request.SortOrder,
        IsActive = request.IsActive
    };

    public static void ApplyUpdate(Paragraph entity, UpdateParagraphRequest request)
    {
        entity.ParagraphTypeId = request.ParagraphTypeId;
        entity.ParagraphTitle = request.ParagraphTitle;
        entity.ParagraphText = request.ParagraphText;
        entity.SortOrder = request.SortOrder;
        entity.IsActive = request.IsActive;
        entity.RowVersion = request.RowVersion;
    }

    public static ParagraphDetailDto ToDetailDto(Paragraph entity) => new()
    {
        ParagraphId = entity.ParagraphId,
        PortfolioProfileId = entity.PortfolioProfileId,
        ParagraphTypeId = entity.ParagraphTypeId,
        ParagraphTypeName = entity.ParagraphTypeName ?? string.Empty,
        ParagraphTitle = entity.ParagraphTitle,
        ParagraphText = entity.ParagraphText,
        SortOrder = entity.SortOrder,
        IsActive = entity.IsActive,
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
