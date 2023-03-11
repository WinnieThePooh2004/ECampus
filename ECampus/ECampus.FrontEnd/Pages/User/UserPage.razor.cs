using ECampus.FrontEnd.Extensions;
using ECampus.FrontEnd.Requests.Interfaces;
using ECampus.Domain.DataTransferObjects;
using Microsoft.AspNetCore.Components;

namespace ECampus.FrontEnd.Pages.User;

public partial class UserPage
{
    [Inject] private IBaseRequests<UserProfile> Requests { get; set; } = default!;
    [Inject] private IHttpContextAccessor HttpContextAccessor { get; set; } = default!;
    private UserProfile? _user;

    protected override async Task OnInitializedAsync()
    {
        _user = await Requests.GetCurrentUserAsync(HttpContextAccessor);
    }
}