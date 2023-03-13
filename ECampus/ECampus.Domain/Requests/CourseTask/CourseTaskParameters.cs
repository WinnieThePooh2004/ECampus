using ECampus.Domain.Responses.CourseTask;

namespace ECampus.Domain.Requests.CourseTask;

public class CourseTaskParameters : QueryParameters<MultipleCourseTaskResponse>, IDataSelectParameters<Entities.CourseTask>
{
    public int CourseId { get; set; }
}