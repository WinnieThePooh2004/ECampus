using Microsoft.Extensions.Logging;
using UniversityTimetable.Infrastructure;
using UniversityTimetable.Infrastructure.Repositories;
using UniversityTimetable.Shared.Interfaces.Data;
using UniversityTimetable.Shared.Interfaces.Repositories;
using UniversityTimetable.Shared.QueryParameters;
using UniversityTimetable.Tests.Shared.DataFactories;
using UniversityTimetable.Tests.Shared.Mocks;

namespace UniversityTimetable.Tests.Unit.BackEnd.Infrastructure.ParametersRepositoryTests;

public class ParametersRepositoryTests<TModel, TParameters>
    where TModel : class, IModel, new()
    where TParameters : class, IQueryParameters<TModel>, new()
{
    private readonly IBaseRepository<TModel> _baseRepository = Substitute.For<IBaseRepository<TModel>>();
    private readonly ParametersRepository<TModel, TParameters> _repository;
    private readonly Fixture _fixture = new();
    private readonly ApplicationDbContext _context = Substitute.For<ApplicationDbContext>();
    private readonly IAbstractFactory<TModel> _factory;

    protected ParametersRepositoryTests(IAbstractFactory<TModel> factory, IMultipleItemSelector<TModel, TParameters> selector)
    {
        _factory = factory;
        _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
        _repository = new ParametersRepository<TModel, TParameters>(_context,
            Substitute.For<ILogger<ParametersRepository<TModel, TParameters>>>(), _baseRepository, selector);
    }

    protected virtual async Task Create_ReturnsFromBaseRepository_BaseRepositoryCalled()
    {
        var item = _factory.CreateModel(_fixture);
        _baseRepository.CreateAsync(item).Returns(item);

        var result = await _repository.CreateAsync(item);

        result.Should().Be(item);
        await _baseRepository.Received(1).CreateAsync(item);
    }
    
    protected virtual async Task Update_ReturnsFromBaseRepository_BaseRepositoryCalled()
    {
        var item = _factory.CreateModel(_fixture);
        _baseRepository.UpdateAsync(item).Returns(item);

        var result = await _repository.UpdateAsync(item);

        result.Should().Be(item);
        await _baseRepository.Received(1).UpdateAsync(item);
    }

    protected virtual async Task Delete_BaseRepositoryCalled()
    {
        await _repository.DeleteAsync(10);

        await _baseRepository.Received(1).DeleteAsync(10);
    }

    protected virtual async Task GetById_ReturnsFromBaseRepository_BaseRepositoryCalled()
    {
        var item = _factory.CreateModel(_fixture);
        _baseRepository.GetByIdAsync(10).Returns(item);

        var result = await _repository.GetByIdAsync(10);

        result.Should().Be(item);
        await _baseRepository.Received(1).GetByIdAsync(10);
    }

    protected virtual async Task GetByParameters_ReturnsFromDb()
    {
        var data = Enumerable.Range(0, 10).Select(i => _factory.CreateModel(_fixture)).ToList();
        var parameters = new TParameters { PageNumber = 0, PageSize = 5 };
        var set = new DbSetMock<TModel>(data);
        _context.Set<TModel>().Returns(set.Object);
        
        var result = await _repository.GetByParameters(parameters);

        result.Data.Should().BeEquivalentTo(data.OrderBy(item => item.Id).Take(parameters.PageSize).ToList(),
            opt => opt.ComparingByMembers<TModel>());
    }
}