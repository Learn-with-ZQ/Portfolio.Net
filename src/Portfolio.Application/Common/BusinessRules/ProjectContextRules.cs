using Portfolio.Application.Common.Exceptions;

namespace Portfolio.Application.Common.BusinessRules;

public static class ProjectContextRules
{
    public static void EnsureSingleContext(string? practice, int? companyId, int? courseId)
    {
        var count = 0;
        if (!string.IsNullOrWhiteSpace(practice)) count++;
        if (companyId.HasValue) count++;
        if (courseId.HasValue) count++;

        if (count > 1)
        {
            throw new BusinessRuleViolationException(
                "A project can only be linked to one context: Practice, Company, or Course.");
        }
    }
}
