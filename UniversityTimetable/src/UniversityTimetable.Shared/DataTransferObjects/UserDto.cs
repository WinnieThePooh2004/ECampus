using UniversityTimetable.Shared.Enums;
using UniversityTimetable.Shared.Interfaces.Data;
using UniversityTimetable.Shared.Models;

namespace UniversityTimetable.Shared.DataTransferObjects;

public class UserDto : IDataTransferObject
{
    public int Id { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
    public string PasswordConfirm { get; set; }
    public string Email { get; set; }
    public UserRole Role { get; set; }

    public List<AuditoryDto> SavedAuditories { get; set; } = default!;
    public List<GroupDto> SavedGroups { get; set; } = default!;
    public List<TeacherDto> SavedTeachers { get; set; } = default!;
}