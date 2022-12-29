using UniversityTimetable.Infrastructure.DataSelectors.SingleItemSelectors;
using UniversityTimetable.Shared.Interfaces.Data;
using UniversityTimetable.Shared.Interfaces.Data.Models;
using UniversityTimetable.Tests.Shared.DataFactories;
using UniversityTimetable.Tests.Shared.Mocks;
using UniversityTimetable.Tests.Shared.Extensions;

namespace UniversityTimetable.Tests.Unit.BackEnd.Infrastructure.ItemSelectorsTests;

public abstract class SingleItemSelectorTests<TModel>
    where TModel : class, IModel, new()
{
    private readonly DbSetMock<TModel> _testSet;
    private readonly Fixture _fixture = new();
    private readonly SingleItemSelector<TModel> _singleItemSelector;
    private readonly List<TModel> _testDataSource;

    protected SingleItemSelectorTests(IAbstractFactory<TModel> dataFactory)
    {
        _testDataSource = dataFactory.CreateMany(_fixture, 10);
        _testSet = new DbSetMock<TModel>(_testDataSource);
        _singleItemSelector = new SingleItemSelector<TModel>();
    }

    protected virtual async Task GetById_ReturnsFromSet_IfModelExists()
    {
        var item = await _singleItemSelector.SelectModel(_testDataSource[0].Id, _testSet.Object);
        item.Should().Be(_testDataSource[0]);
    }

    protected virtual async Task GetById_ReturnsNull_IfItemDoesNotExist()
    {
        var item = await _singleItemSelector.SelectModel(-1, _testSet.Object);
    }
}