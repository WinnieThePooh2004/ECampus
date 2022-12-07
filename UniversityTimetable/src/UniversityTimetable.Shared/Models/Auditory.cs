using UniversityTimetable.Shared.General;

namespace UniversityTimetable.Shared.Models
{
    public class Auditory : IIsDeleted
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Building { get; set; }

        public List<Class> Classes { get; set; }
        public bool IsDeleted { get; set; }
    }
}
