using Portfolio.Application.Common.Exceptions;
using Portfolio.Application.Common.Models;
using Portfolio.Domain.Enums;

namespace Portfolio.Application.Common.Helpers;

public static class SpResultHelper
{
    public static void EnsureSuccess(SpExecutionResult result)
    {
        if (result.IsSuccess)
        {
            return;
        }

        throw result.StatusCode switch
        {
            SpStatusCode.NotFound => new Domain.Exceptions.NotFoundException("Record", result.OutId ?? 0),
            SpStatusCode.ConcurrencyConflict => new StoredProcedureException(result.StatusCode, result.StatusMessage),
            SpStatusCode.Duplicate => new StoredProcedureException(result.StatusCode, result.StatusMessage),
            SpStatusCode.BusinessRuleViolation => new BusinessRuleViolationException(result.StatusMessage),
            SpStatusCode.ValidationError => new ValidationFailureException([result.StatusMessage]),
            _ => new StoredProcedureException(result.StatusCode, result.StatusMessage)
        };
    }

    public static CommandResultDto ToCommandResult(SpExecutionResult result) => CommandResultDto.From(result);
}
