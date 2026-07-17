using Portfolio.Application.Common.Models;

namespace Portfolio.Application.DTOs.Paragraphs;

public sealed class GetParagraphPagedRequest : PagedRequestBase
{
    public int? ParagraphTypeId { get; set; }
    public bool? IsActive { get; set; }
}
