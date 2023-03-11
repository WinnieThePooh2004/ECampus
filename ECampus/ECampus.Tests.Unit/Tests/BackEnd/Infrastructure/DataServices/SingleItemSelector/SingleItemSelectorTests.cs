using ECampus.DataAccess.DataSelectors.SingleItemSelectors;
using ECampus.Domain.Entities;
using ECampus.Tests.Shared.DataFactories;
using ECampus.Tests.Shared.Extensions;
using ECampus.Tests.Shared.Mocks.EntityFramework;
using Microsoft.EntityFrameworkCore;

namespace ECampus.Tests.Unit.Tests.BackEnd.Infrastructure.DataServices.SingleItemSelector;

public class SingleItemSelectorTests
{
    private readonly DbSet<Auditory> _testSet;
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
        var result = await _singleItemSelector.SelectModel(_testDataSource[0].Id, _testSet);

        result.Should().Be(_testDataSource[0]);
    }
    
    [Fact]
    public async Task GetById_ReturnsNull_IfItemDoesNotExist()
    {
        var result = await _singleItemSelector.SelectModel(-1, _testSet);

        result.Should().BeNull();
    }
}