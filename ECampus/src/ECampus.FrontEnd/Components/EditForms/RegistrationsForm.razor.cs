using ECampus.FrontEnd.Requests.Interfaces;
using ECampus.FrontEnd.Validation.Interfaces;
using ECampus.Shared.DataTransferObjects;
using FluentValidation;
using Microsoft.AspNetCore.Components;

namespace ECampus.FrontEnd.Components.EditForms;

public partial class RegistrationsForm
{
    [Parameter] public EventCallback<UserDto> OnSubmit { get; set; }
    [Inject] private IUserValidatorFactory ValidatorFactory { get; set; } = default!;

    private IValidator<UserDto> _validator = default!;
    private readonly UserDto _model = new();

    protected override void OnInitialized()
    {
        _validator = ValidatorFactory.CreateValidator();
    }

    private async Task Submit()
    {
        if (!await IsValid())
        {
            return;
        }

        await OnSubmit.InvokeAsync(_model);
    }

    private async Task<bool> IsValid()
    {
        var errors = await _validator.ValidateAsync(_model);
        return errors.IsValid;
    }
}