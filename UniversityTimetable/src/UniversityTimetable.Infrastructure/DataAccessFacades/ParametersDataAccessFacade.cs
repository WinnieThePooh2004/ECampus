using Microsoft.EntityFrameworkCore;
using UniversityTimetable.Shared.QueryParameters;
using UniversityTimetable.Shared.Interfaces.DataAccess;
using UniversityTimetable.Shared.DataContainers;
using UniversityTimetable.Shared.Interfaces.Data.DataServices;
using UniversityTimetable.Shared.Interfaces.Data.Models;

namespace UniversityTimetable.Infrastructure.DataAccessFacades;

public class ParametersDataAccessFacade<TModel, TParameters> : IParametersDataAccessFacade<TModel, TParameters>
    where TModel : class, IModel, new()
    where TParameters : IQueryParameters<TModel>
{
    private readonly ApplicationDbContext _context;
    private readonly IBaseDataAccessFacade<TModel> _baseDataAccessFacade;
    private readonly IMultipleItemSelector<TModel, TParameters> _multipleItemSelector;
    public ParametersDataAccessFacade(ApplicationDbContext context,
        IBaseDataAccessFacade<TModel> baseDataAccessFacade, IMultipleItemSelector<TModel, TParameters> multipleItemSelector)
    {
        _context = context;
        _baseDataAccessFacade = baseDataAccessFacade;
        _multipleItemSelector = multipleItemSelector;
    }

    public Task<TModel> CreateAsync(TModel entity)
        => _baseDataAccessFacade.CreateAsync(entity);

    public Task DeleteAsync(int id)
        => _baseDataAccessFacade.DeleteAsync(id);

    public Task<TModel> GetByIdAsync(int id)
        => _baseDataAccessFacade.GetByIdAsync(id);

    public async Task<ListWithPaginationData<TModel>> GetByParameters(TParameters parameters)
    {
        var query = _multipleItemSelector.SelectData(_context.Set<TModel>(), parameters);
        var totalCount = await query.CountAsync();
        var pagedItems = await query
            .OrderBy(a => a.Id)
            .Skip((parameters.PageNumber - 1) * parameters.PageSize)
            .Take(parameters.PageSize)
            .ToListAsync();

        return new ListWithPaginationData<TModel>(pagedItems, totalCount, parameters.PageNumber, parameters.PageSize);
    }

    public Task<TModel> UpdateAsync(TModel entity)
        => _baseDataAccessFacade.UpdateAsync(entity);
}