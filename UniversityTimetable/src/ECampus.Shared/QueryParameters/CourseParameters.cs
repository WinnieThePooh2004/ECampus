using ECampus.Shared.Models;

namespace ECampus.Shared.QueryParameters;

public class CourseParameters : QueryParameters, IQueryParameters<Course>
{
    public int GroupId { get; set; }
    public int TeacherId { get; set; }
    public string? Name { get; set; }
}