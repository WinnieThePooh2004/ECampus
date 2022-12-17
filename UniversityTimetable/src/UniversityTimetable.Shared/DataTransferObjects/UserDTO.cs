using UniversityTimetable.Shared.Enums;
using UniversityTimetable.Shared.Interfaces.Data;
using UniversityTimetable.Shared.Models;

namespace UniversityTimetable.Shared.DataTransferObjects
{
    public class UserDTO : IDataTransferObject
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string PasswordConfirm { get; set; }
        public string Email { get; set; }
        public UserRole Role { get; set; }

        public List<Group> SavedGroups { get; set; }
        public List<Teacher> SavedTeachers { get; set; }
        public List<Auditory> SavedAuditories { get; set; }
    }
}
