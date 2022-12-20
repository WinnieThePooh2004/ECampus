using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using UniversityTimetable.Shared.DataContainers;
using UniversityTimetable.Shared.Exceptions.InfrastructureExceptions;
using UniversityTimetable.Shared.Extensions;
using UniversityTimetable.Shared.Interfaces.Data;
using UniversityTimetable.Shared.Interfaces.Repositories;

namespace UniversityTimetable.Infrastructure.Repositories
{
    public class BaseRepository<TModel> : IBaseRepository<TModel>
        where TModel : class, IModel, new()
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<BaseRepository<TModel>> _logger;


        public BaseRepository(ApplicationDbContext context, ILogger<BaseRepository<TModel>> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<TModel> CreateAsync(TModel entity)
        {
            _context.Add(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task DeleteAsync(int id)
        {
            var model = new TModel { Id = id };
            _context.Remove(model);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                _logger.LogAndThrowException(new ObjectNotFoundByIdException(typeof(TModel), id));
            }
        }

        public async Task<TModel> GetByIdAsync(int id)
        {
            var model = await _context.Set<TModel>().FirstOrDefaultAsync(m => m.Id == id);
            if (model is null)
            {
                _logger.LogAndThrowException(new ObjectNotFoundByIdException(typeof(TModel), id));
            }
            return model;
        }

        public async Task<TModel> UpdateAsync(TModel entity)
        {
            _context.Update(entity);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                _logger.LogAndThrowException(new ObjectNotFoundByIdException(typeof(TModel), entity.Id));
            }
            return entity;
        }
    }
}
