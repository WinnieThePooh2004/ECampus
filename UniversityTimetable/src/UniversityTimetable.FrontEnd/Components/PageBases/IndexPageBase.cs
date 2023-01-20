using Microsoft.AspNetCore.Components;
using UniversityTimetable.FrontEnd.Requests.Interfaces;

namespace UniversityTimetable.FrontEnd.Components.PageBases;

public abstract class IndexPageBase<TData, TParameters> : DataTableBase<TData, TParameters>
    where TData : class
    where TParameters : class, IQueryParameters, new()
{
    [Inject] private IBaseRequests<TData> DeleteRequests { get; set; } = default!;
    protected async Task Delete(int id)
    {
        await DeleteRequests.DeleteAsync(id);
        await RefreshData();
    }
}