using ECampus.DataAccess.Interfaces;
using ECampus.Infrastructure;
using ECampus.Shared.Extensions;
using ECampus.Shared.Models;
using ECampus.Shared.QueryParameters;

namespace ECampus.DataAccess.DataSelectors.MultipleItemSelectors;

public class MultipleGroupSelector : IMultipleItemSelector<Group, GroupParameters>
{
    public IQueryable<Group> SelectData(ApplicationDbContext context, GroupParameters parameters)
    {
        var result = context.Groups.Search(g => g.Name, parameters.Name);
        return parameters.DepartmentId == 0 ? result : result.Where(g => g.DepartmentId == parameters.DepartmentId);
    }
}