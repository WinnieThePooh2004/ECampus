using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using UniversityTimetable.Shared.DataContainers;
using UniversityTimetable.Shared.Extentions;
using UniversityTimetable.Shared.Interfaces.Repositories;
using UniversityTimetable.Shared.Models;
using UniversityTimetable.Shared.QueryParameters;

namespace UniversityTimetable.Infrastructure.Repositories
{
    public class FacultyRepository : IRepository<Faculty, FacultyParameters>
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger _logger;
        public FacultyRepository(ApplicationDbContext context, ILogger<FacultyRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<Faculty> CreateAsync(Faculty entity)
        {
            _context.Add(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task DeleteAsync(int id)
        {
            _context.Remove(new Faculty() { Id = id });
            await _context.SaveChangesAsync();
        }

        public async Task<Faculty> GetByIdAsync(int id)
        {
            var facultacy = await _context.Facultacies.FirstOrDefaultAsync(f => f.Id == id);
            if (facultacy == null)
            {
                throw new Exception();
            }
            return facultacy;
        }

        public async Task<ListWithPaginationData<Faculty>> GetByParameters(FacultyParameters parameters)
        {
            var entities = _context.Facultacies.Filter(parameters);

            int totalCount = await entities.CountAsync();

            var pagedItems = await entities
                .OrderBy(a => a.Name)
                .Skip((parameters.PageNumber - 1) * parameters.PageSize)
                .Take(parameters.PageSize)
                .ToListAsync();

            return new ListWithPaginationData<Faculty>(pagedItems, totalCount, parameters.PageNumber, parameters.PageSize);
        }

        public async Task<Faculty> UpdateAsync(Faculty entity)
        {
            _context.Update(entity);
            await _context.SaveChangesAsync();
            return entity;
        }
    }
}
