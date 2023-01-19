using UniversityTimetable.Shared.Interfaces.Data.Models;
using UniversityTimetable.Shared.Metadata;
using UniversityTimetable.Shared.Models;

namespace UniversityTimetable.Shared.DataTransferObjects;

[Dto<Student>]
public class StudentDto : IDataTransferObject
{
    public int Id { get; set; }
    [DisplayName("Last name")]
    public string LastName { get; set; } = string.Empty;
    [DisplayName("First name")]
    public string FirstName { get; set; } = string.Empty;
    public int GroupId { get; set; }
    
    public int? UserId { get; set; }
}