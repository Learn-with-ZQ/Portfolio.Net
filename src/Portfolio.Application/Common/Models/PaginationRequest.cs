namespace Portfolio.Application.Common.Models;

public sealed class PaginationRequest
{
    private const int MaxPageSize = 100;

    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = 10;
    public string? SearchTerm { get; init; }

    public int NormalizedPageNumber => PageNumber < 1 ? 1 : PageNumber;
    public int NormalizedPageSize => PageSize switch
    {
        < 1 => 10,
        > MaxPageSize => MaxPageSize,
        _ => PageSize
    };
}
