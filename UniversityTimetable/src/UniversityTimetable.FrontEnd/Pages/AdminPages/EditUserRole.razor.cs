using Microsoft.AspNetCore.Components;
using UniversityTimetable.FrontEnd.Requests;

namespace UniversityTimetable.FrontEnd.Pages.AdminPages;

public sealed partial class EditUserRole
{
    [Parameter] public int Id { get; set; }
    [Inject] private IUserRolesRequests UserRolesService { get; set; } = default!;

    [Inject] private NavigationManager NavigationManager { get; set; } = default!;
    
    private UserDto? _user;

    protected override async Task OnInitializedAsync()
    {
        _user = await UserRolesService.GetByIdAsync(Id);
    }

    private async Task Save()
    {
        await UserRolesService.UpdateAsync(_user!);
        NavigationManager.NavigateTo("/users");
    }
}