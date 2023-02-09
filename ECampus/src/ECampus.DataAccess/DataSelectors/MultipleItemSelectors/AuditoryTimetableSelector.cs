using ECampus.Contracts.DataSelectParameters;
using ECampus.DataAccess.Interfaces;
using ECampus.Infrastructure;
using ECampus.Shared.Models;
using Microsoft.EntityFrameworkCore;

namespace ECampus.DataAccess.DataSelectors.MultipleItemSelectors;

public class AuditoryTimetableSelector : IMultipleItemSelector<Class, AuditoryTimetableParameters>
{
    public IQueryable<Class> SelectData(ApplicationDbContext context, AuditoryTimetableParameters parameters) =>
        context.Classes
            .Include(c => c.Group)
            .Include(c => c.Subject)
            .Include(c => c.Teacher)
            .Where(c => c.AuditoryId == parameters.AuditoryId);
}