using System.Net;
using System.Security.Claims;
using ECampus.Domain.Auth;
using ECampus.Domain.DataTransferObjects;
using ECampus.Domain.Enums;
using ECampus.Domain.Exceptions;
using ECampus.Domain.Requests.Course;
using Microsoft.AspNetCore.Components;

namespace ECampus.FrontEnd.Pages.User;

public partial class MyCourses
{
    [Inject] private IHttpContextAccessor HttpContextAccessor { get; set; } = default!;

    private int _teacherId;
    private int _groupId;
    private int _studentId;
    private UserRole _role;
    private List<(string LinkName, Func<CourseDto, string> LinkSource)>? _actionLinks;

    protected override void OnInitialized()
    {
        var user = HttpContextAccessor.HttpContext?.User;
        _groupId = int.Parse(user?.FindFirst(CustomClaimTypes.GroupId)?.Value ?? "0");
        _teacherId = int.Parse(user?.FindFirst(CustomClaimTypes.TeacherId)?.Value ?? "0");
        _studentId = int.Parse(user?.FindFirst(CustomClaimTypes.StudentId)?.Value ?? "0");
        _role = Enum.Parse<UserRole>(user?.FindFirst(ClaimTypes.Role)?.Value ??
                                     throw new UnauthorizedAccessException());
        _actionLinks = ActionLinks();
    }

    private void SetParameters(CourseParameters parameters)
    {
        parameters.GroupId = _groupId;
        parameters.TeacherId = _teacherId;
        parameters.OrderBy = "Name";
    }

    private List<(string LinkName, Func<CourseDto, string> LinkSource)> ActionLinks() =>
        _role switch
        {
            UserRole.Guest => throw new HttpResponseException(HttpStatusCode.Forbidden),
            UserRole.Teacher => new List<(string, Func<CourseDto, string>)>
            {
                ("View submissions", c => $"/submissions/{c.Id}")
            },
            UserRole.Student => new List<(string, Func<CourseDto, string>)>
            {
                ("Edit my submission", c => $"/submission/Edit/{c.Id}")
            },
            _ => new List<(string, Func<CourseDto, string>)>()
        };
}