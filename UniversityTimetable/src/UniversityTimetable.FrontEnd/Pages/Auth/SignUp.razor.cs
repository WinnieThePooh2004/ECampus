using Microsoft.AspNetCore.Components;
using UniversityTimetable.FrontEnd.Auth;

namespace UniversityTimetable.FrontEnd.Pages.Auth;

public partial class SignUp
{
    protected override string PageAfterSave => "~/";

    [Inject] private IAuthService AuthService { get; set; } = default!;

    protected override async Task Save(UserDto model)
    {
        await AuthService.Login(model.Email, model.Password);
    }
}