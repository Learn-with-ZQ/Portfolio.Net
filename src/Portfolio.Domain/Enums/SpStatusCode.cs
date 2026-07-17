namespace Portfolio.Domain.Enums;

/// <summary>
/// Mirrors SQL stored procedure @StatusCode convention.
/// </summary>
public enum SpStatusCode
{
    Success = 0,
    ValidationError = -1,
    NotFound = -2,
    ConcurrencyConflict = -3,
    BusinessRuleViolation = -4,
    Duplicate = -5,
    Unauthorized = -6,
    Forbidden = -7,
    UnexpectedError = -99
}
