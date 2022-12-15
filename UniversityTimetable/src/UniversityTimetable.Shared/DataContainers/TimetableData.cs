using UniversityTimetable.Shared.Models;

#nullable enable
namespace UniversityTimetable.Shared.DataContainers
{
    /// <summary>
    /// use it only in IClassRepositoryMethods
    /// </summary>
    public class TimetableData
    {
        public IEnumerable<Class> Classes { get; set; } = default!;
        public Auditory? Auditory { get; set; }
        public Group? Group { get; set; }
        public Teacher? Teacher { get; set; }
    }
}

#nullable disable
