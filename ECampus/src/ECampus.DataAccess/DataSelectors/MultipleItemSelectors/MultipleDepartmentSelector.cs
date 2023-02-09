using ECampus.DataAccess.Interfaces;
using ECampus.Infrastructure;
using ECampus.Shared.Extensions;
using ECampus.Shared.Models;
using ECampus.Shared.QueryParameters;

namespace ECampus.DataAccess.DataSelectors.MultipleItemSelectors;

public class MultipleDepartmentSelector : IMultipleItemSelector<Department, DepartmentParameters>
{
    public IQueryable<Department> SelectData(ApplicationDbContext context, DepartmentParameters parameters)
    {
        var result = context.Departments.Search(d => d.Name, parameters.DepartmentName);
        if (parameters.FacultyId == 0)
        {
            return result;
        }
        return result.Where(d => d.FacultyId == parameters.FacultyId);
    }
}