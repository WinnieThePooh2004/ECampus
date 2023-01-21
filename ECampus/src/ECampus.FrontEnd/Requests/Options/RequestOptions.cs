using ECampus.Shared.DataTransferObjects;

namespace ECampus.FrontEnd.Requests.Options;

public class RequestOptions : IRequestOptions
{
    private readonly Dictionary<Type, string> _controllerNames;
    
    public RequestOptions(IConfiguration configuration)
    {
        _controllerNames = new Dictionary<Type, string>
        {
            [typeof(FacultyDto)] = configuration["Requests:Faculties"]!,
            [typeof(TeacherDto)] = configuration["Requests:Teachers"]!,
            [typeof(AuditoryDto)] = configuration["Requests:Auditories"]!,
            [typeof(ClassDto)] = configuration["Requests:Timetable"]!,
            [typeof(GroupDto)] = configuration["Requests:Groups"]!,
            [typeof(SubjectDto)] = configuration["Requests:Subjects"]!,
            [typeof(DepartmentDto)] = configuration["Requests:Departments"]!,
            [typeof(UserDto)] = configuration["Requests:Users"]!,
            [typeof(StudentDto)] = configuration["Requests:Students"]!,
            [typeof(CourseDto)] = configuration["Requests:Courses"]!,
            [typeof(CourseTaskDto)] = configuration["Requests:CourseTasks"]!,
            [typeof(TaskSubmissionDto)] = configuration["Requests:TaskSubmissions"]!
        };
    }

    public string GetControllerName(Type objectType) => _controllerNames[objectType];
}