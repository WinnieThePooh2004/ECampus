using UniversityTimetable.Shared.Interfaces.Data;

namespace UniversityTimetable.Shared.DataTransferObjects
{
    public class FacultyDTO : IDataTransferObject
    {
        public int Id { get; set; }
        public string Name { get; set; } = default!;
    }
}
