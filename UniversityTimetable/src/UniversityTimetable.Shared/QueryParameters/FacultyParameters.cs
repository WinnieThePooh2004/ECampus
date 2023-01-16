using UniversityTimetable.Shared.Models;

namespace UniversityTimetable.Shared.QueryParameters;

public class FacultyParameters : QueryParameters, IQueryParameters<Faculty>
{
    public string? Name { get; set; }
}