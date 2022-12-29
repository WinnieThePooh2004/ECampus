using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using UniversityTimetable.Shared.Interfaces.DataAccess;
using UniversityTimetable.Shared.Models;
using UniversityTimetable.Shared.Exceptions.InfrastructureExceptions;
using UniversityTimetable.Shared.DataContainers;
using UniversityTimetable.Shared.Extensions;
using UniversityTimetable.Shared.Enums;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace UniversityTimetable.Infrastructure.DataAccessFacades;

public class TimetableDataAccessFacade : ITimetableDataAccessFacade
{

    private readonly ILogger<TimetableDataAccessFacade> _logger;
    private readonly ApplicationDbContext _context;

    public TimetableDataAccessFacade(ILogger<TimetableDataAccessFacade> logger, ApplicationDbContext context)
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
}