using Microsoft.AspNetCore.Components;

namespace UniversityTimetable.FrontEnd.Components.EditForms
{
    public class EditForm<TModel> : ComponentBase where TModel : class
    {
        [Parameter] public TModel Model { get; set; }
        [Parameter] public EventCallback<TModel> OnSubmited { get; set; }

        protected virtual Task Submit()
        {
            return OnSubmited.InvokeAsync(Model);
        }
    }
}
