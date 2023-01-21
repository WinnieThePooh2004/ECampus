using ECampus.FrontEnd.Requests.Interfaces;
using ECampus.Shared.QueryParameters;
using Microsoft.AspNetCore.Components;

namespace ECampus.FrontEnd.Components.PageBases;

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