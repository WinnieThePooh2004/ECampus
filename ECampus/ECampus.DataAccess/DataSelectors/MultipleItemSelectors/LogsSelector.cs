using ECampus.DataAccess.Interfaces;
using ECampus.Domain.Entities;
using ECampus.Domain.Requests.Log;
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