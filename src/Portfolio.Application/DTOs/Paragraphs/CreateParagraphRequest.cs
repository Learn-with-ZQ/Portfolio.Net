using Portfolio.Application.Common.Models;

namespace Portfolio.Application.DTOs.Paragraphs;

public sealed class CreateParagraphRequest : ModuleRequestBase
{
    public int ParagraphTypeId { get; set; }
    public string? ParagraphTitle { get; set; }
    public string ParagraphText { get; set; } = string.Empty;
    public int SortOrder { get; set; }
    public bool IsActive { get; set; } = true;
}
