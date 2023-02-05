using ECampus.Shared.Models;
using ECampus.Shared.QueryParameters;

namespace ECampus.Contracts.DataSelectParameters;

public class TeacherByCourseParameters : IDataSelectParameters<Teacher>
{
    public int CourseId { get; set; }
}