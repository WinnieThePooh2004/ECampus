using UniversityTimetable.Shared.Interfaces.Data;
namespace UniversityTimetable.Shared.DataTransferObjects
{
    public class SubjectDTO : IDataTransferObject
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public List<TeacherDTO> Teachers { get; set; } = new();
    }
}
