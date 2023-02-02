using ECampus.Shared.DataTransferObjects;

namespace ECampus.FrontEnd.Requests.Options;

public class RequestOptions : IRequestOptions
{
    public const string ClientName = "UTApi";
    private readonly Dictionary<Type, string> _controllerNames;

    public RequestOptions(IConfiguration configuration)
    {
        _controllerNames = new Dictionary<Type, string>
        {
            [typeof(FacultyDto)] = configuration["Requests:Faculties"] ??
                                   throw new KeyNotFoundException("There is not config for 'Faculties'"),
            [typeof(TeacherDto)] = configuration["Requests:Teachers"] ??
                                   throw new KeyNotFoundException("There is not config for 'Teachers'"),
            [typeof(AuditoryDto)] = configuration["Requests:Auditories"] ??
                                    throw new KeyNotFoundException("There is not config for 'Auditories'"),
            [typeof(ClassDto)] = configuration["Requests:Timetable"] ??
                                 throw new KeyNotFoundException("There is not config for 'Timetable'"),
            [typeof(GroupDto)] = configuration["Requests:Groups"] ??
                                 throw new KeyNotFoundException("There is not config for 'Groups'"),
            [typeof(SubjectDto)] = configuration["Requests:Subjects"] ??
                                   throw new KeyNotFoundException("There is not config for 'Subjects'"),
            [typeof(DepartmentDto)] = configuration["Requests:Departments"] ??
                                      throw new KeyNotFoundException("There is not config for 'Departments'"),
            [typeof(UserDto)] = configuration["Requests:Users"] ??
                                throw new KeyNotFoundException("There is not config for 'Users'"),
            [typeof(StudentDto)] = configuration["Requests:Students"] ??
                                   throw new KeyNotFoundException("There is not config for 'Students'"),
            [typeof(CourseDto)] = configuration["Requests:Courses"] ??
                                  throw new KeyNotFoundException("There is not config for 'Courses'"),
            [typeof(CourseTaskDto)] = configuration["Requests:CourseTasks"] ??
                                      throw new KeyNotFoundException("There is not config for 'CourseTasks'"),
            [typeof(TaskSubmissionDto)] = configuration["Requests:TaskSubmissions"] ??
                                          throw new KeyNotFoundException("There is not config for 'TaskSubmissions'"),
            [typeof(CourseSummary)] = configuration["Requests:CourseSummary"] ??
                                      throw new KeyNotFoundException("There is not config for 'CourseSummary'")
        };
    }

    public string GetControllerName(Type objectType) => _controllerNames[objectType];
}