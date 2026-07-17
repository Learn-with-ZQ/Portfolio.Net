using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Portfolio.Application.DTOs.Testimonials;
using Portfolio.Application.Services.Interfaces;
using Portfolio.Infrastructure.Authorization;

namespace Portfolio.Api.Controllers;

[Route("api/testimonials")]
public sealed class TestimonialsController : ApiControllerBase
{
    private readonly ITestimonialService _service;

    public TestimonialsController(ITestimonialService service) => _service = service;

    [HttpGet("{id:int}", Name = nameof(GetTestimonialById))]
    [Authorize(Policy = RolePolicies.ReadAccess)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetTestimonialById(int id, CancellationToken cancellationToken)
        => FromResult(await _service.GetByIdAsync(id, cancellationToken).ConfigureAwait(false));

    /// <summary>Public submission — created in Pending state, awaiting approval.</summary>
    [HttpPost("submit")]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Submit([FromBody] SubmitTestimonialRequest request, CancellationToken cancellationToken)
        => FromCommandResult(
            await _service.SubmitAsync(request, cancellationToken).ConfigureAwait(false),
            nameof(GetTestimonialById));

    /// <summary>Paged list. Public callers pass status=2 for approved (published) only.</summary>
    [HttpGet("paged")]
    [Authorize(Policy = RolePolicies.ReadAccess)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetPaged([FromQuery] GetTestimonialPagedRequest request, CancellationToken cancellationToken)
        => FromResult(await _service.GetPagedAsync(request, cancellationToken).ConfigureAwait(false));

    [HttpPost("{id:int}/approve")]
    [Authorize(Policy = RolePolicies.AdminOnly)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Approve(int id, CancellationToken cancellationToken)
        => FromResult(await _service.ApproveAsync(id, cancellationToken).ConfigureAwait(false));

    [HttpPost("{id:int}/reject")]
    [Authorize(Policy = RolePolicies.AdminOnly)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Reject(int id, CancellationToken cancellationToken)
        => FromResult(await _service.RejectAsync(id, cancellationToken).ConfigureAwait(false));

    [HttpDelete("{id:int}")]
    [Authorize(Policy = RolePolicies.AdminOnly)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
        => FromResult(await _service.DeleteAsync(id, cancellationToken).ConfigureAwait(false));
}
