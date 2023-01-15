using Microsoft.EntityFrameworkCore;
using UniversityTimetable.Shared.Extensions;
using UniversityTimetable.Shared.Interfaces.Data.DataServices;
using UniversityTimetable.Shared.Models;
using UniversityTimetable.Shared.QueryParameters;

namespace UniversityTimetable.Infrastructure.DataSelectors.MultipleItemSelectors;

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