using UniversityTimetable.Shared.DataContainers;
using UniversityTimetable.Shared.Interfaces.Data.Models;
using UniversityTimetable.Shared.QueryParameters;

namespace UniversityTimetable.Shared.Interfaces.DataAccess;

public interface IParametersDataAccessFacade<TEntity, in TParams>
    where TEntity : class, IModel
    where TParams : IQueryParameters<TEntity>
{
    public Task<ListWithPaginationData<TEntity>> GetByParameters(TParams parameters);
}