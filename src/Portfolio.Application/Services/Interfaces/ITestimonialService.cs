using Portfolio.Application.Common.Models;
using Portfolio.Application.Common.Results;
using Portfolio.Application.DTOs.Testimonials;

namespace Portfolio.Application.Services.Interfaces;

public interface ITestimonialService
{
    Task<ServiceResult<TestimonialDetailDto>> GetByIdAsync(int testimonialId, CancellationToken cancellationToken = default);
    Task<ServiceResult<CommandResultDto>> SubmitAsync(SubmitTestimonialRequest request, CancellationToken cancellationToken = default);
    Task<ServiceResult> ApproveAsync(int testimonialId, CancellationToken cancellationToken = default);
    Task<ServiceResult> RejectAsync(int testimonialId, CancellationToken cancellationToken = default);
    Task<ServiceResult> DeleteAsync(int testimonialId, CancellationToken cancellationToken = default);
    Task<ServiceResult<PagedResult<TestimonialDto>>> GetPagedAsync(GetTestimonialPagedRequest request, CancellationToken cancellationToken = default);
}
