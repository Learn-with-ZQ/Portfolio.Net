namespace Portfolio.Application.DTOs.Lookups;

/// <summary>Minimal id/name pair for populating dropdowns.</summary>
public sealed class LookupItemDto
{
    public int Id { get; init; }
    public string Name { get; init; } = string.Empty;
}

/// <summary>Shared paged/search request for lookup setup lists (no profile scope).</summary>
public sealed class GetLookupPagedRequest
{
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
    public string? SearchTerm { get; set; }
}

public sealed class CompanyDto
{
    public int CompanyId { get; init; }
    public string CompanyName { get; init; } = string.Empty;
    public string? WebsiteUrl { get; init; }
    public byte[] RowVersion { get; init; } = [];
}

public sealed class CreateCompanyRequest
{
    public string CompanyName { get; set; } = string.Empty;
    public string? WebsiteUrl { get; set; }
}

public sealed class UpdateCompanyRequest
{
    public int CompanyId { get; set; }
    public string CompanyName { get; set; } = string.Empty;
    public string? WebsiteUrl { get; set; }
    public byte[] RowVersion { get; set; } = [];
}

public sealed class DeployDetailDto
{
    public int DeployDetailId { get; init; }
    public string DeployDetailName { get; init; } = string.Empty;
    public string DeployedTo { get; init; } = string.Empty;
    public int? DeployedByCompanyId { get; init; }
    public string? CompanyName { get; init; }
    public byte[] RowVersion { get; init; } = [];
}

public sealed class CreateDeployDetailRequest
{
    public string DeployDetailName { get; set; } = string.Empty;
    public string DeployedTo { get; set; } = string.Empty;
    public int? DeployedByCompanyId { get; set; }
}

public sealed class UpdateDeployDetailRequest
{
    public int DeployDetailId { get; set; }
    public string DeployDetailName { get; set; } = string.Empty;
    public string DeployedTo { get; set; } = string.Empty;
    public int? DeployedByCompanyId { get; set; }
    public byte[] RowVersion { get; set; } = [];
}
