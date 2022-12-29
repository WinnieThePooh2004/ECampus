using UniversityTimetable.Shared.Interfaces.Data;
using UniversityTimetable.Shared.Interfaces.Data.Models;

namespace UniversityTimetable.Shared.Interfaces.Services
{
    public interface IBaseService<TEntity> where TEntity : class, IDataTransferObject
    {
        public Task<TEntity> GetByIdAsync(int? id);
        public Task<TEntity> CreateAsync(TEntity entity);
        public Task<TEntity> UpdateAsync(TEntity entity);
        public Task DeleteAsync(int? id);
    }
}
