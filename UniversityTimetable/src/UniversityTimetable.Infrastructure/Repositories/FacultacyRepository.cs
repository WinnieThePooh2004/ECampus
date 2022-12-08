using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using UniversityTimetable.Shared.Interfaces.Repositories;
using UniversityTimetable.Shared.Models;
using UniversityTimetable.Shared.Pagination;
using UniversityTimetable.Shared.QueryParameters;

namespace UniversityTimetable.Infrastructure.Repositories
{
    public class FacultacyRepository : IRepository<Facultacy, FacultacyParameters>
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger _logger;
        public FacultacyRepository(ApplicationDbContext context, ILogger<FacultacyRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<Facultacy> CreateAsync(Facultacy entity)
        {
            _context.Add(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task DeleteAsync(int id)
        {
            _context.Remove(new Facultacy() { Id = id });
            await _context.SaveChangesAsync();
        }

        public async Task<Facultacy> GetByIdAsync(int id)
        {
            var facultacy = await _context.Facultacies.FirstOrDefaultAsync(f => f.Id == id);
            if (facultacy == null)
            {
                throw new Exception();
            }
            return facultacy;
        }

        public async Task<ListWithPaginationData<Facultacy>> GetByParameters(FacultacyParameters parameters)
        {
            var entities = _context.Facultacies
                .Where(f => string.IsNullOrEmpty(parameters.FacultacyName) || f.Name == parameters.FacultacyName);
            int totalCount = await entities.CountAsync();

            var pagedItems = await entities
                .OrderBy(a => a.Name)
                .Skip((parameters.PageNumber - 1) * parameters.PageSize)
                .Take(parameters.PageSize)
                .ToListAsync();

            return new ListWithPaginationData<Facultacy>(pagedItems, totalCount, parameters.PageNumber, parameters.PageSize);
        }

        public async Task<Facultacy> UpdateAsync(Facultacy entity)
        {
            _context.Update(entity);
            await _context.SaveChangesAsync();
            return entity;
        }
    }
}
