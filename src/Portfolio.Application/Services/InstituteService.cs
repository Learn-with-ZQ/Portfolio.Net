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

public sealed class InstituteService : ApplicationServiceBase, IInstituteService
{
    private readonly IInstituteRepository _repository;
    private readonly ICurrentUserService _currentUser;
    private readonly IValidator<CreateInstituteRequest> _createValidator;
    private readonly IValidator<UpdateInstituteRequest> _updateValidator;

    public InstituteService(IInstituteRepository repository, ICurrentUserService currentUser,
        IValidator<CreateInstituteRequest> createValidator, IValidator<UpdateInstituteRequest> updateValidator)
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

    public async Task<ServiceResult<PagedResult<InstituteDto>>> GetPagedAsync(GetLookupPagedRequest request, CancellationToken cancellationToken = default)
    {
        try
        {
            var page = await _repository.GetPagedAsync(request, cancellationToken).ConfigureAwait(false);
            return ServiceResult<PagedResult<InstituteDto>>.Success(page);
        }
        catch (Exception ex) { return HandleException<PagedResult<InstituteDto>>(ex); }
    }

    public async Task<ServiceResult<InstituteDto>> GetByIdAsync(int instituteId, CancellationToken cancellationToken = default)
    {
        try
        {
            var entity = await _repository.GetByIdAsync(instituteId, cancellationToken).ConfigureAwait(false);
            if (entity is null) throw new NotFoundException("Institute", instituteId);
            return ServiceResult<InstituteDto>.Success(InstituteMapper.ToDto(entity));
        }
        catch (Exception ex) { return HandleException<InstituteDto>(ex); }
    }

    public async Task<ServiceResult<CommandResultDto>> CreateAsync(CreateInstituteRequest request, CancellationToken cancellationToken = default)
    {
        try
        {
            await ValidateAsync(_createValidator, request, cancellationToken).ConfigureAwait(false);
            var result = await _repository.InsertAsync(InstituteMapper.ToEntity(request), _currentUser.UserId, cancellationToken).ConfigureAwait(false);
            SpResultHelper.EnsureSuccess(result);
            return ServiceResult<CommandResultDto>.Success(SpResultHelper.ToCommandResult(result));
        }
        catch (Exception ex) { return HandleException<CommandResultDto>(ex); }
    }

    public async Task<ServiceResult<CommandResultDto>> UpdateAsync(UpdateInstituteRequest request, CancellationToken cancellationToken = default)
    {
        try
        {
            await ValidateAsync(_updateValidator, request, cancellationToken).ConfigureAwait(false);
            var entity = await _repository.GetByIdAsync(request.InstituteId, cancellationToken).ConfigureAwait(false);
            if (entity is null) throw new NotFoundException("Institute", request.InstituteId);

            InstituteMapper.ApplyUpdate(entity, request);
            var result = await _repository.UpdateAsync(entity, _currentUser.UserId, cancellationToken).ConfigureAwait(false);
            SpResultHelper.EnsureSuccess(result);
            return ServiceResult<CommandResultDto>.Success(SpResultHelper.ToCommandResult(result));
        }
        catch (Exception ex) { return HandleException<CommandResultDto>(ex); }
    }

    public async Task<ServiceResult> DeleteAsync(int instituteId, CancellationToken cancellationToken = default)
    {
        try
        {
            var result = await _repository.DeleteAsync(instituteId, _currentUser.UserId, cancellationToken).ConfigureAwait(false);
            SpResultHelper.EnsureSuccess(result);
            return ServiceResult.Success();
        }
        catch (Exception ex) { return HandleException(ex); }
    }
}
