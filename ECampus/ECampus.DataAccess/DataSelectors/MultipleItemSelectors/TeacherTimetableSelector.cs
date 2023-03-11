using ECampus.DataAccess.Contracts.DataSelectParameters;
using ECampus.DataAccess.Interfaces;
using ECampus.Domain.Models;
using ECampus.Infrastructure;
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