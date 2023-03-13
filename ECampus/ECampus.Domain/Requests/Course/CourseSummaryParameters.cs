using ECampus.Domain.Responses.Course;

namespace ECampus.Domain.Requests.Course;

public class CourseSummaryParameters : QueryParameters<CourseSummaryResponse>, IDataSelectParameters<Entities.Course>
{
    public int StudentId { get; set; }
}