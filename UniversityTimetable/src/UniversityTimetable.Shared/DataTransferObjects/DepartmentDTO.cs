using UniversityTimetable.Shared.Interfaces.Data;

namespace UniversityTimetable.Shared.DataTransferObjects
{
    public class DepartmentDTO : IDataTransferObject
    {
        public int Id { get; set; }
        public string Name { get; set; } = default!;

        public int FacultacyId { get; set; }
        public List<TeacherDTO> Teachers { get; set; }
        public List<GroupDTO> Groups { get; set; }
    }
}
