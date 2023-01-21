using ECampus.Shared.DataTransferObjects;
using ECampus.Shared.Enums;
using Microsoft.AspNetCore.Components;

namespace ECampus.FrontEnd.Components.EditForms;

public partial class UserRoleEditForm
{
    [Parameter] public UserDto Model { get; set; } = default!;
    
    private void RoleChanged(UserRole role)
    {
        Model.Role = role;
        StateHasChanged();
    }
}