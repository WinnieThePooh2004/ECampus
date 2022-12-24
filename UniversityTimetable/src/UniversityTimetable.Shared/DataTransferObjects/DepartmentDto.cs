using UniversityTimetable.Shared.Interfaces.Data;

namespace UniversityTimetable.Shared.DataTransferObjects
{
    public class DepartmentDto : IDataTransferObject
    {
        public int Id { get; set; }
        public string Name { get; set; } = default!;

        public int FacultyId { get; set; }
        public List<TeacherDto> Teachers { get; set; }
        public List<GroupDto> Groups { get; set; }
    }
}
