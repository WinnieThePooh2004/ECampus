using Microsoft.EntityFrameworkCore;
using UniversityTimetable.Shared.Interfaces.Data;
using UniversityTimetable.Shared.Models;

namespace UniversityTimetable.Infrastructure.ValidationRepositories;

public class ClassValidationRepository : IValidationRepository<Class>
{
    private readonly ApplicationDbContext _context;

    public ClassValidationRepository(ApplicationDbContext context)
    {
        _context = context;
    }
    
    public Task<Class> LoadRequiredDataForCreate(Class model) => LoadModelRequiredData(model);

    public Task<Class> LoadRequiredDataForUpdate(Class model) => LoadModelRequiredData(model);

    private async Task<Class> LoadModelRequiredData(Class @model) =>
        new()
        {
            Group = await _context.Groups.AsNoTracking().Include(g => g.Classes)
                .FirstOrDefaultAsync(s => s.Id == model.GroupId),
            Auditory = await _context.Auditories.AsNoTracking()
                .Include(a => a.Classes).FirstOrDefaultAsync(s => s.Id == model.AuditoryId),
            Subject = await _context.Subjects.AsNoTracking().FirstOrDefaultAsync(s => s.Id == model.SubjectId),
            Teacher = await _context.Teachers.AsNoTracking().Include(t => t.Classes)
                .Include(t => t.SubjectIds)
                .FirstOrDefaultAsync(t => t.Id == model.TeacherId)
        };

}