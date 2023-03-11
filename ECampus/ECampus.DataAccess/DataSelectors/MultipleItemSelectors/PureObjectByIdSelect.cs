using ECampus.Contracts.DataSelectParameters;
using ECampus.DataAccess.Interfaces;
using ECampus.Domain.Data;
using ECampus.Infrastructure;

namespace ECampus.DataAccess.DataSelectors.MultipleItemSelectors;

public class PureObjectByIdSelect<TModel> : IParametersSelector<TModel, PureByIdParameters<TModel>>
    where TModel : class, IModel
{
    public IQueryable<TModel> SelectData(ApplicationDbContext context, PureByIdParameters<TModel> parameters)
    {
        return context.Set<TModel>().Where(model => model.Id == parameters.Id);
    }
}