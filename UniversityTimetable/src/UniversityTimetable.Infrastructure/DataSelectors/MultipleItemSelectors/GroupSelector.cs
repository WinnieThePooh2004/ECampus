using Microsoft.EntityFrameworkCore;
using UniversityTimetable.Shared.Extensions;
using UniversityTimetable.Shared.Interfaces.Data;
using UniversityTimetable.Shared.Interfaces.Data.DataServices;
using UniversityTimetable.Shared.Models;
using UniversityTimetable.Shared.QueryParameters;

namespace UniversityTimetable.Infrastructure.DataSelectors.MultipleItemSelectors
{
    public class GroupSelector : IMultipleItemSelector<Group, GroupParameters>
    {
        public IQueryable<Group> SelectData(DbSet<Group> data, GroupParameters parameters)
            => data.Search(g => g.Name, parameters.SearchTerm)
            .Where(g => parameters.DepartmentId == 0 || g.DepartmentId == parameters.DepartmentId);
    }
}
