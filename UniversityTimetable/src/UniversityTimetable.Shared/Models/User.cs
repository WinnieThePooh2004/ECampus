using UniversityTimetable.Shared.Enums;
using UniversityTimetable.Shared.Interfaces.Data;

namespace UniversityTimetable.Shared.Models
{
    public class User : IModel, IIsDeleted
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public bool IsDeleted { get; set; }
        public string Username { get; set; }
        public UserRole Role { get; set; }

        //public List<Group> SavedGroups { get; set; }
        //public List<Teacher> SavedTeachers { get; set; }
        //public List<Auditory> SavedAuditories { get; set; }
    }
}
