using Portfolio.Application.Common.Models;

namespace Portfolio.Application.DTOs.References;

public sealed class GetReferencePagedRequest : PagedRequestBase
{
    public bool? IsPublic { get; set; }
}
