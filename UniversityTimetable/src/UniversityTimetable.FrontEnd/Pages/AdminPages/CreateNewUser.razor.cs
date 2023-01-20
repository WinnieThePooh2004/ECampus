using FluentValidation;
using Microsoft.AspNetCore.Components;
using UniversityTimetable.FrontEnd.Requests;
using UniversityTimetable.FrontEnd.Validation.Interfaces;

namespace UniversityTimetable.FrontEnd.Pages.AdminPages;

public partial class CreateNewUser
{
    [Inject] private IUserRolesRequests UserRolesRequests { get; set; } = default!;
    [Inject] private IUserValidatorFactory UserValidatorFactory { get; set; } = default!;

    [Inject] private NavigationManager NavigationManager { get; set; } = default!;
    
    private readonly UserDto _model = new();
    private IValidator<UserDto> _validator = default!;

    protected override void OnInitialized()
    {
        _validator = UserValidatorFactory.CreateValidator();
    }

    private void OnPasswordChanges(string? password)
    {
        _model.Password = password ?? "";
        _model.PasswordConfirm = _model.Password;
    }

    private async Task Save()
    {
        await UserRolesRequests.CreateAsync(_model);
        NavigationManager.NavigateTo("/users");
    }
}