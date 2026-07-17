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

public sealed class DocumentTypeService : ApplicationServiceBase, IDocumentTypeService
{
    private readonly IDocumentTypeRepository _repository;
    private readonly ICurrentUserService _currentUser;
    private readonly IValidator<CreateDocumentTypeRequest> _createValidator;
    private readonly IValidator<UpdateDocumentTypeRequest> _updateValidator;

    public DocumentTypeService(IDocumentTypeRepository repository, ICurrentUserService currentUser,
        IValidator<CreateDocumentTypeRequest> createValidator, IValidator<UpdateDocumentTypeRequest> updateValidator)
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

    public async Task<ServiceResult<PagedResult<DocumentTypeDto>>> GetPagedAsync(GetLookupPagedRequest request, CancellationToken cancellationToken = default)
    {
        try
        {
            var page = await _repository.GetPagedAsync(request, cancellationToken).ConfigureAwait(false);
            return ServiceResult<PagedResult<DocumentTypeDto>>.Success(page);
        }
        catch (Exception ex) { return HandleException<PagedResult<DocumentTypeDto>>(ex); }
    }

    public async Task<ServiceResult<DocumentTypeDto>> GetByIdAsync(int documentTypeId, CancellationToken cancellationToken = default)
    {
        try
        {
            var entity = await _repository.GetByIdAsync(documentTypeId, cancellationToken).ConfigureAwait(false);
            if (entity is null) throw new NotFoundException("DocumentType", documentTypeId);
            return ServiceResult<DocumentTypeDto>.Success(DocumentTypeMapper.ToDto(entity));
        }
        catch (Exception ex) { return HandleException<DocumentTypeDto>(ex); }
    }

    public async Task<ServiceResult<CommandResultDto>> CreateAsync(CreateDocumentTypeRequest request, CancellationToken cancellationToken = default)
    {
        try
        {
            await ValidateAsync(_createValidator, request, cancellationToken).ConfigureAwait(false);
            var result = await _repository.InsertAsync(DocumentTypeMapper.ToEntity(request), _currentUser.UserId, cancellationToken).ConfigureAwait(false);
            SpResultHelper.EnsureSuccess(result);
            return ServiceResult<CommandResultDto>.Success(SpResultHelper.ToCommandResult(result));
        }
        catch (Exception ex) { return HandleException<CommandResultDto>(ex); }
    }

    public async Task<ServiceResult<CommandResultDto>> UpdateAsync(UpdateDocumentTypeRequest request, CancellationToken cancellationToken = default)
    {
        try
        {
            await ValidateAsync(_updateValidator, request, cancellationToken).ConfigureAwait(false);
            var entity = await _repository.GetByIdAsync(request.DocumentTypeId, cancellationToken).ConfigureAwait(false);
            if (entity is null) throw new NotFoundException("DocumentType", request.DocumentTypeId);

            DocumentTypeMapper.ApplyUpdate(entity, request);
            var result = await _repository.UpdateAsync(entity, _currentUser.UserId, cancellationToken).ConfigureAwait(false);
            SpResultHelper.EnsureSuccess(result);
            return ServiceResult<CommandResultDto>.Success(SpResultHelper.ToCommandResult(result));
        }
        catch (Exception ex) { return HandleException<CommandResultDto>(ex); }
    }

    public async Task<ServiceResult> DeleteAsync(int documentTypeId, CancellationToken cancellationToken = default)
    {
        try
        {
            var result = await _repository.DeleteAsync(documentTypeId, _currentUser.UserId, cancellationToken).ConfigureAwait(false);
            SpResultHelper.EnsureSuccess(result);
            return ServiceResult.Success();
        }
        catch (Exception ex) { return HandleException(ex); }
    }
}
