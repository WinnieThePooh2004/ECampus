using Microsoft.EntityFrameworkCore;
using UniversityTimetable.Shared.Extentions;
using UniversityTimetable.Shared.Interfaces.Data;
using UniversityTimetable.Shared.Models;
using UniversityTimetable.Shared.QueryParameters;

namespace UniversityTimetable.Infrastructure.DataSelectors
{
    public class AuditorySelector : IDataSelector<Auditory, AuditoryParameters>
    {
        public IQueryable<Auditory> SelectData(DbSet<Auditory> data, AuditoryParameters parameters)
            => data.Search(a => a.Name, parameters.AuditoryName).Search(a => a.Building, parameters.BuildingName);
    }
}
