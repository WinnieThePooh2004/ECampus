using ECampus.Contracts.DataAccess;
using ECampus.Infrastructure.Interfaces;
using ECampus.Shared.Data;
using ECampus.Shared.QueryParameters;

namespace ECampus.Infrastructure.DataAccessFacades;

public class ParametersDataAccessManager : IParametersDataAccessManager
{
    private readonly ApplicationDbContext _context;
    private readonly IServiceProvider _serviceProvider;
    private readonly Dictionary<Type, object> _createdObjects = new();

    public ParametersDataAccessManager(ApplicationDbContext context, IServiceProvider serviceProvider)
    {
        _context = context;
        _serviceProvider = serviceProvider;
    }

    public IQueryable<TModel> GetByParameters<TModel, TParameters>(TParameters parameters)
        where TModel : class, IModel
        where TParameters : IQueryParameters<TModel>
    {
        var selector = GetOrCreateSelector<TModel, TParameters>();
        return selector.SelectData(_context, parameters);
    }

    private IMultipleItemSelector<TModel, TParameters> GetOrCreateSelector<TModel, TParameters>()
        where TModel : class, IModel 
        where TParameters : IQueryParameters<TModel>
    {
        var type = typeof(IMultipleItemSelector<TModel, TParameters>);
        if (_createdObjects.TryGetValue(type, out var value))
        {
            return (IMultipleItemSelector<TModel, TParameters>)value;
        }
        
        var selector = _serviceProvider.GetService(type);
        if (selector is null)
        {
            throw new ArgumentException($"Cannot provide service for type {type}");
        }
        
        _createdObjects.Add(type, selector);
        return (IMultipleItemSelector<TModel, TParameters>)selector;
    }
}