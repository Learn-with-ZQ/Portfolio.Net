using Portfolio.Application.Common.Models;

namespace Portfolio.Application.DTOs.Awards;

public sealed class SearchAwardRequest : SearchRequestBase
{
    public DateOnly? StartDateFrom { get; set; }
    public DateOnly? StartDateTo { get; set; }
}
