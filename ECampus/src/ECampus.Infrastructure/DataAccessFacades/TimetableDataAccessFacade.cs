using ECampus.Core.Metadata;
using ECampus.Shared.DataContainers;
using ECampus.Shared.Exceptions.InfrastructureExceptions;
using ECampus.Shared.Interfaces.DataAccess;
using ECampus.Shared.Metadata;
using ECampus.Shared.Models;
using Microsoft.EntityFrameworkCore;

namespace ECampus.Infrastructure.DataAccessFacades;

[Inject(typeof(ITimetableDataAccessFacade))]
public class TimetableDataAccessFacade : ITimetableDataAccessFacade
{
    private readonly ApplicationDbContext _context;

    public TimetableDataAccessFacade(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<TimetableData> GetTimetableForAuditoryAsync(int auditoryId)
    {
        var auditory = await _context.Auditories.FirstOrDefaultAsync(g => g.Id == auditoryId);
        if (auditory is null)
        {
            throw new ObjectNotFoundByIdException(typeof(Auditory), auditoryId);
        }
        var timetable = new TimetableData
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
            throw new ObjectNotFoundByIdException(typeof(Group), groupId);
        }
        var timetable = new TimetableData
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
            throw new ObjectNotFoundByIdException(typeof(Teacher), teacherId);
        }
        var timetable = new TimetableData
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