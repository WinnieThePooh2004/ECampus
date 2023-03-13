using ECampus.Domain.Entities;
using ECampus.Domain.Requests;

namespace ECampus.DataAccess.Contracts.DataSelectParameters;

public readonly struct TaskSubmissionByStudentAndCourseParameters : IDataSelectParameters<TaskSubmission>
{
    public required int StudentId { get; init; }
    public required int CourseTaskId { get; init; }
}