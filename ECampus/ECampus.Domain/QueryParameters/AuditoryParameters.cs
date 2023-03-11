using ECampus.Domain.DataTransferObjects;
using ECampus.Domain.Models;

namespace ECampus.Domain.QueryParameters;

public class AuditoryParameters : QueryParameters<AuditoryDto>, IDataSelectParameters<Auditory>
{
    public string? AuditoryName { get; set; } = string.Empty;
    public string? BuildingName { get; set; } = string.Empty;
}