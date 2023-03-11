using ECampus.Shared.Models;
using ECampus.Shared.QueryParameters;

namespace ECampus.Contracts.DataSelectParameters;

public readonly struct TasksByGroupParameters : IDataSelectParameters<CourseTask>
{
    public readonly int GroupId;

    public TasksByGroupParameters(int groupId)
    {
        GroupId = groupId;
    }
}