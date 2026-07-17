using Portfolio.Application.DTOs.Lookups;
using Portfolio.Domain.Entities.Lookups;

namespace Portfolio.Application.Mappings;

public static class DegreeLevelMapper
{
    public static DegreeLevelDto ToDto(DegreeLevel entity) => new()
    {
        DegreeLevelId = entity.DegreeLevelId,
        DegreeLevelName = entity.DegreeLevelName,
        DegreePrefix = entity.DegreePrefix,
        SortOrder = entity.SortOrder,
        RowVersion = entity.RowVersion
    };

    public static DegreeLevel ToEntity(CreateDegreeLevelRequest request) => new()
    {
        DegreeLevelName = request.DegreeLevelName,
        DegreePrefix = request.DegreePrefix,
        SortOrder = request.SortOrder
    };

    public static void ApplyUpdate(DegreeLevel entity, UpdateDegreeLevelRequest request)
    {
        entity.DegreeLevelName = request.DegreeLevelName;
        entity.DegreePrefix = request.DegreePrefix;
        entity.SortOrder = request.SortOrder;
        entity.RowVersion = request.RowVersion;
    }
}
