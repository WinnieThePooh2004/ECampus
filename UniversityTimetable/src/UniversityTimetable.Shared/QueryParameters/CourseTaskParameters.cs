using UniversityTimetable.Shared.Models;

namespace UniversityTimetable.Shared.QueryParameters;

public class CourseTaskParameters : QueryParameters, IQueryParameters<CourseTask>
{
    public int CourseId { get; set; }
}