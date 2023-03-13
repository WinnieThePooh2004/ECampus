using ECampus.Domain.Responses.Auditory;

namespace ECampus.Domain.Requests.Auditory;

public class AuditoryParameters : QueryParameters<MultipleAuditoryResponse>, IDataSelectParameters<Entities.Auditory>
{
    public string? AuditoryName { get; set; } = string.Empty;
    public string? BuildingName { get; set; } = string.Empty;
}