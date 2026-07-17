using Portfolio.Domain.Common;

namespace Portfolio.Domain.Entities.Lookups;

public sealed class DocumentType : AuditableEntity
{
    public int DocumentTypeId { get; set; }
    public string TypeName { get; set; } = string.Empty;
}
