using ECampus.FrontEnd.Requests.Interfaces;
using ECampus.Shared.Data;
using ECampus.Shared.QueryParameters;
using Microsoft.AspNetCore.Components;

namespace ECampus.FrontEnd.Components.PageBases;

public abstract class IndexPageBase<TData, TParameters> : DataTableBase<TData, TParameters>
    where TData : class, IDataTransferObject
    where TParameters : class, IQueryParameters<TData>, new()
{
    [Inject] private IBaseRequests<TData> DeleteRequests { get; set; } = default!;
    protected async Task Delete(int id)
    {
        await DeleteRequests.DeleteAsync(id);
        await RefreshData();
    }
}