namespace UniversityTimetable.FrontEnd.Pages.AdminPages;

public sealed partial class EditUserRole
{
    protected override string PageAfterSave => "/users";

    private void RoleChanged(UserRole role)
    {
        Model!.Role = role;
        StateHasChanged();
    }
}