using Microsoft.AspNetCore.Components;

namespace ECampus.FrontEnd.Pages.Timetable;

public sealed partial class Auditory
{
    [Parameter] public int AuditoryId { get; set; }
    
    private bool _isSaved;
    
    protected override async Task RefreshData()
    {
        await base.RefreshData();
        _isSaved = User?.SavedAuditories?.Any(a => a.Id == AuditoryId) ?? false;
        StateHasChanged();
    }

    private async Task OnSave()
    {
        await RelationsRequests.SaveAuditory(AuditoryId);
        await RefreshData();
    }

    private async Task OnSaveRemoved()
    {
        await RelationsRequests.RemoveSavedAuditory(AuditoryId);
        await RefreshData();
    }
}