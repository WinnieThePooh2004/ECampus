using ECampus.FrontEnd.Requests.Interfaces;
using ECampus.Domain.DataTransferObjects;
using Microsoft.AspNetCore.Components;

namespace ECampus.FrontEnd.Pages.User;

public sealed partial class PasswordChange
{
    [Inject] private IPasswordChangeRequests UserRequests { get; set; } = default!;

    [Inject] private NavigationManager NavigationManager { get; set; } = default!;
    
    private async Task Submit(PasswordChangeDto passwordChange)
    {
        await UserRequests.ChangePassword(passwordChange);
        NavigationManager.NavigateTo("profile");
    }
}