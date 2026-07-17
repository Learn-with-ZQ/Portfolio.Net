using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Portfolio.Application.DTOs.Documents;
using Portfolio.Application.Services.Interfaces;
using Portfolio.Infrastructure.Authorization;

namespace Portfolio.Api.Controllers;

[Route("api/documents")]
public sealed class DocumentsController : ApiControllerBase
{
    private readonly IDocumentService _service;

    public DocumentsController(IDocumentService service) => _service = service;

    [HttpGet("{id:int}", Name = nameof(GetDocumentById))]
    [Authorize(Policy = RolePolicies.ReadAccess)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetDocumentById(int id, CancellationToken cancellationToken)
        => FromResult(await _service.GetByIdAsync(id, cancellationToken).ConfigureAwait(false));

    [HttpPost]
    [Authorize(Policy = RolePolicies.AdminOnly)]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] CreateDocumentRequest request, CancellationToken cancellationToken)
        => FromCommandResult(
            await _service.CreateAsync(request, cancellationToken).ConfigureAwait(false),
            nameof(GetDocumentById));

    [HttpPut("{id:int}")]
    [Authorize(Policy = RolePolicies.AdminOnly)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateDocumentRequest request, CancellationToken cancellationToken)
    {
        if (id != request.DocumentId)
            return MismatchIdResult();

        return FromResult(await _service.UpdateAsync(request, cancellationToken).ConfigureAwait(false));
    }

    [HttpDelete("{id:int}")]
    [Authorize(Policy = RolePolicies.AdminOnly)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
        => FromResult(await _service.DeleteAsync(id, cancellationToken).ConfigureAwait(false));

    [HttpPost("{documentId:int}/versions")]
    [Authorize(Policy = RolePolicies.AdminOnly)]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> CreateVersion(int documentId, [FromBody] CreateDocumentVersionBody body, CancellationToken cancellationToken)
    {
        var request = new CreateDocumentVersionRequest
        {
            SourceDocumentId = documentId,
            FileName = body.FileName,
            FileExtension = body.FileExtension,
            FileSizeBytes = body.FileSizeBytes,
            StoragePath = body.StoragePath,
            MimeType = body.MimeType
        };

        return FromCommandResult(
            await _service.CreateVersionAsync(request, cancellationToken).ConfigureAwait(false),
            nameof(GetDocumentById));
    }

    [HttpGet("search")]
    [Authorize(Policy = RolePolicies.ReadAccess)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> Search([FromQuery] SearchDocumentRequest request, CancellationToken cancellationToken)
        => FromResult(await _service.SearchAsync(request, cancellationToken).ConfigureAwait(false));

    [HttpGet("paged")]
    [Authorize(Policy = RolePolicies.ReadAccess)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetPaged([FromQuery] GetDocumentPagedRequest request, CancellationToken cancellationToken)
        => FromResult(await _service.GetPagedAsync(request, cancellationToken).ConfigureAwait(false));
}

public sealed class CreateDocumentVersionBody
{
    public string FileName { get; set; } = string.Empty;
    public string FileExtension { get; set; } = string.Empty;
    public long? FileSizeBytes { get; set; }
    public string StoragePath { get; set; } = string.Empty;
    public string? MimeType { get; set; }
}
