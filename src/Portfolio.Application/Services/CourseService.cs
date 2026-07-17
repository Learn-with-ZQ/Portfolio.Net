using FluentValidation;
using Portfolio.Application.Abstractions.Persistence;
using Portfolio.Application.Common.Helpers;
using Portfolio.Application.Common.Interfaces;
using Portfolio.Application.Common.Models;
using Portfolio.Application.Common.Results;
using Portfolio.Application.DTOs.Lookups;
using Portfolio.Application.Mappings;
using Portfolio.Application.Services.Interfaces;
using Portfolio.Domain.Exceptions;

namespace Portfolio.Application.Services;

public sealed class CourseService : ApplicationServiceBase, ICourseService
{
    private readonly ICourseRepository _repository;
    private readonly ICurrentUserService _currentUser;
    private readonly IValidator<CreateCourseRequest> _createValidator;
    private readonly IValidator<UpdateCourseRequest> _updateValidator;

    public CourseService(ICourseRepository repository, ICurrentUserService currentUser,
        IValidator<CreateCourseRequest> createValidator, IValidator<UpdateCourseRequest> updateValidator)
    {
        _repository = repository;
        _currentUser = currentUser;
        _createValidator = createValidator;
        _updateValidator = updateValidator;
    }

    public async Task<ServiceResult<IReadOnlyList<LookupItemDto>>> GetLookupAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            var items = await _repository.GetLookupAsync(cancellationToken).ConfigureAwait(false);
            return ServiceResult<IReadOnlyList<LookupItemDto>>.Success(items);
        }
        catch (Exception ex) { return HandleException<IReadOnlyList<LookupItemDto>>(ex); }
    }

    public async Task<ServiceResult<PagedResult<CourseDto>>> GetPagedAsync(GetLookupPagedRequest request, CancellationToken cancellationToken = default)
    {
        try
        {
            var page = await _repository.GetPagedAsync(request, cancellationToken).ConfigureAwait(false);
            return ServiceResult<PagedResult<CourseDto>>.Success(page);
        }
        catch (Exception ex) { return HandleException<PagedResult<CourseDto>>(ex); }
    }

    public async Task<ServiceResult<CourseDto>> GetByIdAsync(int courseId, CancellationToken cancellationToken = default)
    {
        try
        {
            var entity = await _repository.GetByIdAsync(courseId, cancellationToken).ConfigureAwait(false);
            if (entity is null) throw new NotFoundException("Course", courseId);
            return ServiceResult<CourseDto>.Success(CourseMapper.ToDto(entity));
        }
        catch (Exception ex) { return HandleException<CourseDto>(ex); }
    }

    public async Task<ServiceResult<CommandResultDto>> CreateAsync(CreateCourseRequest request, CancellationToken cancellationToken = default)
    {
        try
        {
            await ValidateAsync(_createValidator, request, cancellationToken).ConfigureAwait(false);
            var result = await _repository.InsertAsync(CourseMapper.ToEntity(request), _currentUser.UserId, cancellationToken).ConfigureAwait(false);
            SpResultHelper.EnsureSuccess(result);
            return ServiceResult<CommandResultDto>.Success(SpResultHelper.ToCommandResult(result));
        }
        catch (Exception ex) { return HandleException<CommandResultDto>(ex); }
    }

    public async Task<ServiceResult<CommandResultDto>> UpdateAsync(UpdateCourseRequest request, CancellationToken cancellationToken = default)
    {
        try
        {
            await ValidateAsync(_updateValidator, request, cancellationToken).ConfigureAwait(false);
            var entity = await _repository.GetByIdAsync(request.CourseId, cancellationToken).ConfigureAwait(false);
            if (entity is null) throw new NotFoundException("Course", request.CourseId);

            CourseMapper.ApplyUpdate(entity, request);
            var result = await _repository.UpdateAsync(entity, _currentUser.UserId, cancellationToken).ConfigureAwait(false);
            SpResultHelper.EnsureSuccess(result);
            return ServiceResult<CommandResultDto>.Success(SpResultHelper.ToCommandResult(result));
        }
        catch (Exception ex) { return HandleException<CommandResultDto>(ex); }
    }

    public async Task<ServiceResult> DeleteAsync(int courseId, CancellationToken cancellationToken = default)
    {
        try
        {
            var result = await _repository.DeleteAsync(courseId, _currentUser.UserId, cancellationToken).ConfigureAwait(false);
            SpResultHelper.EnsureSuccess(result);
            return ServiceResult.Success();
        }
        catch (Exception ex) { return HandleException(ex); }
    }
}
