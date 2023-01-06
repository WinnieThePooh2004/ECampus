using UniversityTimetable.Infrastructure.DataSelectors.SingleItemSelectors;
using UniversityTimetable.Shared.Models;
using UniversityTimetable.Tests.Shared.DataFactories;
using UniversityTimetable.Tests.Shared.Extensions;
using UniversityTimetable.Tests.Shared.Mocks.EntityFramework;

namespace UniversityTimetable.Tests.Unit.BackEnd.Infrastructure.DataServices.SingleItemSelector;

public class SingleItemSelectorTests
{
    private readonly DbSetMock<Auditory> _testSet;
    private readonly Fixture _fixture = new();
    private readonly SingleItemSelector<Auditory> _singleItemSelector;
    private readonly List<Auditory> _testDataSource;

    public SingleItemSelectorTests()
    {
        _testDataSource = new AuditoryFactory().CreateMany(_fixture, 10);
        _testSet = new DbSetMock<Auditory>(_testDataSource);
        _singleItemSelector = new SingleItemSelector<Auditory>();
    }

    [Fact]
    public async Task GetById_ReturnsFromSet_IfModelExists()
    {
        var item = await _singleItemSelector.SelectModel(_testDataSource[0].Id, _testSet.Object);
        item.Should().Be(_testDataSource[0]);
    }
    
    [Fact]
    public async Task GetById_ReturnsNull_IfItemDoesNotExist()
    {
        await _singleItemSelector.SelectModel(-1, _testSet.Object);
    }
}