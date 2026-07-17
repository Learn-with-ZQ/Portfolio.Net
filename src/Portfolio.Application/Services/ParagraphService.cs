using FluentValidation;
using Portfolio.Application.Abstractions.Persistence;
using Portfolio.Application.Common.BusinessRules;
using Portfolio.Application.Common.Helpers;
using Portfolio.Application.Common.Interfaces;
using Portfolio.Application.Common.Models;
using Portfolio.Application.Common.Results;
using Portfolio.Application.DTOs.Paragraphs;
using Portfolio.Application.Mappings;
using Portfolio.Application.Services.Interfaces;
using Portfolio.Domain.Exceptions;

namespace Portfolio.Application.Services;

public sealed class ParagraphService : ApplicationServiceBase, IParagraphService
{
    private readonly IParagraphRepository _repository;
    private readonly ICurrentUserService _currentUser;
    private readonly IValidator<CreateParagraphRequest> _createValidator;
    private readonly IValidator<UpdateParagraphRequest> _updateValidator;

    public ParagraphService(
        IParagraphRepository repository,
        ICurrentUserService currentUser,
        IValidator<CreateParagraphRequest> createValidator,
        IValidator<UpdateParagraphRequest> updateValidator)
    {
        _repository = repository;
        _currentUser = currentUser;
        _createValidator = createValidator;
        _updateValidator = updateValidator;
    }

    public async Task<ServiceResult<ParagraphDetailDto>> GetByIdAsync(int paragraphId, CancellationToken cancellationToken = default)
    {
        try
        {
            var entity = await _repository.GetByIdAsync(paragraphId, cancellationToken).ConfigureAwait(false);
            if (entity is null) throw new NotFoundException("Paragraph", paragraphId);
            return ServiceResult<ParagraphDetailDto>.Success(ParagraphMapper.ToDetailDto(entity));
        }
        catch (Exception ex) { return HandleException<ParagraphDetailDto>(ex); }
    }

    public async Task<ServiceResult<CommandResultDto>> CreateAsync(CreateParagraphRequest request, CancellationToken cancellationToken = default)
    {
        try
        {
            await ValidateAsync(_createValidator, request, cancellationToken).ConfigureAwait(false);
            PortfolioProfileRules.EnsureValidProfileId(request.PortfolioProfileId);
            var result = await _repository.InsertAsync(ParagraphMapper.ToEntity(request), _currentUser.UserId, cancellationToken).ConfigureAwait(false);
            SpResultHelper.EnsureSuccess(result);
            return ServiceResult<CommandResultDto>.Success(SpResultHelper.ToCommandResult(result));
        }
        catch (Exception ex) { return HandleException<CommandResultDto>(ex); }
    }

    public async Task<ServiceResult<CommandResultDto>> UpdateAsync(UpdateParagraphRequest request, CancellationToken cancellationToken = default)
    {
        try
        {
            await ValidateAsync(_updateValidator, request, cancellationToken).ConfigureAwait(false);
            var entity = await _repository.GetByIdAsync(request.ParagraphId, cancellationToken).ConfigureAwait(false);
            if (entity is null) throw new NotFoundException("Paragraph", request.ParagraphId);

            ParagraphMapper.ApplyUpdate(entity, request);
            var result = await _repository.UpdateAsync(entity, _currentUser.UserId, cancellationToken).ConfigureAwait(false);
            SpResultHelper.EnsureSuccess(result);
            return ServiceResult<CommandResultDto>.Success(SpResultHelper.ToCommandResult(result));
        }
        catch (Exception ex) { return HandleException<CommandResultDto>(ex); }
    }

    public async Task<ServiceResult> DeleteAsync(int paragraphId, CancellationToken cancellationToken = default)
    {
        try
        {
            SpResultHelper.EnsureSuccess(await _repository.DeleteAsync(paragraphId, _currentUser.UserId, cancellationToken).ConfigureAwait(false));
            return ServiceResult.Success();
        }
        catch (Exception ex) { return HandleException(ex); }
    }

    public async Task<ServiceResult<PagedResult<ParagraphDto>>> GetPagedAsync(GetParagraphPagedRequest request, CancellationToken cancellationToken = default)
    {
        try
        {
            PortfolioProfileRules.EnsureValidProfileId(request.PortfolioProfileId);
            return ServiceResult<PagedResult<ParagraphDto>>.Success(
                await _repository.GetPagedAsync(request, cancellationToken).ConfigureAwait(false));
        }
        catch (Exception ex) { return HandleException<PagedResult<ParagraphDto>>(ex); }
    }
}
