﻿using ECampus.FrontEnd.Requests.Interfaces;
using ECampus.FrontEnd.Validation.Interfaces;
using ECampus.Shared.DataTransferObjects;
using ECampus.Shared.Extensions;
using FluentValidation;
using Microsoft.AspNetCore.Components;

namespace ECampus.FrontEnd.Pages.User;

public sealed partial class EditProfile
{
    [Inject] private IBaseRequests<UserDto> UserRequests { get; set; } = default!;
    [Inject] private NavigationManager NavigationManager { get; set; } = default!;
    [Inject] private IUserValidatorFactory UserValidatorFactory { get; set; } = default!;

    [Inject] private IHttpContextAccessor HttpContextAccessor { get; set; } = default!;
    private IValidator<UserDto> _validator = default!;
    private UserDto? _model;

    protected override async Task OnInitializedAsync()
    {
        _validator = UserValidatorFactory.UpdateValidator();
        _model = await UserRequests.GetByIdAsync(HttpContextAccessor.HttpContext?.User.GetId() ?? throw new Exception());
        _model.PasswordConfirm = _model.Password;
    }

    private async Task Save()
    {
        var errors = await _validator.ValidateAsync(_model!);
        if (!errors.IsValid)
        {
            return;
        }
        await UserRequests.UpdateAsync(_model ?? throw new NullReferenceException());
        NavigationManager.NavigateTo("/profile");
    }
}