using UniversityTimetable.Shared.Interfaces.Data;

namespace UniversityTimetable.Shared.DataTransferObjects;

public class SubjectDto : IDataTransferObject
{
    public int Id { get; set; }
    public string Name { get; set; }

    public List<TeacherDto> Teachers { get; set; } = new();
}