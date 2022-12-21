using Microsoft.AspNetCore.Components;

namespace UniversityTimetable.FrontEnd.Pages.Auth;

public partial class SignUp
{
    protected override string PageAfterSave => "~/";
    [Inject] private NavigationManager NavigationManager { get; set; }
    protected override async Task Save(UserDto model)
    {
        await base.Save(model);
        NavigationManager.NavigateTo($"/login?username={model.Username}&password={model.Email}");
    }
}