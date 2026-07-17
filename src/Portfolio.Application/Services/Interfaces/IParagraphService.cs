using Portfolio.Application.Common.Models;
using Portfolio.Application.Common.Results;
using Portfolio.Application.DTOs.Paragraphs;

namespace Portfolio.Application.Services.Interfaces;

public interface IParagraphService
{
    Task<ServiceResult<ParagraphDetailDto>> GetByIdAsync(int paragraphId, CancellationToken cancellationToken = default);
    Task<ServiceResult<CommandResultDto>> CreateAsync(CreateParagraphRequest request, CancellationToken cancellationToken = default);
    Task<ServiceResult<CommandResultDto>> UpdateAsync(UpdateParagraphRequest request, CancellationToken cancellationToken = default);
    Task<ServiceResult> DeleteAsync(int paragraphId, CancellationToken cancellationToken = default);
    Task<ServiceResult<PagedResult<ParagraphDto>>> GetPagedAsync(GetParagraphPagedRequest request, CancellationToken cancellationToken = default);
}
