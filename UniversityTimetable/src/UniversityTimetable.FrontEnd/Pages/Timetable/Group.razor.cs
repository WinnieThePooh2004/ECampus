using Microsoft.AspNetCore.Components;

namespace UniversityTimetable.FrontEnd.Pages.Timetable;

public partial class Group
{
    [Parameter] public int GroupId { get; set; }
    private bool _isSaved;
    protected override async Task OnSave()
    {
        await UserRequests.SaveGroup(GroupId);
        await RefreshData();
    }

    protected override async Task OnSaveRemoved()
    {
        await UserRequests.RemoveSavedGroup(GroupId);
        await RefreshData();
    }

    protected override async Task RefreshData()
    {
        await base.RefreshData();
        _isSaved = User?.SavedGroups?.Any(g => g.Id == GroupId) ?? false;
    }
}