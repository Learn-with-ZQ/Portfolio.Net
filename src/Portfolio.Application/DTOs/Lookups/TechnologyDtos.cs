namespace Portfolio.Application.DTOs.Lookups;

public sealed class TechnologyDto
{
    public int TechnologyId { get; init; }
    public string TechnologyName { get; init; } = string.Empty;
    public string? Category { get; init; }
    public byte[] RowVersion { get; init; } = [];
}

public sealed class CreateTechnologyRequest
{
    public string TechnologyName { get; set; } = string.Empty;
    public string? Category { get; set; }
}

public sealed class UpdateTechnologyRequest
{
    public int TechnologyId { get; set; }
    public string TechnologyName { get; set; } = string.Empty;
    public string? Category { get; set; }
    public byte[] RowVersion { get; set; } = [];
}
