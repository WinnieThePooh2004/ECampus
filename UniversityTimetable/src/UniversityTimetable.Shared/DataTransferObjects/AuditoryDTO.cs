namespace UniversityTimetable.Shared.DataTransferObjects
{
    public class AuditoryDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Building { get; set; }

        public List<ClassDTO> Classes { get; set; }

        public string FullName => $"b. {Building}: aud. {Name}";
    }
}
