namespace UniversityTimetable.Shared.Interfaces.Services
{
    public interface IBaseService<TEntity> where TEntity : class
    {
        public Task<TEntity> GetByIdAsync(int? id);
        public Task<TEntity> CreateAsync(TEntity entity);
        public Task<TEntity> UpdateAsync(TEntity entity);
        public Task DeleteAsync(int? id);
    }
}
