using UniversityTimetable.Shared.Models;

namespace UniversityTimetable.Shared.QueryParameters;

public class CourseParameters : QueryParameters, IQueryParameters<Course>
{
    public int GroupId { get; set; }
    public int TeacherId { get; set; }
    public string? Name { get; set; }
}