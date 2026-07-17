using Portfolio.Application.Common.Exceptions;

namespace Portfolio.Application.Common.BusinessRules;

public static class DateRangeRules
{
    public static void EnsureValidRange(DateOnly startDate, DateOnly? endDate, string entityName = "Record")
    {
        if (endDate.HasValue && endDate.Value < startDate)
        {
            throw new BusinessRuleViolationException(
                $"{entityName} end date cannot be earlier than start date.");
        }
    }
}
