using System.Diagnostics;

namespace UniversityTimetable.FrontEnd.Requests.Options;

public class RequestOptions : IRequestOptions
{
    private readonly Dictionary<Type, string> _controllerNames;
    
    public RequestOptions(IConfiguration configuration)
    {
        _controllerNames = new Dictionary<Type, string>
        {
            [typeof(FacultyDto)] = configuration["Requests:Faculties"] ?? throw new UnreachableException($"find route to Faculties"),
            [typeof(TeacherDto)] = configuration["Requests:Teachers"] ?? throw new UnreachableException($"find route to Teachers"),
            [typeof(AuditoryDto)] = configuration["Requests:Auditories"] ?? throw new UnreachableException($"find route to Auditories"),
            [typeof(ClassDto)] = configuration["Requests:Timetable"] ?? throw new UnreachableException($"find route to Timetable"),
            [typeof(GroupDto)] = configuration["Requests:Groups"] ?? throw new UnreachableException($"find route to Groups"),
            [typeof(SubjectDto)] = configuration["Requests:Subjects"] ?? throw new UnreachableException($"find route to Subjects"),
            [typeof(DepartmentDto)] = configuration["Requests:Departments"] ?? throw new UnreachableException($"find route to Departments"),
            [typeof(UserDto)] = configuration["Requests:Users"] ?? throw new UnreachableException($"find route to users"),
            [typeof(StudentDto)] = configuration["Requests:Students"] ?? throw new UnreachableException($"find route to users")
        };
    }

    public string GetControllerName(Type objectType) => _controllerNames[objectType];
}