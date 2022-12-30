using FluentValidation;
using Microsoft.AspNetCore.Components;
using UniversityTimetable.FrontEnd.Requests.Interfaces;
using UniversityTimetable.FrontEnd.Validation.Interfaces;

namespace UniversityTimetable.FrontEnd.Pages.User;

public partial class UserPageEdit
{
    private IValidator<UserDto> _validator = default!;
    [Inject] private IUserRequests UserRequests { get; set; } = default!;
    [Inject] private NavigationManager NavigationManager { get; set; } = default!;
    [Inject] private IUserValidatorFactory UserValidatorFactory { get; set; } = default!;
    private UserDto? _model;

    protected override async Task OnInitializedAsync()
    {
        _validator = UserValidatorFactory.UpdateValidator();
        _model = await UserRequests.GetCurrentUserAsync();
        _model.PasswordConfirm = _model.Password;
    }

    private async Task Save()
    {
        await UserRequests.UpdateAsync(_model ?? throw new NullReferenceException());
        NavigationManager.NavigateTo("/profile");
    }
}