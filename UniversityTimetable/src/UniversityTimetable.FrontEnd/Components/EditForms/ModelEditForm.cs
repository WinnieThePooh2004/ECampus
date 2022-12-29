using FluentValidation;
using Microsoft.AspNetCore.Components;

namespace UniversityTimetable.FrontEnd.Components.EditForms
{
    public class ModelEditForm<TModel> : ComponentBase where TModel : class
    {
        [Parameter] public TModel Model { get; set; }
        [Parameter] public EventCallback<TModel> OnSubmit { get; set; }

        [Inject] protected IValidator<TModel> Validator { get; set; }

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
            return !(await Validator.ValidateAsync(Model)).Errors.Any();
        }
    }
}
