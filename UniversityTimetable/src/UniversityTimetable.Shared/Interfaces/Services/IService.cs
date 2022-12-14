using UniversityTimetable.Shared.DataContainers;
using UniversityTimetable.Shared.Interfaces.Repositories;
using UniversityTimetable.Shared.QueryParameters;

namespace UniversityTimetable.Shared.Interfaces.Services
{
    public interface IService<TEntity, TParams> : IBaseService<TEntity>
        where TEntity : class
        where TParams : IQueryParameters
    {
        public Task<ListWithPaginationData<TEntity>> GetByParametersAsync(TParams parameters);
    }
}
