using ECampus.Domain.Models;
using ECampus.Domain.QueryParameters;

namespace ECampus.Contracts.DataSelectParameters;

public readonly struct TaskSubmissionByStudentAndCourseParameters : IDataSelectParameters<TaskSubmission>
{
    public required int StudentId { get; init; }
    public required int CourseTaskId { get; init; }
}