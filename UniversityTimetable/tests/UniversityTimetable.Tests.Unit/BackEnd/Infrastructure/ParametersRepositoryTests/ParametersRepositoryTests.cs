using UniversityTimetable.Infrastructure;
using UniversityTimetable.Infrastructure.DataAccessFacades;
using UniversityTimetable.Shared.Interfaces.Data.DataServices;
using UniversityTimetable.Shared.Interfaces.Data.Models;
using UniversityTimetable.Shared.Interfaces.DataAccess;
using UniversityTimetable.Shared.QueryParameters;
using UniversityTimetable.Tests.Shared.DataFactories;
using UniversityTimetable.Tests.Shared.Extensions;
using UniversityTimetable.Tests.Shared.Mocks;

namespace UniversityTimetable.Tests.Unit.BackEnd.Infrastructure.ParametersRepositoryTests;

public class ParametersRepositoryTests<TModel, TParameters>
    where TModel : class, IModel, new()
    where TParameters : class, IQueryParameters<TModel>, new()
{
    private readonly IBaseDataAccessFacade<TModel> _baseDataAccessFacade = Substitute.For<IBaseDataAccessFacade<TModel>>();
    private readonly ParametersDataAccessFacade<TModel, TParameters> _dataAccessFacade;
    private readonly Fixture _fixture = new();
    private readonly ApplicationDbContext _context = Substitute.For<ApplicationDbContext>();
    private readonly IAbstractFactory<TModel> _factory;

    protected ParametersRepositoryTests(IAbstractFactory<TModel> factory, IMultipleItemSelector<TModel, TParameters> selector)
    {
        _factory = factory;
        _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
        _dataAccessFacade = new ParametersDataAccessFacade<TModel, TParameters>(_context, _baseDataAccessFacade, selector);
    }

    protected virtual async Task Create_ReturnsFromBaseRepository_BaseRepositoryCalled()
    {
        var item = _factory.CreateModel(_fixture);
        _baseDataAccessFacade.CreateAsync(item).Returns(item);

        var result = await _dataAccessFacade.CreateAsync(item);

        result.Should().Be(item);
        await _baseDataAccessFacade.Received(1).CreateAsync(item);
    }
    
    protected virtual async Task Update_ReturnsFromBaseRepository_BaseRepositoryCalled()
    {
        var item = _factory.CreateModel(_fixture);
        _baseDataAccessFacade.UpdateAsync(item).Returns(item);

        var result = await _dataAccessFacade.UpdateAsync(item);

        result.Should().Be(item);
        await _baseDataAccessFacade.Received(1).UpdateAsync(item);
    }

    protected virtual async Task Delete_BaseRepositoryCalled()
    {
        await _dataAccessFacade.DeleteAsync(10);

        await _baseDataAccessFacade.Received(1).DeleteAsync(10);
    }

    protected virtual async Task GetById_ReturnsFromBaseRepository_BaseRepositoryCalled()
    {
        var item = _factory.CreateModel(_fixture);
        _baseDataAccessFacade.GetByIdAsync(10).Returns(item);

        var result = await _dataAccessFacade.GetByIdAsync(10);

        result.Should().Be(item);
        await _baseDataAccessFacade.Received(1).GetByIdAsync(10);
    }

    protected virtual async Task GetByParameters_ReturnsFromDb()
    {
        var data = _factory.CreateMany(_fixture, 10);
        var parameters = new TParameters { PageNumber = 0, PageSize = 5 };
        var set = new DbSetMock<TModel>(data);
        _context.Set<TModel>().Returns(set.Object);
        
        var result = await _dataAccessFacade.GetByParameters(parameters);

        result.Data.Should().BeEquivalentTo(data.OrderBy(item => item.Id).Take(parameters.PageSize).ToList(),
            opt => opt.ComparingByMembers<TModel>());
    }
}