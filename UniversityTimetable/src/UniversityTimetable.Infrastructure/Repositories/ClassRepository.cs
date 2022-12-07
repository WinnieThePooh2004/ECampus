using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using UniversityTimetable.Shared.Interfaces;
using UniversityTimetable.Shared.Models;
using UniversityTimetable.Shared.Pagination;
using UniversityTimetable.Shared.QueryParameters;

namespace UniversityTimetable.Infrastructure.Repositories
{
    public class ClassRepository : IRepository<Class, ClassParameters>
    {

        private readonly ILogger<ClassRepository> _logger;
        private readonly ApplicationDbContext _context;

        public ClassRepository(ILogger<ClassRepository> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task<Class> CreateAsync(Class entity)
        {
            _context.Add(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task DeleteAsync(int id)
        {
            _context.Remove(new Class { Id = id });
            await _context.SaveChangesAsync();
        }

        public async Task<Class> GetByIdAsync(int id)
        {
            var @class = await _context.Classes.FirstOrDefaultAsync(c => c.Id == id);
            if(@class is null)
            {
                throw new Exception();
            }
            return @class;
        }

        public async Task<ListWithPaginationData<Class>> GetByParameters(ClassParameters parameters)
        {
            var query = _context.Classes
                .Where(c => (string.IsNullOrEmpty(parameters.SubjectName) || c.SubjectName == parameters.SubjectName) &&
                c.ClassType == parameters.ClassType &&
                (parameters.GroupId == 0 || parameters.GroupId == c.GroupId) &&
                (parameters.AuditoryId == 0 || parameters.AuditoryId == c.AuditoryId) &&
                (parameters.TeaherId == 0 || parameters.TeaherId == c.TeacherId) &&
                (parameters.Number == 0 || parameters.Number == c.Number) &&
                (parameters.DayOfWeek == 0 || parameters.DayOfWeek == c.DayOfTheWeek));

            var totalCount = await query.CountAsync();
            var pagedItems = await query
                .OrderBy(c => c.DayOfTheWeek)
                .Skip((parameters.PageNumber - 1) * parameters.PageSize)
                .Take(parameters.PageSize)
                .ToListAsync();

            return new ListWithPaginationData<Class>(pagedItems, totalCount, parameters.PageNumber, parameters.PageSize);
        }

        public async Task<Class> UpdateAsync(Class entity)
        {
            _context.Update(entity);
            await _context.SaveChangesAsync();
            return entity;
        }
    }
}
