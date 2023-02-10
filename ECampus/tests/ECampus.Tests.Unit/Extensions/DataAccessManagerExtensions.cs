using ECampus.Contracts.DataAccess;
using ECampus.Contracts.DataSelectParameters;
using ECampus.Shared.Data;
using ECampus.Tests.Shared.Mocks.EntityFramework;

namespace ECampus.Tests.Unit.Extensions;

public static class DataAccessManagerExtensions
{
    public static void SetReturnById<TModel>(this IDataAccessManager manager, int id, TModel model)
        where TModel : class, IModel
    {
        var mockedSet = new DbSetMock<TModel>(model).Object;
        manager.GetByParameters<TModel, PureByIdParameters<TModel>>(
                Arg.Is<PureByIdParameters<TModel>>(parameters => parameters.Id == id))
            .Returns(mockedSet);
    }
    
    public static void SetReturnNullById<TModel>(this IDataAccessManager manager, int id)
        where TModel : class, IModel
    {
        var mockedSet = new DbSetMock<TModel>().Object;
        manager.GetByParameters<TModel, PureByIdParameters<TModel>>(
                Arg.Is<PureByIdParameters<TModel>>(parameters => parameters.Id == id))
            .Returns(mockedSet);
    }
}