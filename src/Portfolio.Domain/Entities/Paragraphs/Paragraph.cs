using Portfolio.Domain.Common;

namespace Portfolio.Domain.Entities.Paragraphs;

public sealed class Paragraph : AuditableEntity
{
    public int ParagraphId { get; set; }
    public int PortfolioProfileId { get; set; }
    public int ParagraphTypeId { get; set; }
    public string? ParagraphTypeName { get; set; }
    public string? ParagraphTitle { get; set; }
    public string ParagraphText { get; set; } = string.Empty;
    public int SortOrder { get; set; }
    public bool IsActive { get; set; } = true;
}
