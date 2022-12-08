﻿using UniversityTimetable.Shared.Pagination;

namespace UniversityTimetable.Shared.Interfaces.Repositories
{
    public interface IRepository<TEntity, TParams> : IBaseRepository<TEntity>
        where TEntity : class
        where TParams : QueryParameters.QueryParameters
    {
        public Task<ListWithPaginationData<TEntity>> GetByParameters(TParams parameters);
    }
}
