using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using UniversityTimetable.Shared.Models;
using UniversityTimetable.Shared.Interfaces.Repositories;
using UniversityTimetable.Shared.Extentions;
using UniversityTimetable.Shared.Exceptions.InfrastructureExceptions;

namespace UniversityTimetable.Infrastructure.Repositories
{
    public class BaseTeacherRepository : IBaseRepository<Teacher>
    {
        private readonly ILogger<BaseTeacherRepository> _logger;
        private readonly ApplicationDbContext _context;

        public BaseTeacherRepository(ILogger<BaseTeacherRepository> logger, ApplicationDbContext context)
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
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                _logger.LogAndThrowException(new ObjectNotFoundByIdException(typeof(Teacher), id));
            }
        }

        public async Task<Teacher> GetByIdAsync(int id)
        {
            var teacher = await _context
                .Teachers
                .Include(t => t.SubjectIds)
                .ThenInclude(st => st.Subject)
                .FirstOrDefaultAsync(t => t.Id == id);
            if (teacher is null)
            {
                _logger.LogAndThrowException(new ObjectNotFoundByIdException(typeof(Teacher), id));
            }
            teacher.Subjects = teacher.SubjectIds.Select(t => t.Subject).ToList();
            return teacher;
        }

        public async Task<Teacher> UpdateAsync(Teacher entity)
        {
            var allSubjects = await _context.SubjectTeachers
                .Where(st => st.TeacherId == entity.Id)
                .ToListAsync();

            _context.RemoveRange(allSubjects.Where(st => !entity.Subjects.Any(s => s.Id == st.SubjectId)));
            _context.AddRange(entity.Subjects
                .Where(s => !allSubjects.Any(st => s.Id == st.SubjectId))
                .Select(s => new SubjectTeacher { TeacherId = entity.Id, SubjectId = s.Id }));

            entity.Subjects?.Clear();
            entity.SubjectIds?.Clear();
            _context.Update(entity);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                _logger.LogAndThrowException(new ObjectNotFoundByIdException(typeof(Teacher), entity.Id));
            }
            return entity;
        }
    }
}
