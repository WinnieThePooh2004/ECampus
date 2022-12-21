using Microsoft.AspNetCore.Components;

namespace UniversityTimetable.FrontEnd.Pages.Timetable;

public partial class Teacher
{
    [Parameter] public int TeacherId { get; set; }

    private bool _isSaved;

    protected override async Task OnSave()
    {
        await UserRequests.SaveTeacher(TeacherId);
        await RefreshData();
    }

    protected override async Task OnSaveRemoved()
    {
        await UserRequests.RemoveSavedTeacher(TeacherId);
        await RefreshData();
    }

    protected override async Task RefreshData()
    {
        await base.RefreshData();
        _isSaved = User.SavedTeachers.Any(t => t.Id == TeacherId);
    }
}