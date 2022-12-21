using Microsoft.EntityFrameworkCore;
using UniversityTimetable.Shared.Extensions;
using UniversityTimetable.Shared.Interfaces.Data;
using UniversityTimetable.Shared.Models;
using UniversityTimetable.Shared.QueryParameters;

namespace UniversityTimetable.Infrastructure.DataSelectors
{
    public class GroupSelector : IDataSelector<Group, GroupParameters>
    {
        public IQueryable<Group> SelectData(DbSet<Group> data, GroupParameters parameters)
            => data.Search(g => g.Name, parameters.SearchTerm)
            .Where(g => parameters.DepartmentId == 0 || g.DepartmentId == parameters.DepartmentId);
    }
}
