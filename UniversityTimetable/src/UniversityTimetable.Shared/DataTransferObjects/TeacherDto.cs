using UniversityTimetable.Shared.Enums;
using UniversityTimetable.Shared.Interfaces.Data.Models;
using UniversityTimetable.Shared.Metadata;
using UniversityTimetable.Shared.Models;

namespace UniversityTimetable.Shared.DataTransferObjects;

[Dto<Teacher>]
public class TeacherDto : IDataTransferObject
{
    public int Id { get; set; }
    [DisplayName("First name")]
    public string FirstName { get; set; } = string.Empty;
    [DisplayName("Last name")]
    public string LastName { get; set; } = string.Empty;
    [DisplayName("Science degree")]
    public ScienceDegree ScienceDegree { get; set; }
    
    public int? UserId { get; set; }

    public int DepartmentId { get; set; }
    public List<SubjectDto>? Subjects { get; set; }
}