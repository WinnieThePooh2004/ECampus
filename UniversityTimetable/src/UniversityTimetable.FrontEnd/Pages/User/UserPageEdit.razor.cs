using FluentValidation;
using Microsoft.AspNetCore.Components;
using UniversityTimetable.FrontEnd.Requests.Interfaces;
using UniversityTimetable.FrontEnd.Validation.Interfaces;

namespace UniversityTimetable.FrontEnd.Pages.User;

public partial class UserPageEdit
{
    private IValidator<UserDto> _validator;
    [Inject] private IUserRequests UserRequests { get; set; }
    [Inject] private NavigationManager NavigationManager { get; set; }
    [Inject] private IUserValidatorFactory UserValidatorFactory { get; set; }
    private UserDto _model;

    protected override async Task OnInitializedAsync()
    {
        _validator = UserValidatorFactory.UpdateValidator();
        _model = await UserRequests.GetCurrentUserAsync();
        _model.PasswordConfirm = _model.Password;
    }

    private async Task Save()
    {
        await UserRequests.UpdateAsync(_model);
        NavigationManager.NavigateTo("/profile");
    }
}