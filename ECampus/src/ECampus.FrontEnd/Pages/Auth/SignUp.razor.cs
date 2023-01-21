using ECampus.FrontEnd.Auth;
using ECampus.Shared.DataTransferObjects;
using Microsoft.AspNetCore.Components;

namespace ECampus.FrontEnd.Pages.Auth;

public partial class SignUp
{
    protected override string PageAfterSave => "~/";

    [Inject] private IAuthService AuthService { get; set; } = default!;

    protected override async Task Save(UserDto model)
    {
        await AuthService.Login(model.Email, model.Password);
    }
}