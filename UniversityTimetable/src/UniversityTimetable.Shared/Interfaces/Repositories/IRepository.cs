using UniversityTimetable.Shared.DataContainers;
using UniversityTimetable.Shared.Interfaces.Data;
using UniversityTimetable.Shared.QueryParameters;

namespace UniversityTimetable.Shared.Interfaces.Repositories
{
    public interface IRepository<TEntity, TParams> : IBaseRepository<TEntity>
        where TEntity : class, IModel
        where TParams : IQueryParameters<TEntity>
    {
        public Task<ListWithPaginationData<TEntity>> GetByParameters(TParams parameters);
    }
}
