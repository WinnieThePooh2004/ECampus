using UniversityTimetable.Shared.General;

namespace UniversityTimetable.Shared.Models
{
    public class Group : IIsDeleted
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsDeleted { get; set; }

        public Department Department { get; set; }
        public int DepartmentId { get; set; }

        public List<Class> Classes { get; set; }
    }
}
