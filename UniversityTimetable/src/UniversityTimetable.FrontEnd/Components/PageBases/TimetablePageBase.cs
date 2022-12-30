using IdentityServer4.Extensions;
using Microsoft.AspNetCore.Components;
using UniversityTimetable.FrontEnd.Requests.Interfaces;

namespace UniversityTimetable.FrontEnd.Components.PageBases;

public abstract class TimetablePageBase : ComponentBase
{
    [Inject] protected IUserRequests UserRequests { get; set; } = default!;
    [Inject] protected IHttpContextAccessor HttpContextAccessor { get; set; } = default!;
    protected UserDto? User { get; private set; }

    protected override async Task OnInitializedAsync()
    {
        if (!(HttpContextAccessor.HttpContext?.User.IsAuthenticated() ?? false))
        {
            return;
        }

        await RefreshData();
    }

    protected virtual async Task RefreshData()
    {
        User = await UserRequests.GetCurrentUserAsync();
    }

    protected abstract Task OnSave();
    protected abstract Task OnSaveRemoved();
}