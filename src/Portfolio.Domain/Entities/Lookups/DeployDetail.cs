using Portfolio.Domain.Common;

namespace Portfolio.Domain.Entities.Lookups;

public sealed class DeployDetail : AuditableEntity
{
    public int DeployDetailId { get; set; }
    public string DeployDetailName { get; set; } = string.Empty;
    public string DeployedTo { get; set; } = string.Empty;
    public int? DeployedByCompanyId { get; set; }
}
