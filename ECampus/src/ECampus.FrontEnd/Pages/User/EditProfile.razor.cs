using ECampus.FrontEnd.Extensions;
using ECampus.FrontEnd.Requests.Interfaces;
using ECampus.Shared.DataTransferObjects;
using Microsoft.AspNetCore.Components;

namespace ECampus.FrontEnd.Pages.User;

public sealed partial class EditProfile
{
    [Inject] private IBaseRequests<UserProfile> UserRequests { get; set; } = default!;
    [Inject] private NavigationManager NavigationManager { get; set; } = default!;
    [Inject] private IHttpContextAccessor HttpContextAccessor { get; set; } = default!;
    private UserProfile? _model;

    protected override async Task OnInitializedAsync()
    {
        _model = await UserRequests.GetCurrentUserAsync(HttpContextAccessor);
    }

    private async Task Save()
    {
        await Submit();
        NavigationManager.NavigateTo("/profile");
    }
}