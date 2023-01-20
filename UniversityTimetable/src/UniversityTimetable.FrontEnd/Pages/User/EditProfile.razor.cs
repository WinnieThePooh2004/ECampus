using FluentValidation;
using Microsoft.AspNetCore.Components;
using UniversityTimetable.FrontEnd.Requests.Interfaces;
using UniversityTimetable.FrontEnd.Validation.Interfaces;
using UniversityTimetable.Shared.Extensions;

namespace UniversityTimetable.FrontEnd.Pages.User;

public sealed partial class EditProfile
{
    private IValidator<UserDto> _validator = default!;
    [Inject] private IBaseRequests<UserDto> UserRequests { get; set; } = default!;
    [Inject] private NavigationManager NavigationManager { get; set; } = default!;
    [Inject] private IUserValidatorFactory UserValidatorFactory { get; set; } = default!;

    [Inject] private IHttpContextAccessor HttpContextAccessor { get; set; } = default!;
    private UserDto? _model;

    protected override async Task OnInitializedAsync()
    {
        _validator = UserValidatorFactory.UpdateValidator();
        _model = await UserRequests.GetByIdAsync(HttpContextAccessor.HttpContext?.User.GetId() ?? throw new Exception());
        _model.PasswordConfirm = _model.Password;
    }

    private async Task Save()
    {
        await UserRequests.UpdateAsync(_model ?? throw new NullReferenceException());
        NavigationManager.NavigateTo("/profile");
    }
}