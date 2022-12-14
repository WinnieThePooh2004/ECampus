using UniversityTimetable.Shared.DataContainers;
using UniversityTimetable.Shared.QueryParameters;

namespace UniversityTimetable.Shared.Interfaces.Repositories
{
    public interface IRepository<TEntity, TParams> : IBaseRepository<TEntity>
        where TEntity : class
        where TParams : IQueryParameters<TEntity>
    {
        public Task<ListWithPaginationData<TEntity>> GetByParameters(TParams parameters);
    }
}
