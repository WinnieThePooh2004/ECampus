using ECampus.Shared.Models;
using ECampus.Shared.QueryParameters;

namespace ECampus.Contracts.DataSelectParameters;

public readonly struct StudentsByCourseParameters : IDataSelectParameters<Student>
{
    public readonly int CourseId;

    public StudentsByCourseParameters(int courseId)
    {
        CourseId = courseId;
    }
}