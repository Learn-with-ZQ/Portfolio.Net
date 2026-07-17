using Portfolio.Domain.Enums;

namespace Portfolio.Application.Common.Models;

public sealed class SpExecutionResult
{
    public SpStatusCode StatusCode { get; init; }
    public string StatusMessage { get; init; } = string.Empty;
    public int? OutId { get; init; }

    public bool IsSuccess => StatusCode == SpStatusCode.Success;
}
