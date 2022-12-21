﻿using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using UniversityTimetable.Shared.Extensions;
using UniversityTimetable.Shared.Interfaces.Data;
using UniversityTimetable.Shared.QueryParameters;
using UniversityTimetable.Shared.Exceptions.InfrastructureExceptions;
using UniversityTimetable.Shared.Interfaces.Repositories;
using UniversityTimetable.Shared.DataContainers;

namespace UniversityTimetable.Infrastructure.Repositories
{
    public class Repository<TModel, TParameters> : IRepository<TModel, TParameters>
        where TModel : class, IModel, new()
        where TParameters : IQueryParameters<TModel>
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<Repository<TModel, TParameters>> _logger;
        private readonly IBaseRepository<TModel> _baseRepository;
        private readonly IDataSelector<TModel, TParameters> _dataSelector;
        public Repository(ApplicationDbContext context, ILogger<Repository<TModel, TParameters>> logger,
            IBaseRepository<TModel> baseRepository, IDataSelector<TModel, TParameters> dataSelector)
        {
            _context = context;
            _logger = logger;
            _baseRepository = baseRepository;
            _dataSelector = dataSelector;
        }

        public Task<TModel> CreateAsync(TModel entity)
            => _baseRepository.CreateAsync(entity);

        public Task DeleteAsync(int id)
            => _baseRepository.DeleteAsync(id);

        public Task<TModel> GetByIdAsync(int id)
            => _baseRepository.GetByIdAsync(id);

        public async Task<ListWithPaginationData<TModel>> GetByParameters(TParameters parameters)
        {
            var query = _dataSelector.SelectData(_context.Set<TModel>(), parameters);
            var totalCount = await query.CountAsync();
            var pagedItems = await query
                .OrderBy(a => a.Id)
                .Skip((parameters.PageNumber - 1) * parameters.PageSize)
                .Take(parameters.PageSize)
                .ToListAsync();

            return new ListWithPaginationData<TModel>(pagedItems, totalCount, parameters.PageNumber, parameters.PageSize);
        }

        public Task<TModel> UpdateAsync(TModel entity)
            => _baseRepository.UpdateAsync(entity);
    }
}
