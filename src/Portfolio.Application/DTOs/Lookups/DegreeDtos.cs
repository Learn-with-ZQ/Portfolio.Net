namespace Portfolio.Application.DTOs.Lookups;

public sealed class DegreeDto
{
    public int DegreeId { get; init; }
    public string DegreeName { get; init; } = string.Empty;
    public byte[] RowVersion { get; init; } = [];
}

public sealed class CreateDegreeRequest
{
    public string DegreeName { get; set; } = string.Empty;
}

public sealed class UpdateDegreeRequest
{
    public int DegreeId { get; set; }
    public string DegreeName { get; set; } = string.Empty;
    public byte[] RowVersion { get; set; } = [];
}
