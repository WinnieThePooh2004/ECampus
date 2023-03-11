using ECampus.Domain.Models;
using ECampus.Domain.QueryParameters;

namespace ECampus.DataAccess.Contracts.DataSelectParameters;

public readonly struct GroupTimetableParameters : IDataSelectParameters<Class>
{
    public readonly int GroupId;

    public GroupTimetableParameters(int groupId)
    {
        GroupId = groupId;
    }
}