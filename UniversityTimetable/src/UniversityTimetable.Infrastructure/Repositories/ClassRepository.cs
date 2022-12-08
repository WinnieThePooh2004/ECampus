using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using UniversityTimetable.Shared.Interfaces.Repositories;
using UniversityTimetable.Shared.Models;
using UniversityTimetable.Shared.Pagination;
using UniversityTimetable.Shared.QueryParameters.TimetableParameters;
using UniversityTimetable.Shared.General;
using UniversityTimetable.Shared.Exceptions.InfrastructureExceptions;

namespace UniversityTimetable.Infrastructure.Repositories
{
    public class ClassRepository : IClassRepository
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
            var @class = await _context
                .Classes
                .Include(c => c.Teacher)
                .Include(c => c.Group)
                .Include(c => c.Auditory)
                .FirstOrDefaultAsync(c => c.Id == id);
            if (@class is null)
            {
                throw new Exception();
            }
            return @class;
        }

        public async Task<Timetable<Class>> GetTimetableForAuditoryAsync(AuditoryTimetableParameters parameters)
        {
            var auditory = await _context.Groups.FirstOrDefaultAsync(g => g.Id == parameters.AuditoryId);
            if (auditory is null)
            {
                throw new ObjectNotFoundByIdException(typeof(Auditory), parameters.AuditoryId);
            }
            var timetable = new Timetable<Class>();
            timetable.AuditoryId = auditory.Id;

            var allauditoryClasses = await _context.Classes
                .Include(c => c.Group)
                .Include(c => c.Teacher)
                .Where(c => c.AuditoryId == parameters.AuditoryId)
                .ToListAsync();

            foreach (var @class in allauditoryClasses)
            {
                timetable.Add(@class);
            }
            return timetable;
        }

        public async Task<Timetable<Class>> GetTimetableForGroupAsync(GroupTimetableParameters parameters)
        {
            var group = await _context.Groups.FirstOrDefaultAsync(g => g.Id == parameters.GroupId);
            if (group is null)
            {
                throw new ObjectNotFoundByIdException(typeof(Group), parameters.GroupId);
            }
            var timetable = new Timetable<Class>();
            timetable.GroupId = group.Id;

            var allGroupClasses = await _context.Classes
                .Include(c => c.Teacher)
                .Include(c => c.Auditory)
                .Where(c => c.GroupId == parameters.GroupId)
                .ToListAsync();

            foreach (var @class in allGroupClasses)
            {
                timetable.Add(@class);
            }
            return timetable;
        }

        public async Task<Timetable<Class>> GetTimetableForTeacherAsync(TeacherTimetableParameters parameters)
        {
            var teacher = await _context.Teachers.FirstOrDefaultAsync(t => t.Id == parameters.TeacherId);
            if (teacher is null)
            {
                throw new ObjectNotFoundByIdException(typeof(Teacher), parameters.TeacherId);
            }
            var timetable = new Timetable<Class>();
            timetable.TeacherId = teacher.Id;

            var allTeacherClasses = await _context.Classes
                .Include(c => c.Group)
                .Include(c => c.Auditory)
                .Where(c => c.TeacherId == parameters.TeacherId)
                .ToListAsync();

            foreach (var @class in allTeacherClasses)
            {
                timetable.Add(@class);
            }
            return timetable;
        }

        public async Task<Class> UpdateAsync(Class entity)
        {
            _context.Update(entity);
            await _context.SaveChangesAsync();
            return entity;
        }
    }
}
