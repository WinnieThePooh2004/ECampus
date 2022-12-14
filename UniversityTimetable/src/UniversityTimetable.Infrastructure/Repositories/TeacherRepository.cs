using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using UniversityTimetable.Shared.Models;
using UniversityTimetable.Shared.QueryParameters;
using UniversityTimetable.Shared.General;
using UniversityTimetable.Shared.Interfaces.Repositories;
using UniversityTimetable.Shared.DataContainers;
using UniversityTimetable.Shared.Extentions;

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
            entity.SubjectIds = entity.Subjects.Select(s => new SubjectTeacher { SubjectId = s.Id }).ToList();
            entity.Subjects = null;
            _context.Add(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task DeleteAsync(int id)
        {
            _context.Remove(new Teacher { Id = id });
            _context.RemoveRange(await _context.SubjectTeachers.Where(st => st.TeacherId == id).ToListAsync());
            await _context.SaveChangesAsync();
        }

        public async Task<Teacher> GetByIdAsync(int id)
        {
            var teacher = await _context
                .Teachers
                .Include(t => t.SubjectIds)
                .ThenInclude(st => st.Subject)
                .FirstOrDefaultAsync(t => t.Id == id);
            if(teacher is null)
            {
                throw new Exception();
            }
            teacher.Subjects = teacher.SubjectIds.Select(t => t.Subject).ToList();
            return teacher;
        }

        public async Task<ListWithPaginationData<Teacher>> GetByParameters(TeacherParameters parameters)
        {
            var query = _context.Teachers.Filter(parameters);

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
            var teacherSubjects = await _context.SubjectTeachers.Where(st => st.TeacherId == entity.Id).ToListAsync();
            entity.SubjectIds = entity.Subjects.Select(s => new SubjectTeacher { SubjectId = s.Id }).ToList();
            entity.Subjects.Clear();
            _context.RemoveRange(teacherSubjects.Where(ts => !entity.Subjects.Any(s => s.Id == ts.SubjectId)));
            _context.Update(entity);
            await _context.SaveChangesAsync();
            return entity;
        }
    }
}
