using ECampus.Shared.Data;
using ECampus.Shared.QueryParameters;

namespace ECampus.Contracts.DataSelectParameters;

public readonly struct PureByIdParameters<TModel> : IDataSelectParameters<TModel>
    where TModel : class, IModel
{
    public readonly int Id;

    public PureByIdParameters(int id)
    {
        Id = id;
    }
}