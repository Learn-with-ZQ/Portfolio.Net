using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Portfolio.Application.DTOs.Paragraphs;
using Portfolio.Application.Services.Interfaces;
using Portfolio.Infrastructure.Authorization;

namespace Portfolio.Api.Controllers;

[Route("api/paragraphs")]
public sealed class ParagraphsController : ApiControllerBase
{
    private readonly IParagraphService _service;

    public ParagraphsController(IParagraphService service) => _service = service;

    [HttpGet("{id:int}", Name = nameof(GetParagraphById))]
    [Authorize(Policy = RolePolicies.ReadAccess)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetParagraphById(int id, CancellationToken cancellationToken)
        => FromResult(await _service.GetByIdAsync(id, cancellationToken).ConfigureAwait(false));

    [HttpPost]
    [Authorize(Policy = RolePolicies.AdminOnly)]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] CreateParagraphRequest request, CancellationToken cancellationToken)
        => FromCommandResult(
            await _service.CreateAsync(request, cancellationToken).ConfigureAwait(false),
            nameof(GetParagraphById));

    [HttpPut("{id:int}")]
    [Authorize(Policy = RolePolicies.AdminOnly)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateParagraphRequest request, CancellationToken cancellationToken)
    {
        if (id != request.ParagraphId)
            return MismatchIdResult();

        return FromResult(await _service.UpdateAsync(request, cancellationToken).ConfigureAwait(false));
    }

    [HttpDelete("{id:int}")]
    [Authorize(Policy = RolePolicies.AdminOnly)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
        => FromResult(await _service.DeleteAsync(id, cancellationToken).ConfigureAwait(false));

    /// <summary>Paged content blocks. Public callers pass isActive=true.</summary>
    [HttpGet("paged")]
    [Authorize(Policy = RolePolicies.ReadAccess)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetPaged([FromQuery] GetParagraphPagedRequest request, CancellationToken cancellationToken)
        => FromResult(await _service.GetPagedAsync(request, cancellationToken).ConfigureAwait(false));
}
