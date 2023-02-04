using ECampus.Shared.Models;
using ECampus.Shared.QueryParameters;

namespace ECampus.Contracts.DataSelectParameters;

public class TaskSubmissionByStudentAndCourseParameters : IDataSelectParameters<TaskSubmission>
{
    public int StudentId { get; set; }
    public int CourseTaskId { get; set; }
}