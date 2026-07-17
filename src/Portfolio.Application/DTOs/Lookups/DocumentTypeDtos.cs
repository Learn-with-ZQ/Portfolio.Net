namespace Portfolio.Application.DTOs.Lookups;

public sealed class DocumentTypeDto
{
    public int DocumentTypeId { get; init; }
    public string TypeName { get; init; } = string.Empty;
    public byte[] RowVersion { get; init; } = [];
}

public sealed class CreateDocumentTypeRequest
{
    public string TypeName { get; set; } = string.Empty;
}

public sealed class UpdateDocumentTypeRequest
{
    public int DocumentTypeId { get; set; }
    public string TypeName { get; set; } = string.Empty;
    public byte[] RowVersion { get; set; } = [];
}
