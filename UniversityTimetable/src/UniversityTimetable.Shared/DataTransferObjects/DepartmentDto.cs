using UniversityTimetable.Shared.Interfaces.Data;

namespace UniversityTimetable.Shared.DataTransferObjects
{
    public class DepartmentDTO : IDataTransferObject
    {
        public int Id { get; set; }
        public string Name { get; set; } = default!;

        public int FacultacyId { get; set; }
        public List<TeacherDto> Teachers { get; set; }
        public List<GroupDto> Groups { get; set; }
    }
}
