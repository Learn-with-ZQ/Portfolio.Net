using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Portfolio.Application.DTOs.Awards;
using Portfolio.Application.Services.Interfaces;
using Portfolio.Infrastructure.Authorization;

namespace Portfolio.Api.Controllers;

[Route("api/awards")]
public sealed class AwardsController : ApiControllerBase
{
    private readonly IAwardService _service;

    public AwardsController(IAwardService service) => _service = service;

    [HttpGet("{id:int}", Name = nameof(GetAwardById))]
    [Authorize(Policy = RolePolicies.ReadAccess)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetAwardById(int id, CancellationToken cancellationToken)
        => FromResult(await _service.GetByIdAsync(id, cancellationToken).ConfigureAwait(false));

    [HttpPost]
    [Authorize(Policy = RolePolicies.AdminOnly)]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] CreateAwardRequest request, CancellationToken cancellationToken)
        => FromCommandResult(
            await _service.CreateAsync(request, cancellationToken).ConfigureAwait(false),
            nameof(GetAwardById));

    [HttpPut("{id:int}")]
    [Authorize(Policy = RolePolicies.AdminOnly)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateAwardRequest request, CancellationToken cancellationToken)
    {
        if (id != request.AwardId)
            return MismatchIdResult();

        return FromResult(await _service.UpdateAsync(request, cancellationToken).ConfigureAwait(false));
    }

    [HttpDelete("{id:int}")]
    [Authorize(Policy = RolePolicies.AdminOnly)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
        => FromResult(await _service.DeleteAsync(id, cancellationToken).ConfigureAwait(false));

    [HttpGet("search")]
    [Authorize(Policy = RolePolicies.ReadAccess)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> Search([FromQuery] SearchAwardRequest request, CancellationToken cancellationToken)
        => FromResult(await _service.SearchAsync(request, cancellationToken).ConfigureAwait(false));

    [HttpGet("paged")]
    [Authorize(Policy = RolePolicies.ReadAccess)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetPaged([FromQuery] GetAwardPagedRequest request, CancellationToken cancellationToken)
        => FromResult(await _service.GetPagedAsync(request, cancellationToken).ConfigureAwait(false));
}
