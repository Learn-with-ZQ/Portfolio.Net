using Portfolio.Domain.Common;

namespace Portfolio.Domain.Entities.Documents;

public sealed class Document : AuditableEntity
{
    public int DocumentId { get; set; }
    public int PortfolioProfileId { get; set; }
    public int DocumentTypeId { get; set; }
    public string DocumentTitle { get; set; } = string.Empty;
    public string FileName { get; set; } = string.Empty;
    public string FileExtension { get; set; } = string.Empty;
    public long? FileSizeBytes { get; set; }
    public string StoragePath { get; set; } = string.Empty;
    public string? MimeType { get; set; }
    public bool IsPublic { get; set; } = true;
    /// <summary>When false, the public site hides the download action (view-only).</summary>
    public bool IsDownloadable { get; set; } = true;
    public int VersionNumber { get; set; } = 1;
    public int SortOrder { get; set; }

    public string? DocumentType { get; set; }
}
