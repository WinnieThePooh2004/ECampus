using Microsoft.AspNetCore.Components;

namespace UniversityTimetable.FrontEnd.Pages.Timetable;

public partial class Teacher
{
    [Parameter] public int TeacherId { get; set; }

    private bool _isSaved;

    private async Task OnSave()
    {
        await RelationsRequests.SaveTeacher(TeacherId);
        await RefreshData();
    }

    private async Task OnSaveRemoved()
    {
        await RelationsRequests.RemoveSavedTeacher(TeacherId);
        await RefreshData();
    }

    protected override async Task RefreshData()
    {
        await base.RefreshData();
        _isSaved = User?.SavedTeachers?.Any(t => t.Id == TeacherId) ?? false;
    }
}