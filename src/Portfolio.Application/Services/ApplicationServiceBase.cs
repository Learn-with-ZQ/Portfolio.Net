using FluentValidation;
using Portfolio.Application.Common.Exceptions;
using Portfolio.Application.Common.Results;
using Portfolio.Domain.Enums;
using Portfolio.Domain.Exceptions;

namespace Portfolio.Application.Services;

public abstract class ApplicationServiceBase
{
    protected static async Task ValidateAsync<T>(IValidator<T> validator, T instance, CancellationToken cancellationToken)
    {
        var result = await validator.ValidateAsync(instance, cancellationToken).ConfigureAwait(false);
        if (!result.IsValid)
        {
            throw new ValidationFailureException(result.Errors.Select(e => e.ErrorMessage));
        }
    }

    protected static ServiceResult<T> HandleException<T>(Exception ex) => ex switch
    {
        ValidationFailureException validation => ServiceResult<T>.Failure(
            validation.Message,
            SpStatusCode.ValidationError,
            validation.Errors),
        BusinessRuleViolationException business => ServiceResult<T>.Failure(
            business.Message,
            SpStatusCode.BusinessRuleViolation),
        NotFoundException notFound => ServiceResult<T>.Failure(
            notFound.Message,
            SpStatusCode.NotFound),
        StoredProcedureException sp => ServiceResult<T>.Failure(
            sp.Message,
            sp.StatusCode),
        UnauthorizedAccessException => ServiceResult<T>.Failure(
            "Unauthorized.",
            SpStatusCode.Unauthorized),
        _ => ServiceResult<T>.Failure(
            "An unexpected error occurred while processing the request.",
            SpStatusCode.UnexpectedError)
    };

    protected static ServiceResult HandleException(Exception ex) => ex switch
    {
        ValidationFailureException validation => ServiceResult.Failure(
            validation.Message,
            SpStatusCode.ValidationError,
            validation.Errors),
        BusinessRuleViolationException business => ServiceResult.Failure(
            business.Message,
            SpStatusCode.BusinessRuleViolation),
        NotFoundException notFound => ServiceResult.Failure(
            notFound.Message,
            SpStatusCode.NotFound),
        StoredProcedureException sp => ServiceResult.Failure(
            sp.Message,
            sp.StatusCode),
        UnauthorizedAccessException => ServiceResult.Failure(
            "Unauthorized.",
            SpStatusCode.Unauthorized),
        _ => ServiceResult.Failure(
            "An unexpected error occurred while processing the request.",
            SpStatusCode.UnexpectedError)
    };
}
