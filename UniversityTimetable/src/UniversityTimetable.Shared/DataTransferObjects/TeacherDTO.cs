using System.Text.Json.Serialization;
using UniversityTimetable.Shared.General;
using UniversityTimetable.Shared.Interfaces.Data;

namespace UniversityTimetable.Shared.DataTransferObjects
{
    public class TeacherDTO : IDataTransferObject
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public ScienceDegree ScienceDegree { get; set; }

        public int DepartmentId { get; set; }
        public List<SubjectDTO> Subjects { get; set; } = new();
        [JsonIgnore] public string FullName => $"{FirstName[0]}. {LastName}";
    }
}
