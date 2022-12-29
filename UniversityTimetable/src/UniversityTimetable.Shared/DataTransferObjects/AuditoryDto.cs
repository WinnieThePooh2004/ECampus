using UniversityTimetable.Shared.Interfaces.Data;
using UniversityTimetable.Shared.Interfaces.Data.Models;

namespace UniversityTimetable.Shared.DataTransferObjects
{
    public class AuditoryDto : IDataTransferObject
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Building { get; set; }
    }
}
