using UniversityTimetable.Shared.Models;

namespace UniversityTimetable.Shared.QueryParameters;

public class GroupParameters : QueryParameters, IQueryParameters<Group>
{
    public int DepartmentId { get; set; }
    public string? Name { get; set; }
}