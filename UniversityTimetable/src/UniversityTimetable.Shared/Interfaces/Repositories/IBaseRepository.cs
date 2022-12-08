namespace UniversityTimetable.Shared.Interfaces.Repositories
{
    public interface IBaseRepository<TEntity> where TEntity : class
    {
        public Task<TEntity> GetByIdAsync(int id);
        public Task<TEntity> CreateAsync(TEntity entity);
        public Task<TEntity> UpdateAsync(TEntity entity);
        public Task DeleteAsync(int id);
    }
}
