using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Portfolio.Application.DTOs.Blog;
using Portfolio.Application.Services.Interfaces;
using Portfolio.Infrastructure.Authorization;

namespace Portfolio.Api.Controllers;

[Route("api/blog")]
public sealed class BlogController : ApiControllerBase
{
    private readonly IBlogService _service;

    public BlogController(IBlogService service) => _service = service;

    [HttpGet("{id:int}", Name = nameof(GetBlogPostById))]
    [Authorize(Policy = RolePolicies.ReadAccess)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetBlogPostById(int id, CancellationToken cancellationToken)
        => FromResult(await _service.GetByIdAsync(id, cancellationToken).ConfigureAwait(false));

    /// <summary>Public post detail by slug (published only).</summary>
    [HttpGet("slug/{slug}")]
    [Authorize(Policy = RolePolicies.ReadAccess)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetBySlug(string slug, [FromQuery] int portfolioProfileId, CancellationToken cancellationToken)
        => FromResult(await _service.GetBySlugAsync(portfolioProfileId, slug, cancellationToken).ConfigureAwait(false));

    [HttpPost]
    [Authorize(Policy = RolePolicies.AdminOnly)]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] CreateBlogPostRequest request, CancellationToken cancellationToken)
        => FromCommandResult(
            await _service.CreateAsync(request, cancellationToken).ConfigureAwait(false),
            nameof(GetBlogPostById));

    [HttpPut("{id:int}")]
    [Authorize(Policy = RolePolicies.AdminOnly)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateBlogPostRequest request, CancellationToken cancellationToken)
    {
        if (id != request.BlogPostId)
            return MismatchIdResult();

        return FromResult(await _service.UpdateAsync(request, cancellationToken).ConfigureAwait(false));
    }

    [HttpPost("{id:int}/publish")]
    [Authorize(Policy = RolePolicies.AdminOnly)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> Publish(int id, CancellationToken cancellationToken)
        => FromResult(await _service.PublishAsync(id, cancellationToken).ConfigureAwait(false));

    [HttpPost("{id:int}/unpublish")]
    [Authorize(Policy = RolePolicies.AdminOnly)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> Unpublish(int id, CancellationToken cancellationToken)
        => FromResult(await _service.UnpublishAsync(id, cancellationToken).ConfigureAwait(false));

    [HttpDelete("{id:int}")]
    [Authorize(Policy = RolePolicies.AdminOnly)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
        => FromResult(await _service.DeleteAsync(id, cancellationToken).ConfigureAwait(false));

    /// <summary>Paged posts. Public callers pass isPublished=true.</summary>
    [HttpGet("paged")]
    [Authorize(Policy = RolePolicies.ReadAccess)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetPaged([FromQuery] GetBlogPagedRequest request, CancellationToken cancellationToken)
        => FromResult(await _service.GetPagedAsync(request, cancellationToken).ConfigureAwait(false));
}
