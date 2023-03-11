using ECampus.DataAccess.Contracts.DataSelectParameters;
using ECampus.DataAccess.Interfaces;
using ECampus.Domain.Models;
using ECampus.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace ECampus.DataAccess.DataSelectors.MultipleItemSelectors;

public class GroupTimetableSelector : IParametersSelector<Class, GroupTimetableParameters>
{
    public IQueryable<Class> SelectData(ApplicationDbContext context, GroupTimetableParameters parameters) =>
        context.Classes
            .Include(c => c.Auditory)
            .Include(c => c.Subject)
            .Include(c => c.Teacher)
            .Where(c => c.GroupId == parameters.GroupId);
}