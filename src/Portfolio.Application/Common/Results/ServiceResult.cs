using Portfolio.Application.Common.Models;
using Portfolio.Domain.Enums;

namespace Portfolio.Application.Common.Results;

public sealed class ServiceResult<T>
{
    public bool IsSuccess { get; init; }
    public T? Data { get; init; }
    public string? ErrorMessage { get; init; }
    public SpStatusCode? StatusCode { get; init; }
    public IReadOnlyList<string> ValidationErrors { get; init; } = [];

    public static ServiceResult<T> Success(T data) => new()
    {
        IsSuccess = true,
        Data = data
    };

    public static ServiceResult<T> Failure(
        string message,
        SpStatusCode statusCode = SpStatusCode.ValidationError,
        IReadOnlyList<string>? validationErrors = null) => new()
    {
        IsSuccess = false,
        ErrorMessage = message,
        StatusCode = statusCode,
        ValidationErrors = validationErrors ?? []
    };
}

public sealed class ServiceResult
{
    public bool IsSuccess { get; init; }
    public string? ErrorMessage { get; init; }
    public SpStatusCode? StatusCode { get; init; }
    public int? Id { get; init; }
    public IReadOnlyList<string> ValidationErrors { get; init; } = [];

    public static ServiceResult Success(int? id = null) => new()
    {
        IsSuccess = true,
        Id = id
    };

    public static ServiceResult Failure(
        string message,
        SpStatusCode statusCode = SpStatusCode.ValidationError,
        IReadOnlyList<string>? validationErrors = null) => new()
    {
        IsSuccess = false,
        ErrorMessage = message,
        StatusCode = statusCode,
        ValidationErrors = validationErrors ?? []
    };

    public static ServiceResult FromCommand(CommandResultDto command) => command.IsSuccess
        ? Success(command.Id)
        : Failure(command.StatusMessage, command.StatusCode);
}
