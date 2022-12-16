using FluentValidation;
using Microsoft.AspNetCore.Components;

namespace UniversityTimetable.FrontEnd.Components.EditForms
{
    public class ModelEditForm<TModel> : ComponentBase where TModel : class
    {
        [Parameter] public TModel Model { get; set; }
        [Parameter] public EventCallback<TModel> OnSubmited { get; set; }

        [Inject] protected AbstractValidator<TModel> Validator { get; set; }

        protected virtual async Task Submit()
        {
            if(!await ValidateAsync(Model))
            {
                return;
            }

            await OnSubmited.InvokeAsync(Model);
        }

        protected virtual async Task<bool> ValidateAsync(TModel model)
        {
            return !(await Validator.ValidateAsync(Model)).Errors.Any();
        }
    }
}
