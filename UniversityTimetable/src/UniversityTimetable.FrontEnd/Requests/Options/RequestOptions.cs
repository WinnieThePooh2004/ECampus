namespace UniversityTimetable.FrontEnd.Requests.Options;

public class RequestOptions : IRequestOptions
{
    private readonly Dictionary<Type, string> _controllerNames;
    
    public RequestOptions(ConfigurationManager configuration)
    {
        _controllerNames = new Dictionary<Type, string>
        {
            [typeof(FacultyDto)] = configuration["Requests:Faculties"],
            [typeof(TeacherDto)] = configuration["Requests:Teachers"],
            [typeof(AuditoryDto)] = configuration["Requests:Auditories"],
            [typeof(ClassDto)] = configuration["Requests:Timetable"],
            [typeof(GroupDto)] = configuration["Requests:Groups"],
            [typeof(SubjectDto)] = configuration["Requests:Subjects"],
            [typeof(DepartmentDto)] = configuration["Requests:Departments"],
            [typeof(UserDto)] = configuration["Requests:Users"]
        };
    }

    public string GetControllerName(Type objectType) => _controllerNames[objectType];
}