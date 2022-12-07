using UniversityTimetable.Shared.General;

namespace UniversityTimetable.Shared.Models
{
    public class Facultacy : IIsDeleted
    {
        public int Id { get; set; }
        public string Name { get; set; } = default!;        
        public bool IsDeleted { get; set; }
        public List<Department> Departments { get; set; }
    }
}
