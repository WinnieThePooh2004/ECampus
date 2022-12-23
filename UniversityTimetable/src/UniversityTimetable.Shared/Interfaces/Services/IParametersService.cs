using UniversityTimetable.Shared.DataContainers;
using UniversityTimetable.Shared.Interfaces.Data;
using UniversityTimetable.Shared.QueryParameters;

namespace UniversityTimetable.Shared.Interfaces.Services
{
    public interface IParametersService<TEntity, in TParams> : IBaseService<TEntity>
        where TEntity : class, IDataTransferObject
        where TParams : IQueryParameters
    {
        public Task<ListWithPaginationData<TEntity>> GetByParametersAsync(TParams parameters);
    }
}
