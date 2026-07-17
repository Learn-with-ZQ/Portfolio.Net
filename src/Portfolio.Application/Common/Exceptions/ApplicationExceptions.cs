using Portfolio.Domain.Enums;

namespace Portfolio.Application.Common.Exceptions;

public class ApplicationException : Exception
{
    public ApplicationException(string message) : base(message)
    {
    }

    public ApplicationException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}

public sealed class ValidationFailureException : ApplicationException
{
    public ValidationFailureException(IEnumerable<string> errors)
        : base(string.Join("; ", errors))
    {
        Errors = errors.ToList();
    }

    public IReadOnlyList<string> Errors { get; }
}

public sealed class BusinessRuleViolationException : ApplicationException
{
    public BusinessRuleViolationException(string message) : base(message)
    {
    }
}

public sealed class StoredProcedureException : ApplicationException
{
    public StoredProcedureException(SpStatusCode statusCode, string message)
        : base(message)
    {
        StatusCode = statusCode;
    }

    public SpStatusCode StatusCode { get; }
}
