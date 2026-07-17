using Portfolio.Application.Common.Models;

namespace Portfolio.Application.DTOs.Paragraphs;

public sealed class ParagraphDetailDto
{
    public int ParagraphId { get; init; }
    public int PortfolioProfileId { get; init; }
    public int ParagraphTypeId { get; init; }
    public string ParagraphTypeName { get; init; } = string.Empty;
    public string? ParagraphTitle { get; init; }
    public string ParagraphText { get; init; } = string.Empty;
    public int SortOrder { get; init; }
    public bool IsActive { get; init; }
    public byte[] RowVersion { get; init; } = [];
    public AuditInfoDto Audit { get; init; } = new();
}
