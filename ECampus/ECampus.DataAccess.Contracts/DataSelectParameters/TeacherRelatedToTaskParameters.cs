using ECampus.Domain.Entities;
using ECampus.Domain.Requests;

namespace ECampus.DataAccess.Contracts.DataSelectParameters;

public readonly struct TeacherRelatedToTaskParameters : IDataSelectParameters<Teacher>
{
    public readonly int CourseTaskId;

    public TeacherRelatedToTaskParameters(int courseTaskId)
    {
        CourseTaskId = courseTaskId;
    }
}