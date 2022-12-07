using UniversityTimetable.Shared.Pagination;

namespace UniversityTimetable.Shared.Interfaces
{
    public interface IService<TValue, TParams>
        where TValue : class
        where TParams : QueryParameters.QueryParameters
    {
        public Task<ListWithPaginationData<TValue>> GetByParameters(TParams parameters);
        public Task<TValue> GetByIdAsync(int? id);
        public Task<TValue> CreateAsync(TValue entity);
        public Task<TValue> UpdateAsync(TValue entity);
        public Task DeleteAsync(int? id);
    }
}
