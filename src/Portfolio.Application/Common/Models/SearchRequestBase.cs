namespace Portfolio.Application.Common.Models;

public abstract class SearchRequestBase : ModuleRequestBase
{
    public string? SearchTerm { get; set; }
}
