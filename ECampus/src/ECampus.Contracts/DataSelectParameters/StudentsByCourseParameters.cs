using ECampus.Shared.Models;
using ECampus.Shared.QueryParameters;

namespace ECampus.Contracts.DataSelectParameters;

public class StudentsByCourseParameters : IDataSelectParameters<Student>
{
    public int CourseId { get; set; }
}