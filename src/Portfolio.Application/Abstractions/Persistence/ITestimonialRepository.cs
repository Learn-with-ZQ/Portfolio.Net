using Portfolio.Application.Common.Models;
using Portfolio.Application.DTOs.Testimonials;
using Portfolio.Domain.Entities.Testimonials;

namespace Portfolio.Application.Abstractions.Persistence;

public interface ITestimonialRepository
{
    Task<Testimonial?> GetByIdAsync(int testimonialId, CancellationToken cancellationToken = default);
    Task<SpExecutionResult> SubmitAsync(Testimonial testimonial, CancellationToken cancellationToken = default);
    Task<SpExecutionResult> SetStatusAsync(int testimonialId, int status, int? updatedBy, CancellationToken cancellationToken = default);
    Task<SpExecutionResult> DeleteAsync(int testimonialId, int? deletedBy, CancellationToken cancellationToken = default);
    Task<PagedResult<TestimonialDto>> GetPagedAsync(GetTestimonialPagedRequest request, CancellationToken cancellationToken = default);
}
