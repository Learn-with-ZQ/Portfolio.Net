using Portfolio.Domain.Common;

namespace Portfolio.Domain.Entities.Awards;

public sealed class AwardDetail : AuditableEntity
{
    public int AwardDetailId { get; set; }
    public int AwardId { get; set; }
    public string AwardDetailName { get; set; } = string.Empty;
    public int SortOrder { get; set; }
}
