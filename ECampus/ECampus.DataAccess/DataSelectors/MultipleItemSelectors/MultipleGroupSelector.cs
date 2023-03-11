using ECampus.DataAccess.Interfaces;
using ECampus.Domain.Entities;
using ECampus.Domain.Extensions;
using ECampus.Domain.QueryParameters;
using ECampus.Infrastructure;

namespace ECampus.DataAccess.DataSelectors.MultipleItemSelectors;

public class MultipleGroupSelector : IParametersSelector<Group, GroupParameters>
{
    public IQueryable<Group> SelectData(ApplicationDbContext context, GroupParameters parameters)
    {
        var result = context.Groups.Search(g => g.Name, parameters.Name);
        return parameters.DepartmentId == 0 ? result : result.Where(g => g.DepartmentId == parameters.DepartmentId);
    }
}