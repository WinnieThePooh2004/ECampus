using ECampus.Shared.Models;

namespace ECampus.Shared.QueryParameters;

public class CourseParameters : QueryParameters, IDataSelectParameters<Course>
{
    public int GroupId { get; set; }
    public int TeacherId { get; set; }
    public string? Name { get; set; }
}