using IdentityServer4.Extensions;
using Microsoft.AspNetCore.Components;
using UniversityTimetable.FrontEnd.Requests.Interfaces;

namespace UniversityTimetable.FrontEnd.Pages.Timetable;

public partial class Auditory
{
    [Parameter] public int AuditoryId { get; set; }
    [Inject] private IUserRequests UserRequests { get; set; }
    [Inject] private IHttpContextAccessor Context { get; set; }

    private UserDto _user;
    private bool _isSaved = false;

    protected override async Task OnInitializedAsync()
    {
        if (!(Context.HttpContext?.User.IsAuthenticated() ?? false))
        {
            return;
        }

        await RefreshData();
    }

    private async Task SaveAuditory()
    {
        await UserRequests.SaveAuditory(AuditoryId);
        await RefreshData();
    }

    private async Task RemoveAuditory()
    {
        await UserRequests.RemoveSavedAuditory(AuditoryId);
        await RefreshData();
    }

    private async Task RefreshData()
    {
        _user = await UserRequests.GetCurrentUserAsync();
        _isSaved = _user.SavedAuditories.Any(a => a.Id == AuditoryId);
        StateHasChanged();
    }
}