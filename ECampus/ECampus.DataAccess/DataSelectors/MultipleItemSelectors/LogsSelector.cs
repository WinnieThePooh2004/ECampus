using ECampus.DataAccess.Interfaces;
using ECampus.Domain.Models;
using ECampus.Domain.QueryParameters;
using ECampus.Infrastructure;

namespace ECampus.DataAccess.DataSelectors.MultipleItemSelectors;

public class LogsSelector : IParametersSelector<Log, LogParameters>
{
    public IQueryable<Log> SelectData(ApplicationDbContext context, LogParameters parameters)
    {
         return context.Set<Log>()
             .Where(log => parameters.From <= log.TimeStamp &&
                           log.TimeStamp <= parameters.To);
    }
}