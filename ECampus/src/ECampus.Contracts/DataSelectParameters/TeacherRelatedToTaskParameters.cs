using ECampus.Shared.Models;
using ECampus.Shared.QueryParameters;

namespace ECampus.Contracts.DataSelectParameters;

public class TeacherRelatedToTaskParameters : IDataSelectParameters<Teacher>
{
    public int CourseTaskId { get; set; }
}