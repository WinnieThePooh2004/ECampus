using ECampus.Shared.DataTransferObjects;
using ECampus.Shared.Models;

namespace ECampus.Shared.QueryParameters;

public class AuditoryParameters : QueryParameters<AuditoryDto>, IDataSelectParameters<Auditory>
{
    public string? AuditoryName { get; set; } = string.Empty;
    public string? BuildingName { get; set; } = string.Empty;
}