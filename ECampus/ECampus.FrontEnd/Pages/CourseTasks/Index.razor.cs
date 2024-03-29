﻿using System.Net;
using System.Security.Claims;
using ECampus.FrontEnd.Requests.Interfaces;
using ECampus.Domain.Auth;
using ECampus.Domain.DataTransferObjects;
using ECampus.Domain.Enums;
using ECampus.Domain.Exceptions;
using ECampus.Domain.Responses.CourseTask;
using Microsoft.AspNetCore.Components;

namespace ECampus.FrontEnd.Pages.CourseTasks;

public partial class Index
{
    [Parameter] public int CourseId { get; set; }

    [Inject] private IBaseRequests<CourseDto> CourseRequests { get; set; } = default!;
    [Inject] private IHttpContextAccessor HttpContextAccessor { get; set; } = default!;

    private bool? _enableEdit;
    private UserRole _role;
    private List<(string LinkName, Func<MultipleCourseTaskResponse, string> LinkSource)>? _actionLinks;

    protected override async Task OnInitializedAsync()
    {
        _role = Enum.Parse<UserRole>(HttpContextAccessor.HttpContext!.User.FindFirst(ClaimTypes.Role)?.Value ??
                                     throw new UnauthorizedAccessException());
        _actionLinks = ActionLinks();
        if (_role is UserRole.Admin)
        {
            _enableEdit = true;
            return;
        }

        var usersTeacherIdString = HttpContextAccessor.HttpContext!.User.FindFirst(CustomClaimTypes.TeacherId)?.Value;
        if (usersTeacherIdString is null)
        {
            return;
        }

        var usersTeacherId = int.Parse(usersTeacherIdString);
        var currentCourse = await CourseRequests.GetByIdAsync(CourseId);
        if (currentCourse.Teachers!.Any(teacher => teacher.Id == usersTeacherId))
        {
            _enableEdit = true;
            return;
        }

        _enableEdit = false;
    }

    private List<(string LinkName, Func<MultipleCourseTaskResponse, string> LinkSource)> ActionLinks() =>
        _role switch
        {
            UserRole.Guest => throw new HttpResponseException(HttpStatusCode.Forbidden),
            UserRole.Teacher => new List<(string, Func<MultipleCourseTaskResponse, string>)>
            {
                ("View submissions", c => $"/submissions/{c.Id}")
            },
            UserRole.Student => new List<(string, Func<MultipleCourseTaskResponse, string>)>
            {
                ("Edit my submission", c => $"/editSubmission/{c.Id}")
            },
            _ => new List<(string, Func<MultipleCourseTaskResponse, string>)>()
        };
}