using FluentValidation;
using Portfolio.Application.Abstractions.Persistence;
using Portfolio.Application.Common.BusinessRules;
using Portfolio.Application.Common.Helpers;
using Portfolio.Application.Common.Interfaces;
using Portfolio.Application.Common.Models;
using Portfolio.Application.Common.Results;
using Portfolio.Application.DTOs.Blog;
using Portfolio.Application.Mappings;
using Portfolio.Application.Services.Interfaces;
using Portfolio.Domain.Exceptions;

namespace Portfolio.Application.Services;

public sealed class BlogService : ApplicationServiceBase, IBlogService
{
    private readonly IBlogRepository _repository;
    private readonly ICurrentUserService _currentUser;
    private readonly IValidator<CreateBlogPostRequest> _createValidator;
    private readonly IValidator<UpdateBlogPostRequest> _updateValidator;

    public BlogService(
        IBlogRepository repository,
        ICurrentUserService currentUser,
        IValidator<CreateBlogPostRequest> createValidator,
        IValidator<UpdateBlogPostRequest> updateValidator)
    {
        _repository = repository;
        _currentUser = currentUser;
        _createValidator = createValidator;
        _updateValidator = updateValidator;
    }

    public async Task<ServiceResult<BlogPostDetailDto>> GetByIdAsync(int blogPostId, CancellationToken cancellationToken = default)
    {
        try
        {
            var entity = await _repository.GetByIdAsync(blogPostId, cancellationToken).ConfigureAwait(false);
            if (entity is null) throw new NotFoundException("BlogPost", blogPostId);
            return ServiceResult<BlogPostDetailDto>.Success(BlogMapper.ToDetailDto(entity));
        }
        catch (Exception ex) { return HandleException<BlogPostDetailDto>(ex); }
    }

    public async Task<ServiceResult<BlogPostDetailDto>> GetBySlugAsync(int portfolioProfileId, string slug, CancellationToken cancellationToken = default)
    {
        try
        {
            PortfolioProfileRules.EnsureValidProfileId(portfolioProfileId);
            var entity = await _repository.GetBySlugAsync(portfolioProfileId, slug, cancellationToken).ConfigureAwait(false);
            if (entity is null) throw new NotFoundException("BlogPost", slug);
            return ServiceResult<BlogPostDetailDto>.Success(BlogMapper.ToDetailDto(entity));
        }
        catch (Exception ex) { return HandleException<BlogPostDetailDto>(ex); }
    }

    public async Task<ServiceResult<CommandResultDto>> CreateAsync(CreateBlogPostRequest request, CancellationToken cancellationToken = default)
    {
        try
        {
            await ValidateAsync(_createValidator, request, cancellationToken).ConfigureAwait(false);
            PortfolioProfileRules.EnsureValidProfileId(request.PortfolioProfileId);
            var result = await _repository.InsertAsync(BlogMapper.ToEntity(request), _currentUser.UserId, cancellationToken).ConfigureAwait(false);
            SpResultHelper.EnsureSuccess(result);
            return ServiceResult<CommandResultDto>.Success(SpResultHelper.ToCommandResult(result));
        }
        catch (Exception ex) { return HandleException<CommandResultDto>(ex); }
    }

    public async Task<ServiceResult<CommandResultDto>> UpdateAsync(UpdateBlogPostRequest request, CancellationToken cancellationToken = default)
    {
        try
        {
            await ValidateAsync(_updateValidator, request, cancellationToken).ConfigureAwait(false);
            var entity = await _repository.GetByIdAsync(request.BlogPostId, cancellationToken).ConfigureAwait(false);
            if (entity is null) throw new NotFoundException("BlogPost", request.BlogPostId);

            BlogMapper.ApplyUpdate(entity, request);
            var result = await _repository.UpdateAsync(entity, _currentUser.UserId, cancellationToken).ConfigureAwait(false);
            SpResultHelper.EnsureSuccess(result);
            return ServiceResult<CommandResultDto>.Success(SpResultHelper.ToCommandResult(result));
        }
        catch (Exception ex) { return HandleException<CommandResultDto>(ex); }
    }

    public Task<ServiceResult> PublishAsync(int blogPostId, CancellationToken cancellationToken = default)
        => SetPublishedAsync(blogPostId, true, cancellationToken);

    public Task<ServiceResult> UnpublishAsync(int blogPostId, CancellationToken cancellationToken = default)
        => SetPublishedAsync(blogPostId, false, cancellationToken);

    private async Task<ServiceResult> SetPublishedAsync(int blogPostId, bool isPublished, CancellationToken cancellationToken)
    {
        try
        {
            SpResultHelper.EnsureSuccess(
                await _repository.SetPublishedAsync(blogPostId, isPublished, _currentUser.UserId, cancellationToken).ConfigureAwait(false));
            return ServiceResult.Success();
        }
        catch (Exception ex) { return HandleException(ex); }
    }

    public async Task<ServiceResult> DeleteAsync(int blogPostId, CancellationToken cancellationToken = default)
    {
        try
        {
            SpResultHelper.EnsureSuccess(await _repository.DeleteAsync(blogPostId, _currentUser.UserId, cancellationToken).ConfigureAwait(false));
            return ServiceResult.Success();
        }
        catch (Exception ex) { return HandleException(ex); }
    }

    public async Task<ServiceResult<PagedResult<BlogPostDto>>> GetPagedAsync(GetBlogPagedRequest request, CancellationToken cancellationToken = default)
    {
        try
        {
            PortfolioProfileRules.EnsureValidProfileId(request.PortfolioProfileId);
            return ServiceResult<PagedResult<BlogPostDto>>.Success(
                await _repository.GetPagedAsync(request, cancellationToken).ConfigureAwait(false));
        }
        catch (Exception ex) { return HandleException<PagedResult<BlogPostDto>>(ex); }
    }
}
