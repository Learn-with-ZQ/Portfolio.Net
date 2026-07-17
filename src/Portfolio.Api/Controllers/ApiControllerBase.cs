using Microsoft.AspNetCore.Mvc;
using Portfolio.Api.Models;
using Portfolio.Application.Common.Models;
using Portfolio.Application.Common.Results;
using Portfolio.Domain.Enums;

namespace Portfolio.Api.Controllers;

[ApiController]
[Produces("application/json")]
public abstract class ApiControllerBase : ControllerBase
{
    protected IActionResult FromResult<T>(ServiceResult<T> result)
    {
        if (result.IsSuccess)
            return Ok(ApiEnvelope<T>.Ok(result.Data!));

        return ToErrorAction(result.StatusCode, result.ErrorMessage, result.ValidationErrors);
    }

    protected IActionResult FromResult(ServiceResult result)
    {
        if (result.IsSuccess)
            return NoContent();

        return ToErrorAction(result.StatusCode, result.ErrorMessage, result.ValidationErrors);
    }

    protected IActionResult FromCommandResult(
        ServiceResult<CommandResultDto> result,
        string? getByIdRouteName = null,
        object? routeValues = null)
    {
        if (!result.IsSuccess || result.Data is null)
            return FromResult(result);

        var envelope = ApiEnvelope<CommandResultDto>.Ok(result.Data);

        if (getByIdRouteName is not null && result.Data.Id.HasValue)
        {
            var values = routeValues ?? new { id = result.Data.Id.Value };
            return CreatedAtRoute(getByIdRouteName, values, envelope);
        }

        return StatusCode(StatusCodes.Status201Created, envelope);
    }

    protected IActionResult MismatchIdResult() =>
        BadRequest(ApiEnvelope<object>.Fail("Route id does not match request id."));

    private IActionResult ToErrorAction(
        SpStatusCode? statusCode,
        string? message,
        IReadOnlyList<string> validationErrors)
    {
        var envelope = ApiEnvelope<object>.Fail(message ?? "Request failed.", validationErrors);

        return statusCode switch
        {
            SpStatusCode.ValidationError => BadRequest(envelope),
            SpStatusCode.NotFound => NotFound(envelope),
            SpStatusCode.ConcurrencyConflict => Conflict(envelope),
            SpStatusCode.BusinessRuleViolation => UnprocessableEntity(envelope),
            SpStatusCode.Duplicate => Conflict(envelope),
            SpStatusCode.Unauthorized => Unauthorized(envelope),
            SpStatusCode.Forbidden => Forbid(),
            _ => StatusCode(StatusCodes.Status500InternalServerError, envelope)
        };
    }
}
