using Portfolio.Application.DTOs.Lookups;
using Portfolio.Domain.Entities.Lookups;

namespace Portfolio.Application.Mappings;

public static class InstituteMapper
{
    public static InstituteDto ToDto(Institute entity) => new()
    {
        InstituteId = entity.InstituteId,
        InstituteName = entity.InstituteName,
        Location = entity.Location,
        RowVersion = entity.RowVersion
    };

    public static Institute ToEntity(CreateInstituteRequest request) => new()
    {
        InstituteName = request.InstituteName,
        Location = request.Location
    };

    public static void ApplyUpdate(Institute entity, UpdateInstituteRequest request)
    {
        entity.InstituteName = request.InstituteName;
        entity.Location = request.Location;
        entity.RowVersion = request.RowVersion;
    }
}
