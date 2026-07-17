namespace Portfolio.Application.Common.Models;

public sealed class AuditInfoDto
{
    public DateTime CreatedAt { get; init; }
    public int? CreatedBy { get; init; }
    public DateTime UpdatedAt { get; init; }
    public int? UpdatedBy { get; init; }
}
