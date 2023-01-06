using Microsoft.AspNetCore.Components;
using UniversityTimetable.FrontEnd.Requests.Interfaces;

namespace UniversityTimetable.FrontEnd.Components.PageBases
{
    public class DataTableBase<TData, TParameters> : ComponentBase
        where TData : class
        where TParameters : class, IQueryParameters, new()
    {
        [Inject] protected IParametersRequests<TData, TParameters>? DataRequests { get; set; }
        protected ListWithPaginationData<TData>? Data { get; private set; }
        protected TParameters Parameters { get; } = new();

        protected async Task RefreshData()
        {
            Data = await DataRequests?.GetByParametersAsync(Parameters)!;
            StateHasChanged();
        }

        protected override Task OnInitializedAsync()
        {
            return RefreshData();
        }

        protected virtual Task PageNumberChanged(int pageNumber)
        {
            Parameters.PageNumber = pageNumber;
            return RefreshData();
        }

        protected virtual Task PageSizeChanged(int pageSize)
        {
            Parameters.PageSize = pageSize;
            return RefreshData();
        }
    }
}