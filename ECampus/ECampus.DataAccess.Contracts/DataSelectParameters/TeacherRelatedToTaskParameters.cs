using ECampus.Shared.Models;
using ECampus.Shared.QueryParameters;

namespace ECampus.Contracts.DataSelectParameters;

public readonly struct TeacherRelatedToTaskParameters : IDataSelectParameters<Teacher>
{
    public readonly int CourseTaskId;

    public TeacherRelatedToTaskParameters(int courseTaskId)
    {
        CourseTaskId = courseTaskId;
    }
}