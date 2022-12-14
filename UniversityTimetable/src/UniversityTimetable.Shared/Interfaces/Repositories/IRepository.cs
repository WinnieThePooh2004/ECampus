using UniversityTimetable.Shared.DataContainers;

namespace UniversityTimetable.Shared.Interfaces.Repositories
{
    public interface IRepository<TEntity, TParams> : IBaseRepository<TEntity>
        where TEntity : class
        where TParams : QueryParameters.QueryParameters<TEntity>
    {
        public Task<ListWithPaginationData<TEntity>> GetByParameters(TParams parameters);
    }
}
