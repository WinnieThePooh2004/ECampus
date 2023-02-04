using ECampus.Contracts.DataSelectParameters;
using ECampus.Infrastructure.Interfaces;
using ECampus.Shared.Models;
using ECampus.Shared.QueryParameters;
using Microsoft.EntityFrameworkCore;

namespace ECampus.Infrastructure.DataSelectors.MultipleItemSelectors;

public class GroupTimetableSelector : IMultipleItemSelector<Class, GroupTimetableParameters>
{
    public IQueryable<Class> SelectData(ApplicationDbContext context, GroupTimetableParameters parameters) =>
        context.Classes
            .Include(c => c.Auditory)
            .Include(c => c.Subject)
            .Include(c => c.Teacher)
            .Where(c => c.GroupId == parameters.GroupId);
}