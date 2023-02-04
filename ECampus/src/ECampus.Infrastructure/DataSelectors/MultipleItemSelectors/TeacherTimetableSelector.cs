using ECampus.Contracts.DataSelectParameters;
using ECampus.Infrastructure.Interfaces;
using ECampus.Shared.Models;
using ECampus.Shared.QueryParameters;
using Microsoft.EntityFrameworkCore;

namespace ECampus.Infrastructure.DataSelectors.MultipleItemSelectors;

public class TeacherTimetableSelector : IMultipleItemSelector<Class, TeacherTimetableParameters>
{
    public IQueryable<Class> SelectData(ApplicationDbContext context, TeacherTimetableParameters parameters) =>
        context.Classes
            .Include(c => c.Group)
            .Include(c => c.Subject)
            .Include(c => c.Auditory)
            .Where(c => c.TeacherId == parameters.TeacherId);
}