using Microsoft.EntityFrameworkCore;
using UniversityTimetable.Shared.Extensions;
using UniversityTimetable.Shared.Interfaces.Data.DataServices;
using UniversityTimetable.Shared.Models;
using UniversityTimetable.Shared.QueryParameters;

namespace UniversityTimetable.Infrastructure.DataSelectors.MultipleItemSelectors;

public class MultipleUserSelector : IMultipleItemSelector<User, UserParameters>
{
    public IQueryable<User> SelectData(DbSet<User> data, UserParameters parameters) =>
        data.Search(u => u.Email, parameters.Email)
            .Search(u => u.Username, parameters.Username)
            .Where(u => u.Role >= parameters.Role)
            .Sort(parameters.OrderBy, parameters.SortOrder);
}