using Microsoft.AspNetCore.Components;
using UniversityTimetable.FrontEnd.Requests.Interfaces;

namespace UniversityTimetable.FrontEnd.Components.PageBases
{
    public abstract class CreatePageBase<TData> : ComponentBase
    {
        [Inject] protected IBaseRequests<TData> Requests { get; set; }
        [Inject] protected NavigationManager Navigation { get; set; }
        protected abstract string PageAfterSave { get; }

        protected virtual async Task Save(TData model)
        {
            await Requests.CreateAsync(model);
            Navigation.NavigateTo(PageAfterSave);
        }
    }
}
