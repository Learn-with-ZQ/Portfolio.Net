using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Portfolio.Application.DTOs.Lookups;
using Portfolio.Application.Services.Interfaces;
using Portfolio.Infrastructure.Authorization;

namespace Portfolio.Api.Controllers;

[Route("api/courses")]
public sealed class CoursesController : ApiControllerBase
{
    private readonly ICourseService _service;

    public CoursesController(ICourseService service) => _service = service;

    [HttpGet("lookup")]
    [Authorize(Policy = RolePolicies.ReadAccess)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetLookup(CancellationToken cancellationToken)
        => FromResult(await _service.GetLookupAsync(cancellationToken).ConfigureAwait(false));

    [HttpGet("paged")]
    [Authorize(Policy = RolePolicies.ReadAccess)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetPaged([FromQuery] GetLookupPagedRequest request, CancellationToken cancellationToken)
        => FromResult(await _service.GetPagedAsync(request, cancellationToken).ConfigureAwait(false));

    [HttpGet("{id:int}", Name = nameof(GetCourseById))]
    [Authorize(Policy = RolePolicies.ReadAccess)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetCourseById(int id, CancellationToken cancellationToken)
        => FromResult(await _service.GetByIdAsync(id, cancellationToken).ConfigureAwait(false));

    [HttpPost]
    [Authorize(Policy = RolePolicies.AdminOnly)]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] CreateCourseRequest request, CancellationToken cancellationToken)
        => FromCommandResult(
            await _service.CreateAsync(request, cancellationToken).ConfigureAwait(false),
            nameof(GetCourseById));

    [HttpPut("{id:int}")]
    [Authorize(Policy = RolePolicies.AdminOnly)]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateCourseRequest request, CancellationToken cancellationToken)
    {
        if (id != request.CourseId)
            return MismatchIdResult();

        return FromCommandResult(await _service.UpdateAsync(request, cancellationToken).ConfigureAwait(false));
    }

    [HttpDelete("{id:int}")]
    [Authorize(Policy = RolePolicies.AdminOnly)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
        => FromResult(await _service.DeleteAsync(id, cancellationToken).ConfigureAwait(false));
}
