using Microsoft.AspNetCore.Components;
using UniversityTimetable.FrontEnd.Requests.Interfaces;

namespace UniversityTimetable.FrontEnd.Pages.User;

public partial class UserPage
{
    [Inject] private IUserRequests Requests { get; set; }
    private UserDto _user;

    protected override async Task OnInitializedAsync()
    {
        _user = await Requests.GetCurrentUserAsync();
    }
}