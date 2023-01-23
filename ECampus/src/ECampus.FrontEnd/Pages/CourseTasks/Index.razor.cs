using System.Security.Claims;
using ECampus.FrontEnd.Requests.Interfaces;
using ECampus.Shared.Auth;
using ECampus.Shared.DataTransferObjects;
using ECampus.Shared.Enums;
using Microsoft.AspNetCore.Components;

namespace ECampus.FrontEnd.Pages.CourseTasks;

public partial class Index
{
    [Parameter] public int CourseId { get; set; }

    [Inject] private IBaseRequests<CourseDto> CourseRequests { get; set; } = default!;
    [Inject] private IHttpContextAccessor HttpContextAccessor { get; set; } = default!;

    private bool? _enableEdit;

    protected override async Task OnInitializedAsync()
    {
        if (HttpContextAccessor.HttpContext!.User.FindFirst(ClaimTypes.Role)?.Value == nameof(UserRole.Admin))
        {
            _enableEdit = true;
            return;
        }

        var usersTeacherIdString = HttpContextAccessor.HttpContext!.User.FindFirst(CustomClaimTypes.TeacherId)?.Value;
        if (usersTeacherIdString is null)
        {
            return;
        }

        var currentCourse = await CourseRequests.GetByIdAsync(CourseId);
        var usersTeacherId = int.Parse(usersTeacherIdString);
        if (currentCourse.Teachers!.Any(teacher => teacher.Id == usersTeacherId))
        {
            _enableEdit = true;
            return;
        }

        _enableEdit = false;
    }
}