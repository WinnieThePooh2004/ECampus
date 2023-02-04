using ECampus.Shared.Models;
using ECampus.Shared.QueryParameters;

namespace ECampus.Contracts.DataSelectParameters;

public class AuditoryTimetableParameters : IDataSelectParameters<Class>
{
    public int AuditoryId { get; init; }
}