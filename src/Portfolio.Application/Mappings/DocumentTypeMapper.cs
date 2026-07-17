using Portfolio.Application.DTOs.Lookups;
using Portfolio.Domain.Entities.Lookups;

namespace Portfolio.Application.Mappings;

public static class DocumentTypeMapper
{
    public static DocumentTypeDto ToDto(DocumentType entity) => new()
    {
        DocumentTypeId = entity.DocumentTypeId,
        TypeName = entity.TypeName,
        RowVersion = entity.RowVersion
    };

    public static DocumentType ToEntity(CreateDocumentTypeRequest request) => new()
    {
        TypeName = request.TypeName
    };

    public static void ApplyUpdate(DocumentType entity, UpdateDocumentTypeRequest request)
    {
        entity.TypeName = request.TypeName;
        entity.RowVersion = request.RowVersion;
    }
}
