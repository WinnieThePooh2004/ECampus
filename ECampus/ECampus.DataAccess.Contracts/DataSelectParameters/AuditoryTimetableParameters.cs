using ECampus.Shared.Models;
using ECampus.Shared.QueryParameters;

namespace ECampus.Contracts.DataSelectParameters;

public readonly struct AuditoryTimetableParameters : IDataSelectParameters<Class>
{
    public readonly int AuditoryId;

    public AuditoryTimetableParameters(int auditoryId)
    {
        AuditoryId = auditoryId;
    }
}