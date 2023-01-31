using ECampus.FrontEnd.Auth;
using ECampus.FrontEnd.Requests.Interfaces;
using ECampus.Shared.DataTransferObjects;
using Microsoft.AspNetCore.Components;

namespace ECampus.FrontEnd.Pages.Auth;

public partial class SignUp
{
    protected override string PageAfterSave => "~/";

    [Inject] private IAuthService AuthService { get; set; } = default!;
    [Inject] private IBaseRequests<UserDto> Requests { get; set; } = default!;

    protected override async Task Save(UserDto model)
    {
        await Requests.CreateAsync(model);
        await AuthService.Login(model.Email, model.Password);
    }
}