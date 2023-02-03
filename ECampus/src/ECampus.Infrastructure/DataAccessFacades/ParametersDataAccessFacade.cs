using ECampus.Contracts.DataAccess;
using ECampus.Infrastructure.Interfaces;
using ECampus.Shared.Data;
using ECampus.Shared.QueryParameters;

namespace ECampus.Infrastructure.DataAccessFacades;

public class ParametersDataAccessFacade<TModel, TParameters> : IParametersDataAccessFacade<TModel, TParameters>
    where TModel : class, IModel, new()
    where TParameters : IQueryParameters<TModel>, IQueryParameters
{
    private readonly ApplicationDbContext _context;
    private readonly IMultipleItemSelector<TModel, TParameters> _multipleItemSelector;

    public ParametersDataAccessFacade(ApplicationDbContext context,
        IMultipleItemSelector<TModel, TParameters> multipleItemSelector)
    {
        _context = context;
        _multipleItemSelector = multipleItemSelector;
    }

    public IQueryable<TModel> GetByParameters(TParameters parameters)
    {
        return _multipleItemSelector.SelectData(_context, parameters);
    }
}