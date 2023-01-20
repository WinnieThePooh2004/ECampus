using Microsoft.AspNetCore.Components;
using UniversityTimetable.FrontEnd.Requests.Interfaces;

namespace UniversityTimetable.FrontEnd.Components.PageBases;

public abstract class EditPageBase<TData> : ComponentBase
    where TData : class
{
    [Parameter] public int Id { get; set; }
    [Inject] protected IBaseRequests<TData> Requests { get; set; } = default!;
    [Inject] protected NavigationManager Navigation { get; set; } = default!;
    protected abstract string PageAfterSave { get; }
    protected TData? Model { get; private set; }
    protected override async Task OnInitializedAsync()
    {
        Model = await GetModel();
        await base.OnInitializedAsync();
    }

    private Task<TData> GetModel()
    {
        return Requests.GetByIdAsync(Id);
    }

    protected async Task Save(TData model)
    {
        await Requests.UpdateAsync(model);
        Navigation.NavigateTo(PageAfterSave);
    }
}