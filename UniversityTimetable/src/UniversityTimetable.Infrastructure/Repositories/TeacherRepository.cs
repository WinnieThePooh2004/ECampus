using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using UniversityTimetable.Shared.Models;
using UniversityTimetable.Shared.QueryParameters;
using UniversityTimetable.Shared.General;
using UniversityTimetable.Shared.Interfaces.Repositories;
using UniversityTimetable.Shared.DataContainers;

namespace UniversityTimetable.Infrastructure.Repositories
{
    public class TeacherRepository : IRepository<Teacher, TeacherParameters>
    {
        private readonly ILogger<TeacherRepository> _logger;
        private readonly ApplicationDbContext _context;

        public TeacherRepository(ILogger<TeacherRepository> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task<Teacher> CreateAsync(Teacher entity)
        {
            _context.Add(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task DeleteAsync(int id)
        {
            _context.Remove(new Teacher { Id = id });
            await _context.SaveChangesAsync();
        }

        public async Task<Teacher> GetByIdAsync(int id)
        {
            var teacher = await _context.Teachers.FirstOrDefaultAsync(t => t.Id == id);
            if(teacher is null)
            {
                throw new Exception();
            }
            return teacher;
        }

        public async Task<ListWithPaginationData<Teacher>> GetByParameters(TeacherParameters parameters)
        {
            var query = _context.Teachers
                .Where(t => (string.IsNullOrEmpty(parameters.FirstName) || t.FirstName == parameters.FirstName) &&
                (string.IsNullOrEmpty(parameters.LastName) || t.LastName == parameters.LastName) && 
                (parameters.ScienceDegree == ScienceDegree.None || parameters.ScienceDegree == t.ScienceDegree) &&
                (parameters.DepartmentId == 0 || parameters.DepartmentId == t.DepartmentId));

            var totalCount = await query.CountAsync();

            var pagedItems = await query
                .OrderBy(c => c.FirstName + c.LastName)
                .Skip((parameters.PageNumber - 1) * parameters.PageSize)
                .Take(parameters.PageSize)
                .ToListAsync();
            return new ListWithPaginationData<Teacher>(pagedItems, totalCount, parameters.PageNumber, parameters.PageSize);

        }

        public async Task<Teacher> UpdateAsync(Teacher entity)
        {
            _context.Update(entity);
            await _context.SaveChangesAsync();
            return entity;
        }
    }
}
