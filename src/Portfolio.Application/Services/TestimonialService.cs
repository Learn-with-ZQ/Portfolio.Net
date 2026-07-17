using FluentValidation;
using Portfolio.Application.Abstractions.Persistence;
using Portfolio.Application.Common.BusinessRules;
using Portfolio.Application.Common.Helpers;
using Portfolio.Application.Common.Interfaces;
using Portfolio.Application.Common.Models;
using Portfolio.Application.Common.Results;
using Portfolio.Application.DTOs.Testimonials;
using Portfolio.Application.Mappings;
using Portfolio.Application.Services.Interfaces;
using Portfolio.Domain.Exceptions;

namespace Portfolio.Application.Services;

public sealed class TestimonialService : ApplicationServiceBase, ITestimonialService
{
    private readonly ITestimonialRepository _repository;
    private readonly ICurrentUserService _currentUser;
    private readonly IValidator<SubmitTestimonialRequest> _submitValidator;

    public TestimonialService(
        ITestimonialRepository repository,
        ICurrentUserService currentUser,
        IValidator<SubmitTestimonialRequest> submitValidator)
    {
        _repository = repository;
        _currentUser = currentUser;
        _submitValidator = submitValidator;
    }

    public async Task<ServiceResult<TestimonialDetailDto>> GetByIdAsync(int testimonialId, CancellationToken cancellationToken = default)
    {
        try
        {
            var entity = await _repository.GetByIdAsync(testimonialId, cancellationToken).ConfigureAwait(false);
            if (entity is null) throw new NotFoundException("Testimonial", testimonialId);
            return ServiceResult<TestimonialDetailDto>.Success(TestimonialMapper.ToDetailDto(entity));
        }
        catch (Exception ex) { return HandleException<TestimonialDetailDto>(ex); }
    }

    public async Task<ServiceResult<CommandResultDto>> SubmitAsync(SubmitTestimonialRequest request, CancellationToken cancellationToken = default)
    {
        try
        {
            await ValidateAsync(_submitValidator, request, cancellationToken).ConfigureAwait(false);
            PortfolioProfileRules.EnsureValidProfileId(request.PortfolioProfileId);
            var result = await _repository.SubmitAsync(TestimonialMapper.ToEntity(request), cancellationToken).ConfigureAwait(false);
            SpResultHelper.EnsureSuccess(result);
            return ServiceResult<CommandResultDto>.Success(SpResultHelper.ToCommandResult(result));
        }
        catch (Exception ex) { return HandleException<CommandResultDto>(ex); }
    }

    public Task<ServiceResult> ApproveAsync(int testimonialId, CancellationToken cancellationToken = default)
        => SetStatusAsync(testimonialId, 2, cancellationToken);

    public Task<ServiceResult> RejectAsync(int testimonialId, CancellationToken cancellationToken = default)
        => SetStatusAsync(testimonialId, 3, cancellationToken);

    private async Task<ServiceResult> SetStatusAsync(int testimonialId, int status, CancellationToken cancellationToken)
    {
        try
        {
            SpResultHelper.EnsureSuccess(
                await _repository.SetStatusAsync(testimonialId, status, _currentUser.UserId, cancellationToken).ConfigureAwait(false));
            return ServiceResult.Success();
        }
        catch (Exception ex) { return HandleException(ex); }
    }

    public async Task<ServiceResult> DeleteAsync(int testimonialId, CancellationToken cancellationToken = default)
    {
        try
        {
            SpResultHelper.EnsureSuccess(
                await _repository.DeleteAsync(testimonialId, _currentUser.UserId, cancellationToken).ConfigureAwait(false));
            return ServiceResult.Success();
        }
        catch (Exception ex) { return HandleException(ex); }
    }

    public async Task<ServiceResult<PagedResult<TestimonialDto>>> GetPagedAsync(GetTestimonialPagedRequest request, CancellationToken cancellationToken = default)
    {
        try
        {
            PortfolioProfileRules.EnsureValidProfileId(request.PortfolioProfileId);
            return ServiceResult<PagedResult<TestimonialDto>>.Success(
                await _repository.GetPagedAsync(request, cancellationToken).ConfigureAwait(false));
        }
        catch (Exception ex) { return HandleException<PagedResult<TestimonialDto>>(ex); }
    }
}
