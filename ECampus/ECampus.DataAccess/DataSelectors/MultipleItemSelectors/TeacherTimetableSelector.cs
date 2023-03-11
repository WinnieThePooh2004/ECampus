using ECampus.Contracts.DataSelectParameters;
using ECampus.DataAccess.Interfaces;
using ECampus.Infrastructure;
using ECampus.Shared.Models;
using Microsoft.EntityFrameworkCore;

namespace ECampus.DataAccess.DataSelectors.MultipleItemSelectors;

public class TeacherTimetableSelector : IParametersSelector<Class, TeacherTimetableParameters>
{
    public IQueryable<Class> SelectData(ApplicationDbContext context, TeacherTimetableParameters parameters) =>
        context.Classes
            .Include(c => c.Group)
            .Include(c => c.Subject)
            .Include(c => c.Auditory)
            .Where(c => c.TeacherId == parameters.TeacherId);
}