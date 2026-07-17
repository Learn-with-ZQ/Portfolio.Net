namespace Portfolio.Application.DTOs.Paragraphs;

public sealed class ParagraphDto
{
    public int ParagraphId { get; init; }
    public int ParagraphTypeId { get; init; }
    public string ParagraphTypeName { get; init; } = string.Empty;
    public string? ParagraphTitle { get; init; }
    public string ParagraphText { get; init; } = string.Empty;
    public int SortOrder { get; init; }
    public bool IsActive { get; init; }
}
