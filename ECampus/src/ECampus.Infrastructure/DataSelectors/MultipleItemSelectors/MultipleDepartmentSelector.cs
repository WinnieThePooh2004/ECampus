using ECampus.Shared.Extensions;
using ECampus.Shared.Interfaces.Data.DataServices;
using ECampus.Shared.Models;
using ECampus.Shared.QueryParameters;
using Microsoft.EntityFrameworkCore;

namespace ECampus.Infrastructure.DataSelectors.MultipleItemSelectors;

public class MultipleDepartmentSelector : IMultipleItemSelector<Department, DepartmentParameters>
{
    public IQueryable<Department> SelectData(DbSet<Department> data, DepartmentParameters parameters)
    {
        var result = data.Search(d => d.Name, parameters.DepartmentName);
        if (parameters.FacultyId == 0)
        {
            return result;
        }
        return result.Where(d => d.FacultyId == parameters.FacultyId);
    }
}