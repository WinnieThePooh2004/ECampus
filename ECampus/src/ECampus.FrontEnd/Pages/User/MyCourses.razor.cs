using ECampus.Shared.Auth;
using ECampus.Shared.QueryParameters;
using Microsoft.AspNetCore.Components;

namespace ECampus.FrontEnd.Pages.User;

public partial class MyCourses
{
    [Inject] private IHttpContextAccessor HttpContextAccessor { get; set; } = default!;

    private int _teacherId;

    private int _groupId;

    protected override void OnInitialized()
    {
        int.TryParse(HttpContextAccessor.HttpContext?.User.FindFirst(CustomClaimTypes.GroupId)?.Value ?? "0",
            out _groupId);
        int.TryParse(HttpContextAccessor.HttpContext?.User.FindFirst(CustomClaimTypes.TeacherId)?.Value ?? "0",
            out _teacherId);
    }

    private void SetParameters(CourseParameters parameters)
    {
        parameters.GroupId = _groupId;
        parameters.TeacherId = _teacherId;
        parameters.OrderBy = "Name";
    }
}