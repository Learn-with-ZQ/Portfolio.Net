namespace Portfolio.Application.DTOs.Documents;

public sealed class DocumentDto
{
    public int DocumentId { get; init; }
    public string DocumentType { get; init; } = string.Empty;
    public string DocumentTitle { get; init; } = string.Empty;
    public string FileName { get; init; } = string.Empty;
    public string FileExtension { get; init; } = string.Empty;
    public long? FileSizeBytes { get; init; }
    public string StoragePath { get; init; } = string.Empty;
    public bool IsPublic { get; init; }
    public bool IsDownloadable { get; init; } = true;
    public int VersionNumber { get; init; }
    public int SortOrder { get; init; }
    public DateTime CreatedAt { get; init; }
}
