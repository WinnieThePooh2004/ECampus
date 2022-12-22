using System.Data;
using Microsoft.Extensions.Logging;
using UniversityTimetable.Infrastructure;
using UniversityTimetable.Infrastructure.Repositories;
using UniversityTimetable.Shared.Exceptions.InfrastructureExceptions;
using UniversityTimetable.Shared.Interfaces.Data;
using UniversityTimetable.Tests.Shared.DataFactories;
using UniversityTimetable.Tests.Shared.Mocks;

namespace UniversityTimetable.Tests.Unit.BackEnd.Infrastructure.BaseRepositoryTests;

public abstract class BaseRepositoryTests<TModel>
    where TModel : class, IModel, new()
{
    private readonly ApplicationDbContext _context;
    private readonly BaseRepository<TModel> _repository;
    private readonly List<TModel> _dataSource;
    private readonly Fixture _fixture;
    private readonly IAbstractFactory<TModel> _dataFactory;

    protected BaseRepositoryTests(IAbstractFactory<TModel> dataFactory)
    {
        _dataFactory = dataFactory;
        _fixture = new Fixture();
        _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
        _dataSource = Enumerable.Range(0, 5).Select(i => CreateModel()).ToList();
        _context = Substitute.For<ApplicationDbContext>();
        var dataAccess = new DbSetMock<TModel>(_dataSource);
        _context.Set<TModel>().Returns(dataAccess.Object);

        _repository = new BaseRepository<TModel>(_context, Substitute.For<ILogger<BaseRepository<TModel>>>());
    }

    protected virtual async Task Create_AddedToDb()
    {
        var model = CreateModel();
        model.Id = 0;
        _context.Add(model).Returns(_ =>
        {
            _dataSource.Add(model);
            return _context.Entry(model);
        });
        
        await _repository.CreateAsync(model);

        _dataSource.Should().Contain(model);
        _context.Received(1).Add(model);
    }

    protected virtual async Task Create_ShouldThrowException_WhenModelIdNot0()
    {
        await new Func<Task>(() => _repository.CreateAsync(new TModel { Id = 10 })).Should()
            .ThrowAsync<InfrastructureExceptions>()
            .WithMessage("Cannot create add object to db if id != 0\nError code: 400");
    }

    protected virtual async Task Update_UpdatedInDb_IfExistsInDb()
    {
        var updatedItem = CreateModel();
        _context.Update(updatedItem).Returns(_ =>
        {
            _dataSource[0] = updatedItem;
            return _context.Entry(updatedItem);
        });

        var result = await _repository.UpdateAsync(updatedItem);

        result.Should().Be(updatedItem);
    }

    protected virtual async Task Update_ShouldThrowException_IfSaveChangeThrowsException()
    {
        // removing async here will lead to compile error
        _context.SaveChangesAsync().Returns(async _ => throw new DBConcurrencyException());

        await new Func<Task>(() => _repository.UpdateAsync(new TModel())).Should()
            .ThrowAsync<ObjectNotFoundByIdException>();
    }

    protected virtual async Task GetById_ReturnsFromDb_IfExistsInDb()
    {
        var model = await _repository.GetByIdAsync(_dataSource[0].Id);

        model.Should().Be(_dataSource[0]);
    }

    protected virtual async Task GetByIdAsync_ShouldThrowException_WhenSetReturnsNull()
    {
        await new Func<Task>(() => _repository.GetByIdAsync(-1)).Should().ThrowAsync<ObjectNotFoundByIdException>();
    }

    private TModel CreateModel()
    {
        return _dataFactory.CreateModel(_fixture);
    }
}