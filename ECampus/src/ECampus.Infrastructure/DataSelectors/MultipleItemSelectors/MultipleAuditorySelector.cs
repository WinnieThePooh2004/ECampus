using ECampus.Infrastructure.Interfaces;
using ECampus.Shared.Extensions;
using ECampus.Shared.Models;
using ECampus.Shared.QueryParameters;
using Microsoft.EntityFrameworkCore;

namespace ECampus.Infrastructure.DataSelectors.MultipleItemSelectors;

public class MultipleAuditorySelector : IMultipleItemSelector<Auditory, AuditoryParameters>
{
    public IQueryable<Auditory> SelectData(DbSet<Auditory> data, AuditoryParameters parameters)
        => data.Search(a => a.Name, parameters.AuditoryName)
            .Search(a => a.Building, parameters.BuildingName);
}