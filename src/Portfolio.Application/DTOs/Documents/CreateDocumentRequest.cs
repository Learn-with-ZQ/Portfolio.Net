using Portfolio.Application.Common.Models;

namespace Portfolio.Application.DTOs.Documents;

public sealed class CreateDocumentRequest : ModuleRequestBase
{
    public int DocumentTypeId { get; set; }
    public string DocumentTitle { get; set; } = string.Empty;
    public string FileName { get; set; } = string.Empty;
    public string FileExtension { get; set; } = string.Empty;
    public long? FileSizeBytes { get; set; }
    public string StoragePath { get; set; } = string.Empty;
    public string? MimeType { get; set; }
    public bool IsPublic { get; set; } = true;
    public bool IsDownloadable { get; set; } = true;
    public int VersionNumber { get; set; } = 1;
    public int SortOrder { get; set; }
}
