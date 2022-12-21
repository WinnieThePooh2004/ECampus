namespace UniversityTimetable.FrontEnd.Requests.Interfaces
{
    public interface IParametersRequests<TEntity, TParams> 
        where TParams : IQueryParameters
    {
        public Task<ListWithPaginationData<TEntity>> GetByParametersAsync(TParams parameters);
    }
}
