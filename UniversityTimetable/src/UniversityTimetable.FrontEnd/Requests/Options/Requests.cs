namespace UniversityTimetable.FrontEnd.Requests.Options
{
    public class Requests
    {
        public static Dictionary<Type, string> ControllerNames = new()
        {
            [typeof(FacultyDTO)] = "Faculties",
            [typeof(TeacherDTO)] = "Teachers",
            [typeof(AuditoryDTO)] = "Auditories",
            [typeof(ClassDTO)] = "Timetable",
            [typeof(GroupDTO)] = "Groups",
            [typeof(SubjectDTO)] = "Subjects",
            [typeof(DepartmentDTO)] = "Departments",
        };
    }
}
