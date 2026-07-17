namespace Portfolio.Application.DTOs.Lookups;

public sealed class InstituteDto
{
    public int InstituteId { get; init; }
    public string InstituteName { get; init; } = string.Empty;
    public string? Location { get; init; }
    public byte[] RowVersion { get; init; } = [];
}

public sealed class CreateInstituteRequest
{
    public string InstituteName { get; set; } = string.Empty;
    public string? Location { get; set; }
}

public sealed class UpdateInstituteRequest
{
    public int InstituteId { get; set; }
    public string InstituteName { get; set; } = string.Empty;
    public string? Location { get; set; }
    public byte[] RowVersion { get; set; } = [];
}
