using Portfolio.Application.Common.Models;

namespace Portfolio.Application.DTOs.Documents;

public sealed class DocumentDetailDto
{
    public int DocumentId { get; init; }
    public int PortfolioProfileId { get; init; }
    public int DocumentTypeId { get; init; }
    public string DocumentType { get; init; } = string.Empty;
    public string DocumentTitle { get; init; } = string.Empty;
    public string FileName { get; init; } = string.Empty;
    public string FileExtension { get; init; } = string.Empty;
    public long? FileSizeBytes { get; init; }
    public string StoragePath { get; init; } = string.Empty;
    public string? MimeType { get; init; }
    public bool IsPublic { get; init; }
    public bool IsDownloadable { get; init; } = true;
    public int VersionNumber { get; init; }
    public int SortOrder { get; init; }
    public byte[] RowVersion { get; init; } = [];
    public AuditInfoDto Audit { get; init; } = new();
}
