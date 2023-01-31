using ECampus.Infrastructure.Interfaces;
using ECampus.Shared.Extensions;
using ECampus.Shared.Models;
using ECampus.Shared.QueryParameters;
using Microsoft.EntityFrameworkCore;

namespace ECampus.Infrastructure.DataSelectors.MultipleItemSelectors;

public class MultipleGroupSelector : IMultipleItemSelector<Group, GroupParameters>
{
    public IQueryable<Group> SelectData(DbSet<Group> data, GroupParameters parameters)
    {
        var result = data.Search(g => g.Name, parameters.Name);
        if (parameters.DepartmentId == 0)
        {
            return result;
        }
        return result.Where(g => g.DepartmentId == parameters.DepartmentId);
    }
}