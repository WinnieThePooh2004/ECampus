using FluentValidation;
using Microsoft.AspNetCore.Components;
using UniversityTimetable.FrontEnd.Requests.Interfaces;

namespace UniversityTimetable.FrontEnd.Pages.User;

public partial class UserPageEdit
{
    [Inject] private IValidator<UserDto> Validator { get; set; }
    [Inject] private IUserRequests UserRequests { get; set; }
    [Inject] private NavigationManager NavigationManager { get; set; }
    private UserDto _model;

    protected override async Task OnInitializedAsync()
    {
        _model = await UserRequests.GetCurrentUserAsync();
    }

    private async Task Save()
    {
        await UserRequests.UpdateAsync(_model);
        NavigationManager.NavigateTo("/profile");
    }
}