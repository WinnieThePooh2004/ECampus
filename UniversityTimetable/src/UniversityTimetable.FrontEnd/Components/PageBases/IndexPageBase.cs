namespace UniversityTimetable.FrontEnd.Components.PageBases
{
    public abstract class IndexPageBase<TData, TParameters> : DataTableBase<TData, TParameters>
        where TData : class
        where TParameters : IQueryParameters, new()
    {
        protected virtual async Task Delete(int Id)
        {
            await DataRequests.DeleteAsync(Id);
            await RefreshData();
        }
    }
}
