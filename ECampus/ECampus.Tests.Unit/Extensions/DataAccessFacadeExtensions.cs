using ECampus.Contracts.DataSelectParameters;
using ECampus.DataAccess.Contracts.DataAccess;
using ECampus.Shared.Data;
using ECampus.Tests.Shared.Mocks.EntityFramework;

namespace ECampus.Tests.Unit.Extensions;

/// <summary>
/// this extensions are valid only for objects created with Substitute.For
/// </summary>
public static class DataAccessFacadeExtensions
{
    public static void SetReturnById<TModel>(this IDataAccessFacade facade, int id, TModel model)
        where TModel : class, IModel
    {
        var mockedSet = new DbSetMock<TModel>(model).Object;
        facade.GetByParameters<TModel, PureByIdParameters<TModel>>(
                Arg.Is<PureByIdParameters<TModel>>(parameters => parameters.Id == id))
            .Returns(mockedSet);
    }
    
    public static void SetReturnNullById<TModel>(this IDataAccessFacade facade, int id)
        where TModel : class, IModel
    {
        var mockedSet = new DbSetMock<TModel>().Object;
        facade.GetByParameters<TModel, PureByIdParameters<TModel>>(
                Arg.Is<PureByIdParameters<TModel>>(parameters => parameters.Id == id))
            .Returns(mockedSet);
    }
}