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

public sealed class DegreeLevelService : ApplicationServiceBase, IDegreeLevelService
{
    private readonly IDegreeLevelRepository _repository;
    private readonly ICurrentUserService _currentUser;
    private readonly IValidator<CreateDegreeLevelRequest> _createValidator;
    private readonly IValidator<UpdateDegreeLevelRequest> _updateValidator;

    public DegreeLevelService(IDegreeLevelRepository repository, ICurrentUserService currentUser,
        IValidator<CreateDegreeLevelRequest> createValidator, IValidator<UpdateDegreeLevelRequest> updateValidator)
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

    public async Task<ServiceResult<PagedResult<DegreeLevelDto>>> GetPagedAsync(GetLookupPagedRequest request, CancellationToken cancellationToken = default)
    {
        try
        {
            var page = await _repository.GetPagedAsync(request, cancellationToken).ConfigureAwait(false);
            return ServiceResult<PagedResult<DegreeLevelDto>>.Success(page);
        }
        catch (Exception ex) { return HandleException<PagedResult<DegreeLevelDto>>(ex); }
    }

    public async Task<ServiceResult<DegreeLevelDto>> GetByIdAsync(int degreeLevelId, CancellationToken cancellationToken = default)
    {
        try
        {
            var entity = await _repository.GetByIdAsync(degreeLevelId, cancellationToken).ConfigureAwait(false);
            if (entity is null) throw new NotFoundException("DegreeLevel", degreeLevelId);
            return ServiceResult<DegreeLevelDto>.Success(DegreeLevelMapper.ToDto(entity));
        }
        catch (Exception ex) { return HandleException<DegreeLevelDto>(ex); }
    }

    public async Task<ServiceResult<CommandResultDto>> CreateAsync(CreateDegreeLevelRequest request, CancellationToken cancellationToken = default)
    {
        try
        {
            await ValidateAsync(_createValidator, request, cancellationToken).ConfigureAwait(false);
            var result = await _repository.InsertAsync(DegreeLevelMapper.ToEntity(request), _currentUser.UserId, cancellationToken).ConfigureAwait(false);
            SpResultHelper.EnsureSuccess(result);
            return ServiceResult<CommandResultDto>.Success(SpResultHelper.ToCommandResult(result));
        }
        catch (Exception ex) { return HandleException<CommandResultDto>(ex); }
    }

    public async Task<ServiceResult<CommandResultDto>> UpdateAsync(UpdateDegreeLevelRequest request, CancellationToken cancellationToken = default)
    {
        try
        {
            await ValidateAsync(_updateValidator, request, cancellationToken).ConfigureAwait(false);
            var entity = await _repository.GetByIdAsync(request.DegreeLevelId, cancellationToken).ConfigureAwait(false);
            if (entity is null) throw new NotFoundException("DegreeLevel", request.DegreeLevelId);

            DegreeLevelMapper.ApplyUpdate(entity, request);
            var result = await _repository.UpdateAsync(entity, _currentUser.UserId, cancellationToken).ConfigureAwait(false);
            SpResultHelper.EnsureSuccess(result);
            return ServiceResult<CommandResultDto>.Success(SpResultHelper.ToCommandResult(result));
        }
        catch (Exception ex) { return HandleException<CommandResultDto>(ex); }
    }

    public async Task<ServiceResult> DeleteAsync(int degreeLevelId, CancellationToken cancellationToken = default)
    {
        try
        {
            var result = await _repository.DeleteAsync(degreeLevelId, _currentUser.UserId, cancellationToken).ConfigureAwait(false);
            SpResultHelper.EnsureSuccess(result);
            return ServiceResult.Success();
        }
        catch (Exception ex) { return HandleException(ex); }
    }
}
