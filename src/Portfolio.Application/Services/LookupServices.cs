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

public sealed class CompanyService : ApplicationServiceBase, ICompanyService
{
    private readonly ICompanyRepository _repository;
    private readonly ICurrentUserService _currentUser;
    private readonly IValidator<CreateCompanyRequest> _createValidator;
    private readonly IValidator<UpdateCompanyRequest> _updateValidator;

    public CompanyService(ICompanyRepository repository, ICurrentUserService currentUser,
        IValidator<CreateCompanyRequest> createValidator, IValidator<UpdateCompanyRequest> updateValidator)
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

    public async Task<ServiceResult<PagedResult<CompanyDto>>> GetPagedAsync(GetLookupPagedRequest request, CancellationToken cancellationToken = default)
    {
        try
        {
            var page = await _repository.GetPagedAsync(request, cancellationToken).ConfigureAwait(false);
            return ServiceResult<PagedResult<CompanyDto>>.Success(page);
        }
        catch (Exception ex) { return HandleException<PagedResult<CompanyDto>>(ex); }
    }

    public async Task<ServiceResult<CompanyDto>> GetByIdAsync(int companyId, CancellationToken cancellationToken = default)
    {
        try
        {
            var entity = await _repository.GetByIdAsync(companyId, cancellationToken).ConfigureAwait(false);
            if (entity is null) throw new NotFoundException("Company", companyId);
            return ServiceResult<CompanyDto>.Success(CompanyMapper.ToDto(entity));
        }
        catch (Exception ex) { return HandleException<CompanyDto>(ex); }
    }

    public async Task<ServiceResult<CommandResultDto>> CreateAsync(CreateCompanyRequest request, CancellationToken cancellationToken = default)
    {
        try
        {
            await ValidateAsync(_createValidator, request, cancellationToken).ConfigureAwait(false);
            var result = await _repository.InsertAsync(CompanyMapper.ToEntity(request), _currentUser.UserId, cancellationToken).ConfigureAwait(false);
            SpResultHelper.EnsureSuccess(result);
            return ServiceResult<CommandResultDto>.Success(SpResultHelper.ToCommandResult(result));
        }
        catch (Exception ex) { return HandleException<CommandResultDto>(ex); }
    }

    public async Task<ServiceResult<CommandResultDto>> UpdateAsync(UpdateCompanyRequest request, CancellationToken cancellationToken = default)
    {
        try
        {
            await ValidateAsync(_updateValidator, request, cancellationToken).ConfigureAwait(false);
            var entity = await _repository.GetByIdAsync(request.CompanyId, cancellationToken).ConfigureAwait(false);
            if (entity is null) throw new NotFoundException("Company", request.CompanyId);

            CompanyMapper.ApplyUpdate(entity, request);
            var result = await _repository.UpdateAsync(entity, _currentUser.UserId, cancellationToken).ConfigureAwait(false);
            SpResultHelper.EnsureSuccess(result);
            return ServiceResult<CommandResultDto>.Success(SpResultHelper.ToCommandResult(result));
        }
        catch (Exception ex) { return HandleException<CommandResultDto>(ex); }
    }

    public async Task<ServiceResult> DeleteAsync(int companyId, CancellationToken cancellationToken = default)
    {
        try
        {
            var result = await _repository.DeleteAsync(companyId, _currentUser.UserId, cancellationToken).ConfigureAwait(false);
            SpResultHelper.EnsureSuccess(result);
            return ServiceResult.Success();
        }
        catch (Exception ex) { return HandleException(ex); }
    }
}

public sealed class DeployDetailService : ApplicationServiceBase, IDeployDetailService
{
    private readonly IDeployDetailRepository _repository;
    private readonly ICurrentUserService _currentUser;
    private readonly IValidator<CreateDeployDetailRequest> _createValidator;
    private readonly IValidator<UpdateDeployDetailRequest> _updateValidator;

    public DeployDetailService(IDeployDetailRepository repository, ICurrentUserService currentUser,
        IValidator<CreateDeployDetailRequest> createValidator, IValidator<UpdateDeployDetailRequest> updateValidator)
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

    public async Task<ServiceResult<PagedResult<DeployDetailDto>>> GetPagedAsync(GetLookupPagedRequest request, CancellationToken cancellationToken = default)
    {
        try
        {
            var page = await _repository.GetPagedAsync(request, cancellationToken).ConfigureAwait(false);
            return ServiceResult<PagedResult<DeployDetailDto>>.Success(page);
        }
        catch (Exception ex) { return HandleException<PagedResult<DeployDetailDto>>(ex); }
    }

    public async Task<ServiceResult<DeployDetailDto>> GetByIdAsync(int deployDetailId, CancellationToken cancellationToken = default)
    {
        try
        {
            var entity = await _repository.GetByIdAsync(deployDetailId, cancellationToken).ConfigureAwait(false);
            if (entity is null) throw new NotFoundException("DeployDetail", deployDetailId);
            return ServiceResult<DeployDetailDto>.Success(DeployDetailMapper.ToDto(entity));
        }
        catch (Exception ex) { return HandleException<DeployDetailDto>(ex); }
    }

    public async Task<ServiceResult<CommandResultDto>> CreateAsync(CreateDeployDetailRequest request, CancellationToken cancellationToken = default)
    {
        try
        {
            await ValidateAsync(_createValidator, request, cancellationToken).ConfigureAwait(false);
            var result = await _repository.InsertAsync(DeployDetailMapper.ToEntity(request), _currentUser.UserId, cancellationToken).ConfigureAwait(false);
            SpResultHelper.EnsureSuccess(result);
            return ServiceResult<CommandResultDto>.Success(SpResultHelper.ToCommandResult(result));
        }
        catch (Exception ex) { return HandleException<CommandResultDto>(ex); }
    }

    public async Task<ServiceResult<CommandResultDto>> UpdateAsync(UpdateDeployDetailRequest request, CancellationToken cancellationToken = default)
    {
        try
        {
            await ValidateAsync(_updateValidator, request, cancellationToken).ConfigureAwait(false);
            var entity = await _repository.GetByIdAsync(request.DeployDetailId, cancellationToken).ConfigureAwait(false);
            if (entity is null) throw new NotFoundException("DeployDetail", request.DeployDetailId);

            DeployDetailMapper.ApplyUpdate(entity, request);
            var result = await _repository.UpdateAsync(entity, _currentUser.UserId, cancellationToken).ConfigureAwait(false);
            SpResultHelper.EnsureSuccess(result);
            return ServiceResult<CommandResultDto>.Success(SpResultHelper.ToCommandResult(result));
        }
        catch (Exception ex) { return HandleException<CommandResultDto>(ex); }
    }

    public async Task<ServiceResult> DeleteAsync(int deployDetailId, CancellationToken cancellationToken = default)
    {
        try
        {
            var result = await _repository.DeleteAsync(deployDetailId, _currentUser.UserId, cancellationToken).ConfigureAwait(false);
            SpResultHelper.EnsureSuccess(result);
            return ServiceResult.Success();
        }
        catch (Exception ex) { return HandleException(ex); }
    }
}
