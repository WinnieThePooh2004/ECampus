using UniversityTimetable.Shared.General;

namespace UniversityTimetable.Shared.Models
{
    public class Department : IIsDeleted
    {
        public int Id { get; set; }
        public string Name { get; set; } = default!;
        public bool IsDeleted { get; set; }

        public int FacultacyId { get; set; }
        public Faculty Facultacy { get; set; }
        public List<Teacher> Teachers { get; set; }
        public List<Group> Groups { get; set; }
    }
}
