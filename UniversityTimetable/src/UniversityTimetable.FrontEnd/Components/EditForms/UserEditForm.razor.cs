using FluentValidation;
using Microsoft.AspNetCore.Components;
using UniversityTimetable.FrontEnd.Requests.Interfaces;
using UniversityTimetable.FrontEnd.Validation.Interfaces;

namespace UniversityTimetable.FrontEnd.Components.EditForms;

public partial class UserEditForm
{
    [Inject] private IUserValidatorFactory? ValidatorFactory { get; set; }
    [Inject] private IBaseRequests<UserDto>? Requests { get; set; }
    private IValidator<UserDto>? _validator;
    private UserDto _model = new();
    protected override void OnInitialized()
    {
        _validator = ValidatorFactory?.CreateValidator() ?? throw new NullReferenceException("ValidatorFactory is null");
    }

    private async Task Submit()
    {
        await Requests?.UpdateAsync(_model)!;
    }
}