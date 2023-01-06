using FluentValidation;
using Microsoft.AspNetCore.Components;
using UniversityTimetable.FrontEnd.Requests.Interfaces;
using UniversityTimetable.FrontEnd.Validation.Interfaces;

namespace UniversityTimetable.FrontEnd.Components.EditForms;

public partial class UserEditForm
{
    [Parameter] public EventCallback<UserDto> OnSubmit { get; set; }
    [Inject] private IUserValidatorFactory ValidatorFactory { get; set; } = default!;
    [Inject] private IBaseRequests<UserDto> Requests { get; set; } = default!;

    [Inject] private NavigationManager NavigationManager { get; set; } = default!;

    [Inject] private IHttpContextAccessor HttpContextAccessor { get; set; } = default!;

    private IValidator<UserDto>? _validator;
    private UserDto _model = new();

    protected override void OnInitialized()
    {
        _validator = ValidatorFactory.CreateValidator() ?? throw new NullReferenceException("ValidatorFactory is null");
    }

    private async Task Submit()
    {
        if (!await IsValid())
        {
            return;
        }

        await Requests.CreateAsync(_model);
        await OnSubmit.InvokeAsync(_model);
    }

    private async Task<bool> IsValid()
    {
        return (await _validator?.ValidateAsync(_model)!)?.IsValid ?? false;
    }
}