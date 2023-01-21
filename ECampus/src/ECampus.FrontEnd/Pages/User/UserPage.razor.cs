using ECampus.FrontEnd.Extensions;
using ECampus.FrontEnd.Requests.Interfaces;
using ECampus.Shared.DataTransferObjects;
using Microsoft.AspNetCore.Components;

namespace ECampus.FrontEnd.Pages.User;

public partial class UserPage
{
    [Inject] private IBaseRequests<UserDto> Requests { get; set; } = default!;
    [Inject] private IHttpContextAccessor HttpContextAccessor { get; set; } = default!;
    private UserDto? _user;

    protected override async Task OnInitializedAsync()
    {
        _user = await Requests.GetCurrentUserAsync(HttpContextAccessor);
    }
}