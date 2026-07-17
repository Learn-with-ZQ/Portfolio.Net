using Portfolio.Application.Abstractions.Persistence;
using Portfolio.Application.Common.Models;
using Portfolio.Application.DTOs.Paragraphs;
using Portfolio.Domain.Entities.Paragraphs;
using Portfolio.Persistence.Common;

namespace Portfolio.Persistence.Repositories;

public sealed class ParagraphRepository : IParagraphRepository
{
    private readonly Database _db;
    public ParagraphRepository(Database db) => _db = db;

    public async Task<Paragraph?> GetByIdAsync(int paragraphId, CancellationToken cancellationToken = default)
    {
        var (_, item) = await _db.QuerySingleAsync<Paragraph>("dbo.usp_Paragraphs_GetById", new Dictionary<string, object?>
        { ["ParagraphID_Pk"] = paragraphId, ["IncludeDeleted"] = false }, cancellationToken).ConfigureAwait(false);
        return item;
    }

    public Task<SpExecutionResult> InsertAsync(Paragraph paragraph, int? createdBy, CancellationToken cancellationToken = default) =>
        _db.ExecuteAsync("dbo.usp_Paragraphs_Insert", new Dictionary<string, object?>
        {
            ["PortfolioProfileID_Fk"] = paragraph.PortfolioProfileId,
            ["ParagraphTypeID_Fk"] = paragraph.ParagraphTypeId,
            ["ParagraphTitle"] = paragraph.ParagraphTitle,
            ["ParagraphText"] = paragraph.ParagraphText,
            ["SortOrder"] = paragraph.SortOrder,
            ["IsActive"] = paragraph.IsActive,
            ["CreatedBy"] = createdBy
        },
        outIdKey: "OutParagraphID", cancellationToken);

    public Task<SpExecutionResult> UpdateAsync(Paragraph paragraph, int? updatedBy, CancellationToken cancellationToken = default) =>
        _db.ExecuteAsync("dbo.usp_Paragraphs_Update", new Dictionary<string, object?>
        {
            ["ParagraphID_Pk"] = paragraph.ParagraphId,
            ["ParagraphTypeID_Fk"] = paragraph.ParagraphTypeId,
            ["ParagraphTitle"] = paragraph.ParagraphTitle,
            ["ParagraphText"] = paragraph.ParagraphText,
            ["SortOrder"] = paragraph.SortOrder,
            ["IsActive"] = paragraph.IsActive,
            ["RowVersion"] = paragraph.RowVersion,
            ["UpdatedBy"] = updatedBy
        },
        cancellationToken: cancellationToken);

    public Task<SpExecutionResult> DeleteAsync(int paragraphId, int? deletedBy, CancellationToken cancellationToken = default) =>
        _db.ExecuteAsync("dbo.usp_Paragraphs_Delete", new Dictionary<string, object?>
        {
            ["ParagraphID_Pk"] = paragraphId,
            ["DeletedBy"] = deletedBy
        },
        cancellationToken: cancellationToken);

    public async Task<PagedResult<ParagraphDto>> GetPagedAsync(GetParagraphPagedRequest request, CancellationToken cancellationToken = default)
    {
        var (_, rows, total, pages) = await _db.QueryPagedAsync<ParagraphDto>("dbo.usp_Paragraphs_GetPaged", new Dictionary<string, object?>
        {
            ["PortfolioProfileID_Fk"] = request.PortfolioProfileId,
            ["PageNumber"] = request.PageNumber,
            ["PageSize"] = request.PageSize,
            ["SearchTerm"] = request.SearchTerm,
            ["ParagraphTypeID_Fk"] = request.ParagraphTypeId,
            ["IsActive"] = request.IsActive
        }, cancellationToken).ConfigureAwait(false);

        return new PagedResult<ParagraphDto>
        {
            Items = rows,
            PageNumber = request.PageNumber,
            PageSize = request.PageSize,
            TotalRecords = total,
            TotalPages = pages
        };
    }
}
