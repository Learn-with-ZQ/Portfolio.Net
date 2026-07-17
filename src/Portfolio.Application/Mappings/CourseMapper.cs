using Portfolio.Application.DTOs.Lookups;
using Portfolio.Domain.Entities.Lookups;

namespace Portfolio.Application.Mappings;

public static class CourseMapper
{
    public static CourseDto ToDto(Course entity) => new()
    {
        CourseId = entity.CourseId,
        CourseName = entity.CourseName,
        InstituteId = entity.InstituteId,
        SortOrder = entity.SortOrder,
        RowVersion = entity.RowVersion
    };

    public static Course ToEntity(CreateCourseRequest request) => new()
    {
        CourseName = request.CourseName,
        InstituteId = request.InstituteId,
        SortOrder = request.SortOrder
    };

    public static void ApplyUpdate(Course entity, UpdateCourseRequest request)
    {
        entity.CourseName = request.CourseName;
        entity.InstituteId = request.InstituteId;
        entity.SortOrder = request.SortOrder;
        entity.RowVersion = request.RowVersion;
    }
}
