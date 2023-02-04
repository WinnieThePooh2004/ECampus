using ECampus.Contracts.DataSelectParameters;
using ECampus.Infrastructure.Interfaces;
using ECampus.Shared.Models;
using ECampus.Shared.QueryParameters;
using Microsoft.EntityFrameworkCore;

namespace ECampus.Infrastructure.DataSelectors.MultipleItemSelectors;

public class AuditoryTimetableSelector : IMultipleItemSelector<Class, AuditoryTimetableParameters>
{
    public IQueryable<Class> SelectData(ApplicationDbContext context, AuditoryTimetableParameters parameters) =>
        context.Classes
            .Include(c => c.Group)
            .Include(c => c.Subject)
            .Include(c => c.Teacher)
            .Where(c => c.AuditoryId == parameters.AuditoryId);
}