using ECampus.Domain.Responses.Course;

namespace ECampus.Domain.Requests.Course;

public class CourseParameters : QueryParameters<MultipleCourseResponse>, IDataSelectParameters<Entities.Course>
{
    public int GroupId { get; set; }
    public int TeacherId { get; set; }
    public string? Name { get; set; }
}