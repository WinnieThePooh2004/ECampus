using UniversityTimetable.Shared.Interfaces.Data;
using UniversityTimetable.Shared.Interfaces.Data.Models;

namespace UniversityTimetable.Shared.Interfaces.DataAccess;

public interface IBaseDataAccessFacade<TEntity> where TEntity : class, IModel
{
    public Task<TEntity> GetByIdAsync(int id);
    public Task<TEntity> CreateAsync(TEntity entity);
    public Task<TEntity> UpdateAsync(TEntity entity);
    public Task DeleteAsync(int id);
}