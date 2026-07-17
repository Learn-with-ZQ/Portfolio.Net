using Portfolio.Domain.Common;

namespace Portfolio.Domain.Entities.Awards;

public sealed class Award : AuditableEntity
{
    public int AwardId { get; set; }
    public int PortfolioProfileId { get; set; }
    public string AwardName { get; set; } = string.Empty;
    public DateOnly StartDate { get; set; }
    public DateOnly? EndDate { get; set; }
    public int SortOrder { get; set; }

    public ICollection<AwardDetail> Details { get; set; } = [];
}
