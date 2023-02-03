using ECampus.Shared.Models;

namespace ECampus.Shared.QueryParameters;

public class TaskSubmissionByStudentAndCourseParameters : IQueryParameters<TaskSubmission>
{
    public int StudentId { get; set; }
    public int CourseTaskId { get; set; }
}