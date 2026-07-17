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

public sealed class TechnologyService : ApplicationServiceBase, ITechnologyService
{
    private readonly ITechnologyRepository _repository;
    private readonly ICurrentUserService _currentUser;
    private readonly IValidator<CreateTechnologyRequest> _createValidator;
    private readonly IValidator<UpdateTechnologyRequest> _updateValidator;

    public TechnologyService(ITechnologyRepository repository, ICurrentUserService currentUser,
        IValidator<CreateTechnologyRequest> createValidator, IValidator<UpdateTechnologyRequest> updateValidator)
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

    public async Task<ServiceResult<PagedResult<TechnologyDto>>> GetPagedAsync(GetLookupPagedRequest request, CancellationToken cancellationToken = default)
    {
        try
        {
            var page = await _repository.GetPagedAsync(request, cancellationToken).ConfigureAwait(false);
            return ServiceResult<PagedResult<TechnologyDto>>.Success(page);
        }
        catch (Exception ex) { return HandleException<PagedResult<TechnologyDto>>(ex); }
    }

    public async Task<ServiceResult<TechnologyDto>> GetByIdAsync(int technologyId, CancellationToken cancellationToken = default)
    {
        try
        {
            var entity = await _repository.GetByIdAsync(technologyId, cancellationToken).ConfigureAwait(false);
            if (entity is null) throw new NotFoundException("Technology", technologyId);
            return ServiceResult<TechnologyDto>.Success(TechnologyMapper.ToDto(entity));
        }
        catch (Exception ex) { return HandleException<TechnologyDto>(ex); }
    }

    public async Task<ServiceResult<CommandResultDto>> CreateAsync(CreateTechnologyRequest request, CancellationToken cancellationToken = default)
    {
        try
        {
            await ValidateAsync(_createValidator, request, cancellationToken).ConfigureAwait(false);
            var result = await _repository.InsertAsync(TechnologyMapper.ToEntity(request), _currentUser.UserId, cancellationToken).ConfigureAwait(false);
            SpResultHelper.EnsureSuccess(result);
            return ServiceResult<CommandResultDto>.Success(SpResultHelper.ToCommandResult(result));
        }
        catch (Exception ex) { return HandleException<CommandResultDto>(ex); }
    }

    public async Task<ServiceResult<CommandResultDto>> UpdateAsync(UpdateTechnologyRequest request, CancellationToken cancellationToken = default)
    {
        try
        {
            await ValidateAsync(_updateValidator, request, cancellationToken).ConfigureAwait(false);
            var entity = await _repository.GetByIdAsync(request.TechnologyId, cancellationToken).ConfigureAwait(false);
            if (entity is null) throw new NotFoundException("Technology", request.TechnologyId);

            TechnologyMapper.ApplyUpdate(entity, request);
            var result = await _repository.UpdateAsync(entity, _currentUser.UserId, cancellationToken).ConfigureAwait(false);
            SpResultHelper.EnsureSuccess(result);
            return ServiceResult<CommandResultDto>.Success(SpResultHelper.ToCommandResult(result));
        }
        catch (Exception ex) { return HandleException<CommandResultDto>(ex); }
    }

    public async Task<ServiceResult> DeleteAsync(int technologyId, CancellationToken cancellationToken = default)
    {
        try
        {
            var result = await _repository.DeleteAsync(technologyId, _currentUser.UserId, cancellationToken).ConfigureAwait(false);
            SpResultHelper.EnsureSuccess(result);
            return ServiceResult.Success();
        }
        catch (Exception ex) { return HandleException(ex); }
    }
}
