using FluentValidation;
using Portfolio.Application.Abstractions.Persistence;
using Portfolio.Application.Common.BusinessRules;
using Portfolio.Application.Common.Helpers;
using Portfolio.Application.Common.Interfaces;
using Portfolio.Application.Common.Models;
using Portfolio.Application.Common.Results;
using Portfolio.Application.DTOs.References;
using Portfolio.Application.Mappings;
using Portfolio.Application.Services.Interfaces;
using Portfolio.Domain.Exceptions;

namespace Portfolio.Application.Services;

public sealed class ReferenceService : ApplicationServiceBase, IReferenceService
{
    private readonly IReferenceRepository _repository;
    private readonly ICurrentUserService _currentUser;
    private readonly IValidator<CreateReferenceRequest> _createValidator;
    private readonly IValidator<UpdateReferenceRequest> _updateValidator;

    public ReferenceService(
        IReferenceRepository repository,
        ICurrentUserService currentUser,
        IValidator<CreateReferenceRequest> createValidator,
        IValidator<UpdateReferenceRequest> updateValidator)
    {
        _repository = repository;
        _currentUser = currentUser;
        _createValidator = createValidator;
        _updateValidator = updateValidator;
    }

    public async Task<ServiceResult<ReferenceDetailDto>> GetByIdAsync(int referenceId, CancellationToken cancellationToken = default)
    {
        try
        {
            var entity = await _repository.GetByIdAsync(referenceId, cancellationToken).ConfigureAwait(false);
            if (entity is null) throw new NotFoundException("Reference", referenceId);
            return ServiceResult<ReferenceDetailDto>.Success(ReferenceMapper.ToDetailDto(entity));
        }
        catch (Exception ex) { return HandleException<ReferenceDetailDto>(ex); }
    }

    public async Task<ServiceResult<CommandResultDto>> CreateAsync(CreateReferenceRequest request, CancellationToken cancellationToken = default)
    {
        try
        {
            await ValidateAsync(_createValidator, request, cancellationToken).ConfigureAwait(false);
            PortfolioProfileRules.EnsureValidProfileId(request.PortfolioProfileId);
            var result = await _repository.InsertAsync(ReferenceMapper.ToEntity(request), _currentUser.UserId, cancellationToken).ConfigureAwait(false);
            SpResultHelper.EnsureSuccess(result);
            return ServiceResult<CommandResultDto>.Success(SpResultHelper.ToCommandResult(result));
        }
        catch (Exception ex) { return HandleException<CommandResultDto>(ex); }
    }

    public async Task<ServiceResult<CommandResultDto>> UpdateAsync(UpdateReferenceRequest request, CancellationToken cancellationToken = default)
    {
        try
        {
            await ValidateAsync(_updateValidator, request, cancellationToken).ConfigureAwait(false);
            var entity = await _repository.GetByIdAsync(request.ReferenceId, cancellationToken).ConfigureAwait(false);
            if (entity is null) throw new NotFoundException("Reference", request.ReferenceId);

            ReferenceMapper.ApplyUpdate(entity, request);
            var result = await _repository.UpdateAsync(entity, _currentUser.UserId, cancellationToken).ConfigureAwait(false);
            SpResultHelper.EnsureSuccess(result);
            return ServiceResult<CommandResultDto>.Success(SpResultHelper.ToCommandResult(result));
        }
        catch (Exception ex) { return HandleException<CommandResultDto>(ex); }
    }

    public async Task<ServiceResult> DeleteAsync(int referenceId, CancellationToken cancellationToken = default)
    {
        try
        {
            SpResultHelper.EnsureSuccess(await _repository.DeleteAsync(referenceId, _currentUser.UserId, cancellationToken).ConfigureAwait(false));
            return ServiceResult.Success();
        }
        catch (Exception ex) { return HandleException(ex); }
    }

    public async Task<ServiceResult<PagedResult<ReferenceDto>>> GetPagedAsync(GetReferencePagedRequest request, CancellationToken cancellationToken = default)
    {
        try
        {
            PortfolioProfileRules.EnsureValidProfileId(request.PortfolioProfileId);
            return ServiceResult<PagedResult<ReferenceDto>>.Success(
                await _repository.GetPagedAsync(request, cancellationToken).ConfigureAwait(false));
        }
        catch (Exception ex) { return HandleException<PagedResult<ReferenceDto>>(ex); }
    }
}
