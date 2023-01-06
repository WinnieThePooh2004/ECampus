using IdentityServer4.Extensions;
using Microsoft.AspNetCore.Components;

namespace UniversityTimetable.FrontEnd.Pages.Timetable;

public sealed partial class Auditory
{
    [Parameter] public int AuditoryId { get; set; }

    private bool _isSaved;

    protected override async Task OnInitializedAsync()
    {
        if (!(HttpContextAccessor.HttpContext?.User.IsAuthenticated() ?? false))
        {
            return;
        }

        await RefreshData();
    }

    protected override async Task RefreshData()
    {
        await base.RefreshData();
        _isSaved = User?.SavedAuditories?.Any(a => a.Id == AuditoryId) ?? false;
        StateHasChanged();
    }

    protected override async Task OnSave()
    {
        await UserRequests.SaveAuditory(AuditoryId);
        await RefreshData();
    }

    protected override async Task OnSaveRemoved()
    {
        await UserRequests.RemoveSavedAuditory(AuditoryId);
        await RefreshData();
    }
}