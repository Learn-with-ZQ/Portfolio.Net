using Portfolio.Application.Abstractions.Persistence;
using Portfolio.Application.Common.Models;
using Portfolio.Application.DTOs.Lookups;
using Portfolio.Domain.Entities.Lookups;
using Portfolio.Persistence.Common;

namespace Portfolio.Persistence.Repositories;

public sealed class CourseRepository : ICourseRepository
{
    private readonly Database _db;

    public CourseRepository(Database db) => _db = db;

    public async Task<IReadOnlyList<LookupItemDto>> GetLookupAsync(CancellationToken cancellationToken = default)
    {
        var (_, rows) = await _db.QueryAsync<LookupItemDto>(
            "dbo.usp_Course_GetLookup", new Dictionary<string, object?>(), cancellationToken).ConfigureAwait(false);
        return rows;
    }

    public async Task<PagedResult<CourseDto>> GetPagedAsync(GetLookupPagedRequest request, CancellationToken cancellationToken = default)
    {
        var (_, rows, total, pages) = await _db.QueryPagedAsync<CourseDto>("dbo.usp_Course_GetPaged", new Dictionary<string, object?>
        {
            ["PageNumber"] = request.PageNumber,
            ["PageSize"] = request.PageSize,
            ["SearchTerm"] = request.SearchTerm
        }, cancellationToken).ConfigureAwait(false);
        return new PagedResult<CourseDto> { Items = rows, PageNumber = request.PageNumber, PageSize = request.PageSize, TotalRecords = total, TotalPages = pages };
    }

    public async Task<Course?> GetByIdAsync(int courseId, CancellationToken cancellationToken = default)
    {
        var (_, item) = await _db.QuerySingleAsync<Course>("dbo.usp_Course_GetById", new Dictionary<string, object?>
        {
            ["CourseID_Pk"] = courseId
        }, cancellationToken).ConfigureAwait(false);
        return item;
    }

    public Task<SpExecutionResult> InsertAsync(Course course, int? createdBy, CancellationToken cancellationToken = default) =>
        _db.ExecuteAsync("dbo.usp_Course_Insert", new Dictionary<string, object?>
        {
            ["CourseName"] = course.CourseName,
            ["InstituteID_Fk"] = course.InstituteId,
            ["SortOrder"] = course.SortOrder,
            ["CreatedBy"] = createdBy
        }, outIdKey: "OutCourseID", cancellationToken);

    public Task<SpExecutionResult> UpdateAsync(Course course, int? updatedBy, CancellationToken cancellationToken = default) =>
        _db.ExecuteAsync("dbo.usp_Course_Update", new Dictionary<string, object?>
        {
            ["CourseID_Pk"] = course.CourseId,
            ["CourseName"] = course.CourseName,
            ["InstituteID_Fk"] = course.InstituteId,
            ["SortOrder"] = course.SortOrder,
            ["RowVersion"] = course.RowVersion,
            ["UpdatedBy"] = updatedBy
        }, cancellationToken: cancellationToken);

    public Task<SpExecutionResult> DeleteAsync(int courseId, int? deletedBy, CancellationToken cancellationToken = default) =>
        _db.ExecuteAsync("dbo.usp_Course_Delete", new Dictionary<string, object?>
        {
            ["CourseID_Pk"] = courseId,
            ["DeletedBy"] = deletedBy
        }, cancellationToken: cancellationToken);
}
