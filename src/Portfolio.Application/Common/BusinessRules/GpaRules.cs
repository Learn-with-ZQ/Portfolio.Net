using Portfolio.Application.Common.Exceptions;

namespace Portfolio.Application.Common.BusinessRules;

public static class GpaRules
{
    public static void EnsureValidGpa(decimal? gpa, decimal? cgpa)
    {
        if (gpa is < 0 or > 4)
        {
            throw new BusinessRuleViolationException("GPA must be between 0 and 4.");
        }

        if (cgpa is < 0 or > 4)
        {
            throw new BusinessRuleViolationException("CGPA must be between 0 and 4.");
        }
    }
}
