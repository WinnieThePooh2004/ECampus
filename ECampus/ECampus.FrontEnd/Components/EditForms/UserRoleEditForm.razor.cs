using ECampus.Domain.DataTransferObjects;
using ECampus.Domain.Enums;
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