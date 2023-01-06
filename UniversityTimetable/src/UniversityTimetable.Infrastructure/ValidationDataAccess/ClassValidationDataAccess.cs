using Microsoft.EntityFrameworkCore;
using UniversityTimetable.Shared.Interfaces.Data.Validation;
using UniversityTimetable.Shared.Models;

namespace UniversityTimetable.Infrastructure.ValidationDataAccess;

public class ClassValidationDataAccess : IValidationDataAccess<Class>
{
    private readonly ApplicationDbContext _context;

    public ClassValidationDataAccess(ApplicationDbContext context)
    {
        _context = context;
    }
    
    public Task<Class> LoadRequiredDataForCreateAsync(Class model) => LoadModelRequiredData(model);

    public Task<Class> LoadRequiredDataForUpdateAsync(Class model) => LoadModelRequiredData(model);

    private async Task<Class> LoadModelRequiredData(Class model) =>
        new()
        {
            Group = await _context.Groups.AsNoTracking().Include(g => g.Classes)
                .FirstOrDefaultAsync(g => g.Id == model.GroupId),
            Auditory = await _context.Auditories.AsNoTracking()
                .Include(a => a.Classes).FirstOrDefaultAsync(s => s.Id == model.AuditoryId),
            Subject = await _context.Subjects.AsNoTracking().FirstOrDefaultAsync(s => s.Id == model.SubjectId),
            Teacher = await _context.Teachers.AsNoTracking().Include(t => t.Classes)
                .Include(t => t.SubjectIds)
                .FirstOrDefaultAsync(t => t.Id == model.TeacherId),
            TeacherId = model.TeacherId,
            SubjectId = model.SubjectId,
            AuditoryId = model.AuditoryId,
            GroupId = model.GroupId,
            WeekDependency = model.WeekDependency,
            DayOfWeek = model.DayOfWeek,
            Number = model.Number,
            ClassType = model.ClassType,
            Id = model.Id
        };

}