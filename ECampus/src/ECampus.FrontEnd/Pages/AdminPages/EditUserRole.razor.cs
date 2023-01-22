﻿using ECampus.FrontEnd.Requests;
using ECampus.Shared.DataTransferObjects;
using Microsoft.AspNetCore.Components;

namespace ECampus.FrontEnd.Pages.AdminPages;

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