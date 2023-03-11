using ECampus.DataAccess.Interfaces;
using ECampus.Domain.Extensions;
using ECampus.Domain.Models;
using ECampus.Domain.QueryParameters;
using ECampus.Infrastructure;

namespace ECampus.DataAccess.DataSelectors.MultipleItemSelectors;

public class MultipleDepartmentSelector : IParametersSelector<Department, DepartmentParameters>
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