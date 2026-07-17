using Portfolio.Application.Common.Models;
using Portfolio.Application.DTOs.Paragraphs;
using Portfolio.Domain.Entities.Paragraphs;

namespace Portfolio.Application.Abstractions.Persistence;

public interface IParagraphRepository
{
    Task<Paragraph?> GetByIdAsync(int paragraphId, CancellationToken cancellationToken = default);
    Task<SpExecutionResult> InsertAsync(Paragraph paragraph, int? createdBy, CancellationToken cancellationToken = default);
    Task<SpExecutionResult> UpdateAsync(Paragraph paragraph, int? updatedBy, CancellationToken cancellationToken = default);
    Task<SpExecutionResult> DeleteAsync(int paragraphId, int? deletedBy, CancellationToken cancellationToken = default);
    Task<PagedResult<ParagraphDto>> GetPagedAsync(GetParagraphPagedRequest request, CancellationToken cancellationToken = default);
}
