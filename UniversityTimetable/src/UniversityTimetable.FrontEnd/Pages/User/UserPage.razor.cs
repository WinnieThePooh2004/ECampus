using Microsoft.AspNetCore.Components;
using UniversityTimetable.FrontEnd.Extensions;
using UniversityTimetable.FrontEnd.Requests.Interfaces;

namespace UniversityTimetable.FrontEnd.Pages.User;

public partial class UserPage
{
    [Inject] private IUserRequests Requests { get; set; } = default!;
    [Inject] private IHttpContextAccessor HttpContextAccessor { get; set; } = default!;
    private UserDto? _user;

    protected override async Task OnInitializedAsync()
    {
        _user = await Requests.GetCurrentUserAsync(HttpContextAccessor);
    }
}