namespace Portfolio.Application.DTOs.Documents;

public sealed class UpdateDocumentRequest
{
    public int DocumentId { get; set; }
    public int DocumentTypeId { get; set; }
    public string DocumentTitle { get; set; } = string.Empty;
    public string FileName { get; set; } = string.Empty;
    public string FileExtension { get; set; } = string.Empty;
    public long? FileSizeBytes { get; set; }
    public string StoragePath { get; set; } = string.Empty;
    public string? MimeType { get; set; }
    public bool IsPublic { get; set; }
    public bool IsDownloadable { get; set; } = true;
    public int VersionNumber { get; set; }
    public int SortOrder { get; set; }
    public byte[] RowVersion { get; set; } = [];
}
