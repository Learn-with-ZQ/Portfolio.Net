namespace Portfolio.Application.DTOs.Paragraphs;

public sealed class UpdateParagraphRequest
{
    public int ParagraphId { get; set; }
    public int ParagraphTypeId { get; set; }
    public string? ParagraphTitle { get; set; }
    public string ParagraphText { get; set; } = string.Empty;
    public int SortOrder { get; set; }
    public bool IsActive { get; set; }
    public byte[] RowVersion { get; set; } = [];
}
