using UniversityTimetable.Shared.Enums;
using UniversityTimetable.Shared.Interfaces.Data;

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
    }
}
