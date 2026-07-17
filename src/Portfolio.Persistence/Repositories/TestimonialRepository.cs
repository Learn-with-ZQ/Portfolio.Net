using Portfolio.Application.Abstractions.Persistence;
using Portfolio.Application.Common.Models;
using Portfolio.Application.DTOs.Testimonials;
using Portfolio.Domain.Entities.Testimonials;
using Portfolio.Persistence.Common;

namespace Portfolio.Persistence.Repositories;

public sealed class TestimonialRepository : ITestimonialRepository
{
    private readonly Database _db;
    public TestimonialRepository(Database db) => _db = db;

    public async Task<Testimonial?> GetByIdAsync(int testimonialId, CancellationToken cancellationToken = default)
    {
        var (_, entity) = await _db.QuerySingleAsync<Testimonial>(
            "dbo.usp_Testimonials_GetById",
            new Dictionary<string, object?> { ["TestimonialID_Pk"] = testimonialId, ["IncludeDeleted"] = false },
            cancellationToken).ConfigureAwait(false);
        return entity;
    }

    public Task<SpExecutionResult> SubmitAsync(Testimonial t, CancellationToken cancellationToken = default) =>
        _db.ExecuteAsync("dbo.usp_Testimonials_Submit", new Dictionary<string, object?>
        {
            ["PortfolioProfileID_Fk"] = t.PortfolioProfileId,
            ["AuthorName"] = t.AuthorName,
            ["AuthorTitle"] = t.AuthorTitle,
            ["AuthorCompany"] = t.AuthorCompany,
            ["AuthorEmail"] = t.AuthorEmail,
            ["Relationship"] = t.Relationship,
            ["Rating"] = t.Rating,
            ["Content"] = t.Content,
            ["LinkedInUrl"] = t.LinkedInUrl
        }, outIdKey: "OutTestimonialID", cancellationToken);

    public Task<SpExecutionResult> SetStatusAsync(int testimonialId, int status, int? updatedBy, CancellationToken cancellationToken = default) =>
        _db.ExecuteAsync("dbo.usp_Testimonials_SetStatus", new Dictionary<string, object?>
        {
            ["TestimonialID_Pk"] = testimonialId,
            ["Status"] = status,
            ["UpdatedBy"] = updatedBy
        }, cancellationToken: cancellationToken);

    public Task<SpExecutionResult> DeleteAsync(int testimonialId, int? deletedBy, CancellationToken cancellationToken = default) =>
        _db.ExecuteAsync("dbo.usp_Testimonials_Delete", new Dictionary<string, object?>
        {
            ["TestimonialID_Pk"] = testimonialId,
            ["DeletedBy"] = deletedBy
        }, cancellationToken: cancellationToken);

    public async Task<PagedResult<TestimonialDto>> GetPagedAsync(GetTestimonialPagedRequest request, CancellationToken cancellationToken = default)
    {
        var (_, rows, total, pages) = await _db.QueryPagedAsync<TestimonialDto>(
            "dbo.usp_Testimonials_GetPaged", new Dictionary<string, object?>
            {
                ["PortfolioProfileID_Fk"] = request.PortfolioProfileId,
                ["PageNumber"] = request.PageNumber,
                ["PageSize"] = request.PageSize,
                ["SearchTerm"] = request.SearchTerm,
                ["Status"] = request.Status
            }, cancellationToken).ConfigureAwait(false);

        return new PagedResult<TestimonialDto>
        {
            Items = rows,
            PageNumber = request.PageNumber,
            PageSize = request.PageSize,
            TotalRecords = total,
            TotalPages = pages
        };
    }
}
