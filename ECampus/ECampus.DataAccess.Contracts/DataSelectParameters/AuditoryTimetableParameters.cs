using ECampus.Domain.Models;
using ECampus.Domain.QueryParameters;

namespace ECampus.Contracts.DataSelectParameters;

public readonly struct AuditoryTimetableParameters : IDataSelectParameters<Class>
{
    public readonly int AuditoryId;

    public AuditoryTimetableParameters(int auditoryId)
    {
        AuditoryId = auditoryId;
    }
}