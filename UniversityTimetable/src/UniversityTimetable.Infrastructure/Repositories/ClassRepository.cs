using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using UniversityTimetable.Shared.Interfaces.Repositories;
using UniversityTimetable.Shared.Models;
using UniversityTimetable.Shared.Exceptions.InfrastructureExceptions;
using UniversityTimetable.Shared.DataContainers;
using UniversityTimetable.Shared.Extensions;
using UniversityTimetable.Shared.Enums;
using static System.Runtime.InteropServices.JavaScript.JSType;

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

        public async Task<TimetableData> GetTimetableForAuditoryAsync(int auditoryId)
        {
            var auditory = await _context.Auditories.FirstOrDefaultAsync(g => g.Id == auditoryId);
            if (auditory is null)
            {
                _logger.LogAndThrowException(new ObjectNotFoundByIdException(typeof(Auditory), auditoryId));
            }
            var timetable = new TimetableData()
            {
                Auditory = auditory,
                Classes = await _context.Classes
                .Include(c => c.Teacher)
                .Include(c => c.Group)
                .Include(c => c.Subject)
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
                _logger.LogAndThrowException(new ObjectNotFoundByIdException(typeof(Shared.Models.Group), groupId));
            }
            var timetable = new TimetableData()
            {
                Group = group,
                Classes = await _context.Classes
                .IgnoreAutoIncludes()
                .Include(c => c.Teacher)
                .Include(c => c.Auditory)
                .Include(c => c.Subject)
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
                _logger.LogAndThrowException(new ObjectNotFoundByIdException(typeof(Teacher), teacherId));
            }
            var timetable = new TimetableData()
            {
                Teacher = teacher,
                Classes = await _context.Classes
                .Include(c => c.Group)
                .Include(c => c.Auditory)
                .Include(c => c.Subject)
                .Where(c => c.TeacherId == teacherId)
                .ToListAsync()
            };
            return timetable;
        }

        public async Task<Dictionary<string, string>> ValidateAsync(Class @class)
        {
            var errors = await FindReferencedValues(@class);
            if(errors.Any())
            {
                return errors;
            }
            errors.AddRange(ValidateSubject(@class));
            errors.AddRange(ValidateTeacher(@class));
            errors.AddRange(ValidateAuditory(@class));
            errors.AddRange(ValidateGroup(@class));
            return errors;
        }

        private Dictionary<string, string> ValidateTeacher(Class @class)
        {
            var errors = new Dictionary<string, string>();
            if (@class.Teacher.Classes
                .Any(c => c.Number == @class.Number &&
                c.DayOfWeek == @class.DayOfWeek &&
                c.Id != @class.Id &&
                (c.WeekDependency == WeekDependency.None ||
                @class.WeekDependency == WeekDependency.None ||
                c.WeekDependency == @class.WeekDependency)))
            {
                errors.Add(nameof(@class.AuditoryId), $"Teacher {@class.Teacher.FirstName} {@class.Teacher.LastName} " +
                    $"already has class number {@class.Number}" +
                    $" on {(DayOfWeek)@class.DayOfWeek}s " +
                    $"with week dependency {@class.WeekDependency}");
            }

            return errors;
        }
        
        private Dictionary<string, string> ValidateSubject(Class @class)
        {
            var errors = new Dictionary<string, string>();
            if (!@class.Teacher.SubjectIds.Any(s => s.SubjectId == @class.SubjectId))
            {
                errors.Add(nameof(@class.TeacherId), $"Teacher {@class.Teacher.FirstName} {@class.Teacher.LastName} " +
                    $"does not teach subject {@class.Subject.Name}");
            }
            return errors;
        }

        private Dictionary<string, string> ValidateAuditory(Class @class)
        {
            var errors = new Dictionary<string, string>();
            if (@class.Auditory.Classes
                .Any(c => c.Number == @class.Number &&
                c.DayOfWeek == @class.DayOfWeek &&
                c.Id != @class.Id &&
                (c.WeekDependency == WeekDependency.None ||
                @class.WeekDependency == WeekDependency.None ||
                c.WeekDependency == @class.WeekDependency)))
            {
                errors.Add(nameof(@class.AuditoryId), $"Auditory {@class.Auditory.Name} in building {@class.Auditory.Building} " +
                    $"already has class number {@class.Number}" +
                    $" on {(DayOfWeek)@class.DayOfWeek}s " +
                    $"with week dependency {@class.WeekDependency}");
            }
            return errors;
        }

        private async Task<Dictionary<string, string>> FindReferencedValues(Class @class)
        {
            var errors = new Dictionary<string, string>();
            @class.Group = await _context.Groups.AsNoTracking().Include(g => g.Classes).FirstOrDefaultAsync(s => s.Id == @class.GroupId);
            if (@class.Group is null)
            {
                errors.Add(nameof(@class.GroupId), "Subject does not exist");
                return errors;
            }
            @class.Auditory = await _context.Auditories.AsNoTracking()
                .Include(a => a.Classes).FirstOrDefaultAsync(s => s.Id == @class.AuditoryId);
            if (@class.Auditory is null)
            {
                errors.Add(nameof(@class.AuditoryId), "Subject does not exist");
                return errors;
            }
            @class.Subject = await _context.Subjects.AsNoTracking().FirstOrDefaultAsync(s => s.Id == @class.SubjectId);
            if (@class.Subject is null)
            {
                errors.Add(nameof(@class.SubjectId), "Subject does not exist");
                return errors;
            }
            @class.Teacher = await _context.Teachers.AsNoTracking().Include(t => t.Classes)
                .Include(t => t.SubjectIds)
                .FirstOrDefaultAsync(t => t.Id == @class.TeacherId);
            if (@class.Teacher is null)
            {
                errors.Add(nameof(@class.TeacherId), "Teacher does not exist");
                return errors;
            }

            return errors;
        }

        private Dictionary<string, string> ValidateGroup(Class @class)
        {
            var errors = new Dictionary<string, string>();
            if(@class.Group.Classes
                .Any(c => c.Number == @class.Number &&
                c.DayOfWeek == @class.DayOfWeek && 
                c.Id != @class.Id &&
                (c.WeekDependency == WeekDependency.None 
                || @class.WeekDependency == WeekDependency.None ||
                c.WeekDependency == @class.WeekDependency)))
            {
                errors.Add(nameof(@class.GroupId), $"Group {@class.Group.Name} already has class number {@class.Number}" +
                    $" on {(DayOfWeek)@class.DayOfWeek}s " +
                    $"with week dependency {@class.WeekDependency}");
            }
            return errors;
        }
    }
}
