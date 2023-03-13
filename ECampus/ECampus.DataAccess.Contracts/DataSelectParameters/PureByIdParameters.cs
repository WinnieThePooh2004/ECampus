using ECampus.Domain.Data;
using ECampus.Domain.Requests;

namespace ECampus.DataAccess.Contracts.DataSelectParameters;

public readonly struct PureByIdParameters<TModel> : IDataSelectParameters<TModel>
    where TModel : class, IEntity
{
    public readonly int Id;

    public PureByIdParameters(int id)
    {
        Id = id;
    }
}