using UniversityTimetable.Shared.Enums;
using UniversityTimetable.Shared.Interfaces.Data;

namespace UniversityTimetable.Shared.Models
{
    public class Auditory : IIsDeleted, IModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Building { get; set; }

        public List<Class> Classes { get; set; }
        public bool IsDeleted { get; set; }
    }
}
