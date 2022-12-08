using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using UniversityTimetable.Shared.Interfaces.Repositories;
using UniversityTimetable.Shared.Models;
using UniversityTimetable.Shared.Exceptions.InfrastructureExceptions;
using UniversityTimetable.Shared.DataContainers;
using System.Text.RegularExpressions;

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

        public async Task<TimetableData> GetTimetableForAuditoryAsync(int auditoryId)
        {
            var auditory = await _context.Auditories.FirstOrDefaultAsync(g => g.Id == auditoryId);
            if (auditory is null)
            {
                throw new ObjectNotFoundByIdException(typeof(Auditory), auditoryId);
            }
            var timetable = new TimetableData()
            {
                Auditory = auditory,
                Classes = await _context.Classes
                .IgnoreAutoIncludes()
                .Include(c => c.Teacher)
                .Include(c => c.Auditory)
                .Where(c => c.AuditoryId == auditoryId)
                .ToListAsync()
            };
            return timetable;
        }

        public async Task<TimetableData> GetTimetableForGroupAsync(int groupId)
        {
            var group = await _context.Groups.FirstOrDefaultAsync(g => g.Id == groupId);
            if (group is null)
            {
                throw new ObjectNotFoundByIdException(typeof(Shared.Models.Group), groupId);
            }
            var timetable = new TimetableData()
            {
                Group = group,
                Classes = await _context.Classes
                .IgnoreAutoIncludes()
                .Include(c => c.Teacher)
                .Include(c => c.Auditory)
                .Where(c => c.GroupId == groupId)
                .ToListAsync()
            };
            return timetable;
        }

        public async Task<TimetableData> GetTimetableForTeacherAsync(int teacherId)
        {
            var teacher = await _context.Teachers.FirstOrDefaultAsync(t => t.Id == teacherId);
            if (teacher is null)
            {
                throw new ObjectNotFoundByIdException(typeof(Teacher), teacherId);
            }
            var timetable = new TimetableData()
            {
                Teacher = teacher,
                Classes = await _context.Classes
                .Include(c => c.Group)
                .Include(c => c.Auditory)
                .Where(c => c.TeacherId == teacherId)
                .ToListAsync()
            };
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
