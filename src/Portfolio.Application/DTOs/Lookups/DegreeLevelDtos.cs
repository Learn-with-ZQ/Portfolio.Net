namespace Portfolio.Application.DTOs.Lookups;

public sealed class DegreeLevelDto
{
    public int DegreeLevelId { get; init; }
    public string DegreeLevelName { get; init; } = string.Empty;
    public string DegreePrefix { get; init; } = string.Empty;
    public int SortOrder { get; init; }
    public byte[] RowVersion { get; init; } = [];
}

public sealed class CreateDegreeLevelRequest
{
    public string DegreeLevelName { get; set; } = string.Empty;
    public string DegreePrefix { get; set; } = string.Empty;
    public int SortOrder { get; set; }
}

public sealed class UpdateDegreeLevelRequest
{
    public int DegreeLevelId { get; set; }
    public string DegreeLevelName { get; set; } = string.Empty;
    public string DegreePrefix { get; set; } = string.Empty;
    public int SortOrder { get; set; }
    public byte[] RowVersion { get; set; } = [];
}
