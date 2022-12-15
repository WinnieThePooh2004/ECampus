using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using UniversityTimetable.Shared.Exceptions.InfrastructureExceptions;
using UniversityTimetable.Shared.Extentions;
using UniversityTimetable.Shared.Interfaces.Repositories;
using UniversityTimetable.Shared.Models;

namespace UniversityTimetable.Infrastructure.Repositories
{
    public class BaseSubjectRepository : IBaseRepository<Subject>
    {
        private readonly ILogger<BaseSubjectRepository> _logger;
        private readonly ApplicationDbContext _context;

        public BaseSubjectRepository(ILogger<BaseSubjectRepository> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task<Subject> CreateAsync(Subject entity)
        {
            entity.TeacherIds = entity.Teachers.Select(t => new SubjectTeacher { TeacherId = t.Id }).ToList();
            entity.Teachers = null;
            _context.Add(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task DeleteAsync(int id)
        {
            _context.Remove(new Subject { Id = id });
            _context.RemoveRange(await _context.SubjectTeachers.Where(st => st.SubjectId == id).ToListAsync());
            try
            {
                await _context.SaveChangesAsync();
            }
            catch(DbUpdateConcurrencyException)
            {
                _logger.LogAndThrowException(new ObjectNotFoundByIdException(typeof(Subject), id));
            }
        }

        public async Task<Subject> GetByIdAsync(int id)
        {
            var subject = await _context.Subjects
                .Include(t => t.TeacherIds)
                .ThenInclude(st => st.Teacher)
                .FirstOrDefaultAsync(t => t.Id == id);
            if (subject is null)
            {
                _logger.LogAndThrowException(new ObjectNotFoundByIdException(typeof(Subject), id));
            }
            subject.Teachers = subject.TeacherIds.Select(t => t.Teacher).ToList();
            return subject;
        }

        public async Task<Subject> UpdateAsync(Subject entity)
        {
            var allTeachers = await _context.SubjectTeachers
                .Where(st => st.SubjectId == entity.Id)
                .ToListAsync();

            _context.RemoveRange(allTeachers.Where(st => !entity.Teachers.Any(t => t.Id == st.TeacherId)));
            _context.AddRange(entity.Teachers
                .Where(s => !allTeachers.Any(st => s.Id == st.TeacherId))
                .Select(s => new SubjectTeacher { SubjectId = entity.Id, TeacherId = s.Id }));

            entity.Teachers?.Clear();
            entity.TeacherIds?.Clear();
            _context.Update(entity);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                _logger.LogAndThrowException(new ObjectNotFoundByIdException(typeof(Subject), entity.Id));
            }
            return entity;
        }
    }
}
