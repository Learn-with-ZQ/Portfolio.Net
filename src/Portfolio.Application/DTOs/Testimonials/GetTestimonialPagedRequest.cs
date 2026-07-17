using Portfolio.Application.Common.Models;

namespace Portfolio.Application.DTOs.Testimonials;

public sealed class GetTestimonialPagedRequest : PagedRequestBase
{
    /// <summary>Optional status filter (1 Pending, 2 Approved, 3 Rejected).</summary>
    public int? Status { get; set; }
}
