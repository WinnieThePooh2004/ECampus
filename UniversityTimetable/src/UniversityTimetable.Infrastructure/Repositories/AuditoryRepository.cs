using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using UniversityTimetable.Shared.Interfaces;
using UniversityTimetable.Shared.Models;
using UniversityTimetable.Shared.Pagination;
using UniversityTimetable.Shared.QueryParameters;

namespace UniversityTimetable.Infrastructure.Repositories
{
    public class AuditoryRepository : IRepository<Auditory, AuditoryParameters>
    {
        private readonly ILogger _logger;
        private readonly ApplicationDbContext _context;

        public AuditoryRepository(ILogger<AuditoryRepository> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task<Auditory> CreateAsync(Auditory entity)
        {
            _context.Add(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task DeleteAsync(int id)
        {
            _context.Remove(new Auditory { Id = id });
            await _context.SaveChangesAsync();
        }

        public async Task<Auditory> GetByIdAsync(int id)
        {
            var auditory = await _context.Auditories.FirstOrDefaultAsync(a => a.Id == id);
            if (auditory is null)
            {
                throw new Exception();
            }
            return auditory;
        }

        public async Task<ListWithPaginationData<Auditory>> GetByParameters(AuditoryParameters parameters)
        {
            var query = _context.Auditories
                .Where(a => (string.IsNullOrEmpty(parameters.BuildingName) || a.Building == parameters.BuildingName) &&
                (string.IsNullOrEmpty(parameters.AuditoryName) || a.Name == parameters.AuditoryName));
            var totalCount = await query.CountAsync();
            var pagedItems = await query
                .OrderBy(a => a.Name)
                .Skip((parameters.PageNumber - 1) * parameters.PageSize)
                .Take(parameters.PageSize)
                .ToListAsync();

            return new ListWithPaginationData<Auditory>(pagedItems, totalCount, parameters.PageNumber, parameters.PageSize);
        }

        public async Task<Auditory> UpdateAsync(Auditory entity)
        {
            _context.Auditories.Update(entity);
            await _context.SaveChangesAsync();
            return entity;
        }
    }
}
