using Microsoft.EntityFrameworkCore;
using UniversityTimetable.Shared.Extensions;
using UniversityTimetable.Shared.Interfaces.Data;
using UniversityTimetable.Shared.Interfaces.Data.DataServices;
using UniversityTimetable.Shared.Models;
using UniversityTimetable.Shared.QueryParameters;

namespace UniversityTimetable.Infrastructure.DataSelectors.MultipleItemSelectors
{
    public class AuditorySelector : IMultipleItemSelector<Auditory, AuditoryParameters>
    {
        public IQueryable<Auditory> SelectData(DbSet<Auditory> data, AuditoryParameters parameters)
            => data.Search(a => a.Name, parameters.AuditoryName).Search(a => a.Building, parameters.BuildingName);
    }
}
