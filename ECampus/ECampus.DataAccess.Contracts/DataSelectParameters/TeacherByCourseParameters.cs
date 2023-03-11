using ECampus.Domain.Models;
using ECampus.Domain.QueryParameters;

namespace ECampus.DataAccess.Contracts.DataSelectParameters;

public readonly struct TeacherByCourseParameters : IDataSelectParameters<Teacher>
{
    public readonly int CourseId;

    public TeacherByCourseParameters(int courseId)
    {
        CourseId = courseId;
    }
}