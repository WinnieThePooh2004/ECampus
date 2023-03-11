using ECampus.Domain.Entities;
using ECampus.Domain.QueryParameters;

namespace ECampus.DataAccess.Contracts.DataSelectParameters;

public readonly struct StudentsByCourseParameters : IDataSelectParameters<Student>
{
    public readonly int CourseId;

    public StudentsByCourseParameters(int courseId)
    {
        CourseId = courseId;
    }
}