using ECampus.Domain.Models;
using ECampus.Domain.QueryParameters;

namespace ECampus.Contracts.DataSelectParameters;

public readonly struct TasksByGroupParameters : IDataSelectParameters<CourseTask>
{
    public readonly int GroupId;

    public TasksByGroupParameters(int groupId)
    {
        GroupId = groupId;
    }
}