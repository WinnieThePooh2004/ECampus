using UniversityTimetable.Shared.Interfaces.Data;

namespace UniversityTimetable.Shared.DataTransferObjects
{
    public class AuditoryDTO : IDataTransferObject
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Building { get; set; }
    }
}
