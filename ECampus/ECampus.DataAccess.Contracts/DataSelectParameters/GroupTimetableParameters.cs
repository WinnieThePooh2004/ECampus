using ECampus.Shared.Models;
using ECampus.Shared.QueryParameters;

namespace ECampus.Contracts.DataSelectParameters;

public readonly struct GroupTimetableParameters : IDataSelectParameters<Class>
{
    public readonly int GroupId;

    public GroupTimetableParameters(int groupId)
    {
        GroupId = groupId;
    }
}