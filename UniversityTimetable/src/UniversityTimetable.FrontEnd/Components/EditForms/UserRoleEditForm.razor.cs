using Microsoft.AspNetCore.Components;

namespace UniversityTimetable.FrontEnd.Components.EditForms;

public partial class UserRoleEditForm
{
    [Parameter] public UserDto Model { get; set; } = default!;
    
    private void RoleChanged(UserRole role)
    {
        Model.Role = role;
        StateHasChanged();
    }
}