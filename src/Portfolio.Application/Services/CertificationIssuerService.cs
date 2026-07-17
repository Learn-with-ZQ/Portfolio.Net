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

public sealed class CertificationIssuerService : ApplicationServiceBase, ICertificationIssuerService
{
    private readonly ICertificationIssuerRepository _repository;
    private readonly ICurrentUserService _currentUser;
    private readonly IValidator<CreateCertificationIssuerRequest> _createValidator;
    private readonly IValidator<UpdateCertificationIssuerRequest> _updateValidator;

    public CertificationIssuerService(ICertificationIssuerRepository repository, ICurrentUserService currentUser,
        IValidator<CreateCertificationIssuerRequest> createValidator, IValidator<UpdateCertificationIssuerRequest> updateValidator)
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

    public async Task<ServiceResult<PagedResult<CertificationIssuerDto>>> GetPagedAsync(GetLookupPagedRequest request, CancellationToken cancellationToken = default)
    {
        try
        {
            var page = await _repository.GetPagedAsync(request, cancellationToken).ConfigureAwait(false);
            return ServiceResult<PagedResult<CertificationIssuerDto>>.Success(page);
        }
        catch (Exception ex) { return HandleException<PagedResult<CertificationIssuerDto>>(ex); }
    }

    public async Task<ServiceResult<CertificationIssuerDto>> GetByIdAsync(int certificationIssuerId, CancellationToken cancellationToken = default)
    {
        try
        {
            var entity = await _repository.GetByIdAsync(certificationIssuerId, cancellationToken).ConfigureAwait(false);
            if (entity is null) throw new NotFoundException("CertificationIssuer", certificationIssuerId);
            return ServiceResult<CertificationIssuerDto>.Success(CertificationIssuerMapper.ToDto(entity));
        }
        catch (Exception ex) { return HandleException<CertificationIssuerDto>(ex); }
    }

    public async Task<ServiceResult<CommandResultDto>> CreateAsync(CreateCertificationIssuerRequest request, CancellationToken cancellationToken = default)
    {
        try
        {
            await ValidateAsync(_createValidator, request, cancellationToken).ConfigureAwait(false);
            var result = await _repository.InsertAsync(CertificationIssuerMapper.ToEntity(request), _currentUser.UserId, cancellationToken).ConfigureAwait(false);
            SpResultHelper.EnsureSuccess(result);
            return ServiceResult<CommandResultDto>.Success(SpResultHelper.ToCommandResult(result));
        }
        catch (Exception ex) { return HandleException<CommandResultDto>(ex); }
    }

    public async Task<ServiceResult<CommandResultDto>> UpdateAsync(UpdateCertificationIssuerRequest request, CancellationToken cancellationToken = default)
    {
        try
        {
            await ValidateAsync(_updateValidator, request, cancellationToken).ConfigureAwait(false);
            var entity = await _repository.GetByIdAsync(request.CertificationIssuerId, cancellationToken).ConfigureAwait(false);
            if (entity is null) throw new NotFoundException("CertificationIssuer", request.CertificationIssuerId);

            CertificationIssuerMapper.ApplyUpdate(entity, request);
            var result = await _repository.UpdateAsync(entity, _currentUser.UserId, cancellationToken).ConfigureAwait(false);
            SpResultHelper.EnsureSuccess(result);
            return ServiceResult<CommandResultDto>.Success(SpResultHelper.ToCommandResult(result));
        }
        catch (Exception ex) { return HandleException<CommandResultDto>(ex); }
    }

    public async Task<ServiceResult> DeleteAsync(int certificationIssuerId, CancellationToken cancellationToken = default)
    {
        try
        {
            var result = await _repository.DeleteAsync(certificationIssuerId, _currentUser.UserId, cancellationToken).ConfigureAwait(false);
            SpResultHelper.EnsureSuccess(result);
            return ServiceResult.Success();
        }
        catch (Exception ex) { return HandleException(ex); }
    }
}
