using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using UniversityTimetable.Infrastructure;
using UniversityTimetable.Infrastructure.Repositories;
using UniversityTimetable.Shared.Exceptions.InfrastructureExceptions;
using UniversityTimetable.Shared.Interfaces.Data;
using UniversityTimetable.Tests.Shared.DataFactories;

namespace UniversityTimetable.Tests.Unit.BackEnd.Infrastructure.BaseRepositoryTests;

public abstract class BaseRepositoryTests<TModel>
    where TModel : class, IModel, new()
{
    private readonly ApplicationDbContext _context;
    private readonly BaseRepository<TModel> _repository;
    private readonly Fixture _fixture;
    private readonly IAbstractFactory<TModel> _dataFactory;
    private readonly ISingleItemSelector<TModel> _selector;
    private readonly IDataUpdate<TModel> _update = Substitute.For<IDataUpdate<TModel>>();
    private readonly IDataCreate<TModel> _create = Substitute.For<IDataCreate<TModel>>();
    private readonly IDataDelete<TModel> _delete = Substitute.For<IDataDelete<TModel>>();
    protected BaseRepositoryTests(IAbstractFactory<TModel> dataFactory)
    {
        _dataFactory = dataFactory;
        _fixture = new Fixture();
        _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
        _context = Substitute.For<ApplicationDbContext>();
        _selector = Substitute.For<ISingleItemSelector<TModel>>();
        _repository = new BaseRepository<TModel>(_context, Substitute.For<ILogger<BaseRepository<TModel>>>(), _selector, _delete, _update, _create);
    }

    protected virtual async Task Create_AddedToDb_CreateCalled()
    {
        var model = CreateModel();
        model.Id = 0;
        
        await _repository.CreateAsync(model);

        await _create.Received(1).CreateAsync(model, _context);
    }

    protected virtual async Task Create_ShouldThrowException_WhenModelIdNot0_CreateWasNotCalled()
    {
        await new Func<Task>(() => _repository.CreateAsync(new TModel { Id = 10 })).Should()
            .ThrowAsync<InfrastructureExceptions>()
            .WithMessage("Cannot create add object to db if id != 0\nError code: 400");

        await _create.DidNotReceive().CreateAsync(Arg.Any<TModel>(), _context);
    }

    protected virtual async Task Update_UpdatedInDb_IfExistsInDb()
    {
        var updatedItem = CreateModel();

        var result = await _repository.UpdateAsync(updatedItem);

        result.Should().Be(updatedItem);
    }

    protected virtual async Task Update_ShouldThrowException_IfSaveChangeThrowsException()
    {
        // removing async here will lead to compile error
        _context.SaveChangesAsync().Returns(1).AndDoes(call => throw new Exception());

        await new Func<Task>(() => _repository.UpdateAsync(new TModel())).Should()
            .ThrowAsync<InfrastructureExceptions>();
    }

    protected virtual async Task GetById_ReturnsFromSelector_IfSelectorReturnsItem()
    {
        var item = CreateModel();
        _selector.SelectModel(1, Arg.Any<DbSet<TModel>>()).Returns(item);
        var model = await _repository.GetByIdAsync(1);

        model.Should().Be(item);
    }

    protected virtual async Task GetByIdAsync_ShouldThrowException_WhenSelectorReturnsNull()
    {
        await new Func<Task>(() => _repository.GetByIdAsync(-1)).Should().ThrowAsync<ObjectNotFoundByIdException>();
    }

    private TModel CreateModel()
    {
        return _dataFactory.CreateModel(_fixture);
    }
}