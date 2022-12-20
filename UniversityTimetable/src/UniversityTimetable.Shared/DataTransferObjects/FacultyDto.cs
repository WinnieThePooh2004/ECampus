using UniversityTimetable.Shared.Interfaces.Data;

namespace UniversityTimetable.Shared.DataTransferObjects
{
    public class FacultyDto : IDataTransferObject
    {
        public int Id { get; set; }
        public string Name { get; set; } = default!;
    }
}
