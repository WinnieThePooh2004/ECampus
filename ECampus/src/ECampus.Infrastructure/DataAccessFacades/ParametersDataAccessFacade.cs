using ECampus.Infrastructure.Interfaces;
using ECampus.Shared.DataContainers;
using ECampus.Shared.Extensions;
using ECampus.Shared.Interfaces.Data.Models;
using ECampus.Shared.Interfaces.DataAccess;
using ECampus.Shared.QueryParameters;
using Microsoft.EntityFrameworkCore;

namespace ECampus.Infrastructure.DataAccessFacades;

public class ParametersDataAccessFacade<TModel, TParameters> : IParametersDataAccessFacade<TModel, TParameters>
    where TModel : class, IModel, new()
    where TParameters : IQueryParameters<TModel>
{
    private readonly ApplicationDbContext _context;
    private readonly IMultipleItemSelector<TModel, TParameters> _multipleItemSelector;

    public ParametersDataAccessFacade(ApplicationDbContext context,
        IMultipleItemSelector<TModel, TParameters> multipleItemSelector)
    {
        _context = context;
        _multipleItemSelector = multipleItemSelector;
    }

    public async Task<ListWithPaginationData<TModel>> GetByParameters(TParameters parameters)
    {
        var query = _multipleItemSelector.SelectData(_context.Set<TModel>(), parameters);
        var totalCount = await query.CountAsync();
        var pagedItems = await query
            .Sort(parameters.OrderBy, parameters.SortOrder)
            .Skip((parameters.PageNumber - 1) * parameters.PageSize)
            .Take(parameters.PageSize)
            .ToListAsync();

        return new ListWithPaginationData<TModel>(pagedItems, totalCount, parameters.PageNumber, parameters.PageSize);
    }
}