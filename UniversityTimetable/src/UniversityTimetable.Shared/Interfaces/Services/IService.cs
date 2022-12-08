using UniversityTimetable.Shared.DataContainers;
using UniversityTimetable.Shared.Interfaces.Repositories;

namespace UniversityTimetable.Shared.Interfaces.Services
{
    public interface IService<TEntity, TParams> : IBaseService<TEntity>
        where TEntity : class
        where TParams : QueryParameters.QueryParameters
    {
        public Task<ListWithPaginationData<TEntity>> GetByParametersAsync(TParams parameters);
    }
}
