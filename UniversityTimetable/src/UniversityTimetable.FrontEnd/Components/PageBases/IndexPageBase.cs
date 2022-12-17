using Microsoft.AspNetCore.Components;
using UniversityTimetable.FrontEnd.Requests.Interfaces;

namespace UniversityTimetable.FrontEnd.Components.PageBases
{
    public abstract class IndexPageBase<TData, TParameters> : DataTableBase<TData, TParameters>
        where TData : class
        where TParameters : IQueryParameters, new()
    {
        [Inject] private IBaseRequests<TData> DeleteRequests { get; set; }
        protected virtual async Task Delete(int Id)
        {
            await DeleteRequests.DeleteAsync(Id);
            await RefreshData();
        }
    }
}
