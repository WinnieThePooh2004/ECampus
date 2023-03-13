using ECampus.Domain.Entities;
using ECampus.Domain.Requests;

namespace ECampus.DataAccess.Contracts.DataSelectParameters;

public readonly struct TasksByGroupParameters : IDataSelectParameters<CourseTask>
{
    public readonly int GroupId;

    public TasksByGroupParameters(int groupId)
    {
        GroupId = groupId;
    }
}