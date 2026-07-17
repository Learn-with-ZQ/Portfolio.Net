using Portfolio.Application.Common.Models;

namespace Portfolio.Application.DTOs.Documents;

public sealed class GetDocumentPagedRequest : PagedRequestBase
{
    public int? DocumentTypeId { get; set; }
    public bool? IsPublic { get; set; }
}
