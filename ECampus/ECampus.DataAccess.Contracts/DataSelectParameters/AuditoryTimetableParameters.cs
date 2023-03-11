using ECampus.Domain.Entities;
using ECampus.Domain.QueryParameters;

namespace ECampus.DataAccess.Contracts.DataSelectParameters;

public readonly struct AuditoryTimetableParameters : IDataSelectParameters<Class>
{
    public readonly int AuditoryId;

    public AuditoryTimetableParameters(int auditoryId)
    {
        AuditoryId = auditoryId;
    }
}