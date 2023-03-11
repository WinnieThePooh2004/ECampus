using ECampus.Domain.Models;
using ECampus.Domain.QueryParameters;

namespace ECampus.DataAccess.Contracts.DataSelectParameters;

public readonly struct TasksByGroupParameters : IDataSelectParameters<CourseTask>
{
    public readonly int GroupId;

    public TasksByGroupParameters(int groupId)
    {
        GroupId = groupId;
    }
}