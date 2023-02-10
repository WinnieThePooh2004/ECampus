using ECampus.Shared.Models;
using ECampus.Shared.QueryParameters;

namespace ECampus.Contracts.DataSelectParameters;

public readonly struct TaskSubmissionByStudentAndCourseParameters : IDataSelectParameters<TaskSubmission>
{
    public required int StudentId { get; init; }
    public required int CourseTaskId { get; init; }
}