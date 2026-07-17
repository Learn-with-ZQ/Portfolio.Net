using Portfolio.Domain.Enums;

namespace Portfolio.Application.Common.Models;

public sealed class CommandResultDto
{
    public SpStatusCode StatusCode { get; init; }
    public string StatusMessage { get; init; } = string.Empty;
    public int? Id { get; init; }
    public bool IsSuccess => StatusCode == SpStatusCode.Success;

    public static CommandResultDto From(SpExecutionResult result) => new()
    {
        StatusCode = result.StatusCode,
        StatusMessage = result.StatusMessage,
        Id = result.OutId
    };
}
