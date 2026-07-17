using Portfolio.Application.DTOs.Lookups;
using Portfolio.Domain.Entities.Lookups;

namespace Portfolio.Application.Mappings;

public static class DegreeMapper
{
    public static DegreeDto ToDto(Degree entity) => new()
    {
        DegreeId = entity.DegreeId,
        DegreeName = entity.DegreeName,
        RowVersion = entity.RowVersion
    };

    public static Degree ToEntity(CreateDegreeRequest request) => new()
    {
        DegreeName = request.DegreeName
    };

    public static void ApplyUpdate(Degree entity, UpdateDegreeRequest request)
    {
        entity.DegreeName = request.DegreeName;
        entity.RowVersion = request.RowVersion;
    }
}
