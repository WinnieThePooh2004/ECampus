using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using UniversityTimetable.Infrastructure;
using UniversityTimetable.Infrastructure.DataAccessFacades;
using UniversityTimetable.Shared.Exceptions.InfrastructureExceptions;
using UniversityTimetable.Shared.Interfaces.Data.DataServices;
using UniversityTimetable.Shared.Interfaces.Data.Models;
using UniversityTimetable.Tests.Shared.DataFactories;

namespace UniversityTimetable.Tests.Unit.BackEnd.Infrastructure.BaseDataAccessFacadeTests;

public abstract class BaseDataAccessFacadeTests<TModel>
    where TModel : class, IModel, new()
{
    private readonly ApplicationDbContext _context;
    private readonly BaseDataAccessFacade<TModel> _dataAccessFacade;
    private readonly Fixture _fixture;
    private readonly IAbstractFactory<TModel> _dataFactory;
    private readonly ISingleItemSelector<TModel> _selector;
    private readonly IDataUpdate<TModel> _update = Substitute.For<IDataUpdate<TModel>>();
    private readonly IDataCreate<TModel> _create = Substitute.For<IDataCreate<TModel>>();
    private readonly IDataDelete<TModel> _delete = Substitute.For<IDataDelete<TModel>>();
    protected BaseDataAccessFacadeTests(IAbstractFactory<TModel> dataFactory)
    {
        _dataFactory = dataFactory;
        _fixture = new Fixture();
        _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
        _context = Substitute.For<ApplicationDbContext>();
        _selector = Substitute.For<ISingleItemSelector<TModel>>();
        _dataAccessFacade = new BaseDataAccessFacade<TModel>(_context, Substitute.For<ILogger<BaseDataAccessFacade<TModel>>>(), _selector, _delete, _update, _create);
    }

    protected virtual async Task Create_AddedToDb_CreateCalled()
    {
        var model = CreateModel();
        model.Id = 0;
        
        await _dataAccessFacade.CreateAsync(model);

        await _create.Received(1).CreateAsync(model, _context);
    }
    
    protected virtual async Task Update_UpdatedInDb_IfExistsInDb()
    {
        var updatedItem = CreateModel();

        var result = await _dataAccessFacade.UpdateAsync(updatedItem);

        result.Should().Be(updatedItem);
    }

    protected virtual async Task Update_ShouldThrowException_IfSaveChangeThrowsException()
    {
        _context.SaveChangesAsync().Returns(1).AndDoes(_ => throw new Exception());

        await new Func<Task>(() => _dataAccessFacade.UpdateAsync(new TModel())).Should()
            .ThrowAsync<InfrastructureExceptions>();
    }

    protected virtual async Task GetById_ReturnsFromSelector_IfSelectorReturnsItem()
    {
        var item = CreateModel();
        _selector.SelectModel(1, Arg.Any<DbSet<TModel>>()).Returns(item);
        var model = await _dataAccessFacade.GetByIdAsync(1);

        model.Should().Be(item);
    }

    protected virtual async Task GetByIdAsync_ShouldThrowException_WhenSelectorReturnsNull()
    {
        await new Func<Task>(() => _dataAccessFacade.GetByIdAsync(-1)).Should().ThrowAsync<ObjectNotFoundByIdException>();
    }

    private TModel CreateModel()
    {
        return _dataFactory.CreateModel(_fixture);
    }
}