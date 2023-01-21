using ECampus.FrontEnd.Extensions;
using ECampus.FrontEnd.Requests.Interfaces;
using ECampus.Shared.DataTransferObjects;
using IdentityServer4.Extensions;
using Microsoft.AspNetCore.Components;

namespace ECampus.FrontEnd.Components.PageBases;

public abstract class TimetablePageBase : ComponentBase
{
    [Inject] protected IUserRelationshipsRequests RelationsRequests { get; set; } = default!;
    [Inject] private IBaseRequests<UserDto> UserRequests { get; set; } = default!;
    [Inject] protected IHttpContextAccessor HttpContextAccessor { get; set; } = default!;
    protected UserDto? User { get; private set; }

    protected override async Task OnInitializedAsync()
    {
        if (!HttpContextAccessor.HttpContext!.User.IsAuthenticated())
        {
            return;
        }

        await RefreshData();
    }

    protected virtual async Task RefreshData()
    {
        User = await UserRequests.GetCurrentUserAsync(HttpContextAccessor);
    }
}