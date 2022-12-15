namespace UniversityTimetable.FrontEnd.Requests.Interfaces
{
    public interface IRequests<TEntity, TParams> : IBaseRequests<TEntity> where TParams : IQueryParameters
    {
        public Task<ListWithPaginationData<TEntity>> GetByParametersAsync(TParams parameters);
    }
}
