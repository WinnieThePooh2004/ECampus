using ECampus.Contracts.DataAccess;
using ECampus.Core.Extensions;
using ECampus.Infrastructure.Interfaces;
using ECampus.Shared.Data;
using ECampus.Shared.QueryParameters;

namespace ECampus.Infrastructure.DataAccessFacades;

public class ParametersDataAccessManager : IParametersDataAccessManager
{
    private readonly ApplicationDbContext _context;
    private readonly IServiceProvider _serviceProvider;

    public ParametersDataAccessManager(ApplicationDbContext context, IServiceProvider serviceProvider)
    {
        _context = context;
        _serviceProvider = serviceProvider;
    }

    public IQueryable<TModel> GetByParameters<TModel, TParameters>(TParameters parameters)
        where TModel : class, IModel
        where TParameters : IDataSelectParameters<TModel>
    {
        var selector = _serviceProvider.GetServiceOfType<IMultipleItemSelector<TModel, TParameters>>();
        return selector.SelectData(_context, parameters);
    }
}