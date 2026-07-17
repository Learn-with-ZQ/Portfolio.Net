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

public sealed class DegreeService : ApplicationServiceBase, IDegreeService
{
    private readonly IDegreeRepository _repository;
    private readonly ICurrentUserService _currentUser;
    private readonly IValidator<CreateDegreeRequest> _createValidator;
    private readonly IValidator<UpdateDegreeRequest> _updateValidator;

    public DegreeService(IDegreeRepository repository, ICurrentUserService currentUser,
        IValidator<CreateDegreeRequest> createValidator, IValidator<UpdateDegreeRequest> updateValidator)
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

    public async Task<ServiceResult<PagedResult<DegreeDto>>> GetPagedAsync(GetLookupPagedRequest request, CancellationToken cancellationToken = default)
    {
        try
        {
            var page = await _repository.GetPagedAsync(request, cancellationToken).ConfigureAwait(false);
            return ServiceResult<PagedResult<DegreeDto>>.Success(page);
        }
        catch (Exception ex) { return HandleException<PagedResult<DegreeDto>>(ex); }
    }

    public async Task<ServiceResult<DegreeDto>> GetByIdAsync(int degreeId, CancellationToken cancellationToken = default)
    {
        try
        {
            var entity = await _repository.GetByIdAsync(degreeId, cancellationToken).ConfigureAwait(false);
            if (entity is null) throw new NotFoundException("Degree", degreeId);
            return ServiceResult<DegreeDto>.Success(DegreeMapper.ToDto(entity));
        }
        catch (Exception ex) { return HandleException<DegreeDto>(ex); }
    }

    public async Task<ServiceResult<CommandResultDto>> CreateAsync(CreateDegreeRequest request, CancellationToken cancellationToken = default)
    {
        try
        {
            await ValidateAsync(_createValidator, request, cancellationToken).ConfigureAwait(false);
            var result = await _repository.InsertAsync(DegreeMapper.ToEntity(request), _currentUser.UserId, cancellationToken).ConfigureAwait(false);
            SpResultHelper.EnsureSuccess(result);
            return ServiceResult<CommandResultDto>.Success(SpResultHelper.ToCommandResult(result));
        }
        catch (Exception ex) { return HandleException<CommandResultDto>(ex); }
    }

    public async Task<ServiceResult<CommandResultDto>> UpdateAsync(UpdateDegreeRequest request, CancellationToken cancellationToken = default)
    {
        try
        {
            await ValidateAsync(_updateValidator, request, cancellationToken).ConfigureAwait(false);
            var entity = await _repository.GetByIdAsync(request.DegreeId, cancellationToken).ConfigureAwait(false);
            if (entity is null) throw new NotFoundException("Degree", request.DegreeId);

            DegreeMapper.ApplyUpdate(entity, request);
            var result = await _repository.UpdateAsync(entity, _currentUser.UserId, cancellationToken).ConfigureAwait(false);
            SpResultHelper.EnsureSuccess(result);
            return ServiceResult<CommandResultDto>.Success(SpResultHelper.ToCommandResult(result));
        }
        catch (Exception ex) { return HandleException<CommandResultDto>(ex); }
    }

    public async Task<ServiceResult> DeleteAsync(int degreeId, CancellationToken cancellationToken = default)
    {
        try
        {
            var result = await _repository.DeleteAsync(degreeId, _currentUser.UserId, cancellationToken).ConfigureAwait(false);
            SpResultHelper.EnsureSuccess(result);
            return ServiceResult.Success();
        }
        catch (Exception ex) { return HandleException(ex); }
    }
}
