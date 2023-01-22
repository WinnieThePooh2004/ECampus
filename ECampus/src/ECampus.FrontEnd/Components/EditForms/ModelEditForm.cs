﻿using FluentValidation;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;

namespace ECampus.FrontEnd.Components.EditForms;

public class ModelEditForm<TModel> : ComponentBase where TModel : class
{
    [Parameter] public TModel Model { get; set; } = default!;
    [Parameter] public EventCallback<TModel> OnSubmit { get; set; }

    [CascadingParameter] protected EditContext EditContext { get; set; } = default!;

    protected override void OnInitialized()
    {
        EditContext = new EditContext(Model);
    }

    [Inject] protected IValidator<TModel>? Validator { get; set; }

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
        return !(await Validator?.ValidateAsync(Model)!).Errors.Any();
    }
}