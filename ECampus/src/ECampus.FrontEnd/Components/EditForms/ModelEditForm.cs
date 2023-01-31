using FluentValidation;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;

namespace ECampus.FrontEnd.Components.EditForms;

public class ModelEditForm<TModel> : ComponentBase where TModel : class
{
    [Parameter] public TModel Model { get; set; } = default!;
    [Parameter] public EventCallback<TModel> OnSubmit { get; set; }
    [Inject] protected IValidator<TModel> Validator { get; set; } = default!;

    [CascadingParameter] protected EditContext EditContext { get; set; } = default!;

    protected override void OnInitialized()
    {
        EditContext = new EditContext(Model);
    }
    
    protected async Task Submit()
    {
        if(!await ValidateAsync())
        {
            return;
        }

        await OnSubmit.InvokeAsync(Model);
    }

    private async Task<bool> ValidateAsync()
    {
        var errors = await Validator.ValidateAsync(Model)!;
        return !errors.Errors.Any();
    }
}