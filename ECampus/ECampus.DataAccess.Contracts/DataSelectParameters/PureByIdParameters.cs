using ECampus.Domain.Data;
using ECampus.Domain.QueryParameters;

namespace ECampus.DataAccess.Contracts.DataSelectParameters;

public readonly struct PureByIdParameters<TModel> : IDataSelectParameters<TModel>
    where TModel : class, IModel
{
    public readonly int Id;

    public PureByIdParameters(int id)
    {
        Id = id;
    }
}