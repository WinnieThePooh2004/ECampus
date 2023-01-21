using Microsoft.AspNetCore.Components;

namespace ECampus.FrontEnd.Pages.Timetable;

public partial class Group
{
    [Parameter] public int GroupId { get; set; }
    private bool _isSaved;

    private async Task OnSave()
    {
        await RelationsRequests.SaveGroup(GroupId);
        await RefreshData();
    }

    private async Task OnSaveRemoved()
    {
        await RelationsRequests.RemoveSavedGroup(GroupId);
        await RefreshData();
    }

    protected override async Task RefreshData()
    {
        await base.RefreshData();
        _isSaved = User?.SavedGroups?.Any(g => g.Id == GroupId) ?? false;
    }
}