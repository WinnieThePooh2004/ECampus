using ECampus.Domain.Models;
using ECampus.Domain.QueryParameters;

namespace ECampus.DataAccess.Contracts.DataSelectParameters;

public readonly struct TeacherRelatedToTaskParameters : IDataSelectParameters<Teacher>
{
    public readonly int CourseTaskId;

    public TeacherRelatedToTaskParameters(int courseTaskId)
    {
        CourseTaskId = courseTaskId;
    }
}