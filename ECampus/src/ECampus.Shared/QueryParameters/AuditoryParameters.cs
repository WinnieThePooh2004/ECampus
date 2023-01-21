using ECampus.Shared.Models;

namespace ECampus.Shared.QueryParameters;

public class AuditoryParameters : QueryParameters, IQueryParameters<Auditory>
{
    public string? AuditoryName { get; set; } = string.Empty;
    public string? BuildingName { get; set; } = string.Empty;
}