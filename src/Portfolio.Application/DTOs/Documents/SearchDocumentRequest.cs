using Portfolio.Application.Common.Models;

namespace Portfolio.Application.DTOs.Documents;

public sealed class SearchDocumentRequest : SearchRequestBase
{
    public int? DocumentTypeId { get; set; }
    public bool? IsPublic { get; set; }
    public string? FileExtension { get; set; }
}
