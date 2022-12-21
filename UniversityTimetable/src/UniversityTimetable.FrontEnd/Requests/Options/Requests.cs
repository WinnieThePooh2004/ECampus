namespace UniversityTimetable.FrontEnd.Requests.Options
{
    public class Requests
    {
        public static Dictionary<Type, string> ControllerNames = new()
        {
            [typeof(FacultyDto)] = "Faculties",
            [typeof(TeacherDto)] = "Teachers",
            [typeof(AuditoryDto)] = "Auditories",
            [typeof(ClassDto)] = "Timetable",
            [typeof(GroupDto)] = "Groups",
            [typeof(SubjectDto)] = "Subjects",
            [typeof(DepartmentDTO)] = "Departments",
            [typeof(UserDto)] = "Users"
        };
    }
}
