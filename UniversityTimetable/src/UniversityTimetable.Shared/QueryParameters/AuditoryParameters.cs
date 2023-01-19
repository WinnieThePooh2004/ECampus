using UniversityTimetable.Shared.Models;

namespace UniversityTimetable.Shared.QueryParameters;

public class AuditoryParameters : QueryParameters, IQueryParameters<Auditory>
{
    public string? AuditoryName { get; set; } = string.Empty;
    public string? BuildingName { get; set; } = string.Empty;
}