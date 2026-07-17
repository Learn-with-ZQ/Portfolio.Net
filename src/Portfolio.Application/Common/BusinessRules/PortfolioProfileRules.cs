using Portfolio.Application.Common.Exceptions;

namespace Portfolio.Application.Common.BusinessRules;

public static class PortfolioProfileRules
{
    public static void EnsureValidProfileId(int portfolioProfileId)
    {
        if (portfolioProfileId <= 0)
        {
            throw new BusinessRuleViolationException("A valid portfolio profile id is required.");
        }
    }
}
