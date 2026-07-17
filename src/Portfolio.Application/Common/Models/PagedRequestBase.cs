namespace Portfolio.Application.Common.Models;

public abstract class PagedRequestBase : SearchRequestBase
{
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;

    public PaginationRequest ToPaginationRequest() => new()
    {
        PageNumber = PageNumber,
        PageSize = PageSize,
        SearchTerm = SearchTerm
    };
}
