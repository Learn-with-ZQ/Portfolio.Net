using Portfolio.Application.DTOs.Lookups;
using Portfolio.Domain.Entities.Lookups;

namespace Portfolio.Application.Mappings;

public static class TechnologyMapper
{
    public static TechnologyDto ToDto(Technology entity) => new()
    {
        TechnologyId = entity.TechnologyId,
        TechnologyName = entity.TechnologyName,
        Category = entity.Category,
        RowVersion = entity.RowVersion
    };

    public static Technology ToEntity(CreateTechnologyRequest request) => new()
    {
        TechnologyName = request.TechnologyName,
        Category = request.Category
    };

    public static void ApplyUpdate(Technology entity, UpdateTechnologyRequest request)
    {
        entity.TechnologyName = request.TechnologyName;
        entity.Category = request.Category;
        entity.RowVersion = request.RowVersion;
    }
}
