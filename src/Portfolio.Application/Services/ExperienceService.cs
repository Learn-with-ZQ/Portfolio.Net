using FluentValidation;
using Portfolio.Application.Abstractions.Persistence;
using Portfolio.Application.Common.BusinessRules;
using Portfolio.Application.Common.Helpers;
using Portfolio.Application.Common.Interfaces;
using Portfolio.Application.Common.Models;
using Portfolio.Application.Common.Results;
using Portfolio.Application.DTOs.Experience;
using Portfolio.Application.Mappings;
using Portfolio.Application.Services.Interfaces;
using Portfolio.Domain.Exceptions;

namespace Portfolio.Application.Services;

public sealed class ExperienceService : ApplicationServiceBase, IExperienceService
{
    private readonly IExperienceRepository _repository;
    private readonly ICurrentUserService _currentUser;
    private readonly IValidator<CreateExperienceRequest> _createValidator;
    private readonly IValidator<UpdateExperienceRequest> _updateValidator;
    private readonly IValidator<CreateExperienceDetailRequest> _createDetailValidator;
    private readonly IValidator<UpdateExperienceDetailRequest> _updateDetailValidator;
    private readonly IValidator<SearchExperienceRequest> _searchValidator;
    private readonly IValidator<GetExperiencePagedRequest> _pagedValidator;

    public ExperienceService(
        IExperienceRepository repository,
        ICurrentUserService currentUser,
        IValidator<CreateExperienceRequest> createValidator,
        IValidator<UpdateExperienceRequest> updateValidator,
        IValidator<CreateExperienceDetailRequest> createDetailValidator,
        IValidator<UpdateExperienceDetailRequest> updateDetailValidator,
        IValidator<SearchExperienceRequest> searchValidator,
        IValidator<GetExperiencePagedRequest> pagedValidator)
    {
        _repository = repository;
        _currentUser = currentUser;
        _createValidator = createValidator;
        _updateValidator = updateValidator;
        _createDetailValidator = createDetailValidator;
        _updateDetailValidator = updateDetailValidator;
        _searchValidator = searchValidator;
        _pagedValidator = pagedValidator;
    }

    public async Task<ServiceResult<ExperienceDetailDto>> GetByIdAsync(int experienceId, CancellationToken cancellationToken = default)
    {
        try
        {
            if (experienceId <= 0)
            {
                return ServiceResult<ExperienceDetailDto>.Failure("Experience id is required.");
            }

            var entity = await _repository.GetByIdAsync(experienceId, cancellationToken).ConfigureAwait(false);
            if (entity is null)
            {
                throw new NotFoundException("Experience", experienceId);
            }

            return ServiceResult<ExperienceDetailDto>.Success(ExperienceMapper.ToDetailDto(entity));
        }
        catch (Exception ex)
        {
            return HandleException<ExperienceDetailDto>(ex);
        }
    }

    public async Task<ServiceResult<CommandResultDto>> CreateAsync(CreateExperienceRequest request, CancellationToken cancellationToken = default)
    {
        try
        {
            await ValidateAsync(_createValidator, request, cancellationToken).ConfigureAwait(false);
            PortfolioProfileRules.EnsureValidProfileId(request.PortfolioProfileId);
            DateRangeRules.EnsureValidRange(request.StartDate, request.EndDate, "Experience");

            var entity = ExperienceMapper.ToEntity(request);
            var result = await _repository.InsertAsync(entity, _currentUser.UserId, cancellationToken).ConfigureAwait(false);
            SpResultHelper.EnsureSuccess(result);

            return ServiceResult<CommandResultDto>.Success(SpResultHelper.ToCommandResult(result));
        }
        catch (Exception ex)
        {
            return HandleException<CommandResultDto>(ex);
        }
    }

    public async Task<ServiceResult<CommandResultDto>> UpdateAsync(UpdateExperienceRequest request, CancellationToken cancellationToken = default)
    {
        try
        {
            await ValidateAsync(_updateValidator, request, cancellationToken).ConfigureAwait(false);
            DateRangeRules.EnsureValidRange(request.StartDate, request.EndDate, "Experience");

            var existing = await _repository.GetByIdAsync(request.ExperienceId, cancellationToken).ConfigureAwait(false);
            if (existing is null)
            {
                throw new NotFoundException("Experience", request.ExperienceId);
            }

            ExperienceMapper.ApplyUpdate(existing, request);
            var result = await _repository.UpdateAsync(existing, _currentUser.UserId, cancellationToken).ConfigureAwait(false);
            SpResultHelper.EnsureSuccess(result);

            return ServiceResult<CommandResultDto>.Success(SpResultHelper.ToCommandResult(result));
        }
        catch (Exception ex)
        {
            return HandleException<CommandResultDto>(ex);
        }
    }

    public async Task<ServiceResult> DeleteAsync(int experienceId, CancellationToken cancellationToken = default)
    {
        try
        {
            var result = await _repository.DeleteAsync(experienceId, _currentUser.UserId, cancellationToken).ConfigureAwait(false);
            SpResultHelper.EnsureSuccess(result);
            return ServiceResult.Success();
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    public async Task<ServiceResult<IReadOnlyList<ExperienceDto>>> SearchAsync(SearchExperienceRequest request, CancellationToken cancellationToken = default)
    {
        try
        {
            await ValidateAsync(_searchValidator, request, cancellationToken).ConfigureAwait(false);
            PortfolioProfileRules.EnsureValidProfileId(request.PortfolioProfileId);

            var items = await _repository.SearchAsync(request, cancellationToken).ConfigureAwait(false);
            return ServiceResult<IReadOnlyList<ExperienceDto>>.Success(items);
        }
        catch (Exception ex)
        {
            return HandleException<IReadOnlyList<ExperienceDto>>(ex);
        }
    }

    public async Task<ServiceResult<PagedResult<ExperienceDto>>> GetPagedAsync(GetExperiencePagedRequest request, CancellationToken cancellationToken = default)
    {
        try
        {
            await ValidateAsync(_pagedValidator, request, cancellationToken).ConfigureAwait(false);
            PortfolioProfileRules.EnsureValidProfileId(request.PortfolioProfileId);

            var page = await _repository.GetPagedAsync(request, cancellationToken).ConfigureAwait(false);
            return ServiceResult<PagedResult<ExperienceDto>>.Success(page);
        }
        catch (Exception ex)
        {
            return HandleException<PagedResult<ExperienceDto>>(ex);
        }
    }

    public async Task<ServiceResult<CommandResultDto>> CreateDetailAsync(CreateExperienceDetailRequest request, CancellationToken cancellationToken = default)
    {
        try
        {
            await ValidateAsync(_createDetailValidator, request, cancellationToken).ConfigureAwait(false);

            var parent = await _repository.GetByIdAsync(request.ExperienceId, cancellationToken).ConfigureAwait(false);
            if (parent is null)
            {
                throw new NotFoundException("Experience", request.ExperienceId);
            }

            var detail = ExperienceMapper.ToDetailEntity(request);
            var result = await _repository.InsertDetailAsync(detail, _currentUser.UserId, cancellationToken).ConfigureAwait(false);
            SpResultHelper.EnsureSuccess(result);

            return ServiceResult<CommandResultDto>.Success(SpResultHelper.ToCommandResult(result));
        }
        catch (Exception ex)
        {
            return HandleException<CommandResultDto>(ex);
        }
    }

    public async Task<ServiceResult<CommandResultDto>> UpdateDetailAsync(UpdateExperienceDetailRequest request, CancellationToken cancellationToken = default)
    {
        try
        {
            await ValidateAsync(_updateDetailValidator, request, cancellationToken).ConfigureAwait(false);

            var detail = ExperienceMapper.ToDetailEntity(new CreateExperienceDetailRequest
            {
                ExperienceId = 0,
                ExperienceDetailName = request.ExperienceDetailName,
                SortOrder = request.SortOrder
            });
            detail.ExperienceDetailId = request.ExperienceDetailId;
            detail.RowVersion = request.RowVersion;

            var result = await _repository.UpdateDetailAsync(detail, _currentUser.UserId, cancellationToken).ConfigureAwait(false);
            SpResultHelper.EnsureSuccess(result);

            return ServiceResult<CommandResultDto>.Success(SpResultHelper.ToCommandResult(result));
        }
        catch (Exception ex)
        {
            return HandleException<CommandResultDto>(ex);
        }
    }

    public async Task<ServiceResult> DeleteDetailAsync(int experienceDetailId, CancellationToken cancellationToken = default)
    {
        try
        {
            var result = await _repository.DeleteDetailAsync(experienceDetailId, _currentUser.UserId, cancellationToken).ConfigureAwait(false);
            SpResultHelper.EnsureSuccess(result);
            return ServiceResult.Success();
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }
}
