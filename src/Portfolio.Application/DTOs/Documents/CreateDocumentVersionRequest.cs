namespace Portfolio.Application.DTOs.Documents;

public sealed class CreateDocumentVersionRequest
{
    public int SourceDocumentId { get; set; }
    public string FileName { get; set; } = string.Empty;
    public string FileExtension { get; set; } = string.Empty;
    public long? FileSizeBytes { get; set; }
    public string StoragePath { get; set; } = string.Empty;
    public string? MimeType { get; set; }
}
