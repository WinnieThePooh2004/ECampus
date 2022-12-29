using UniversityTimetable.Shared.Models;

namespace UniversityTimetable.Shared.DataContainers;

/// <summary>
/// use it only in IClassRepositoryMethods
/// </summary>
public class TimetableData
{
    public IEnumerable<Class?> Classes { get; init; } = default!;
    public Auditory? Auditory { get; init; }
    public Group? Group { get; init; }
    public Teacher? Teacher { get; init; }
}