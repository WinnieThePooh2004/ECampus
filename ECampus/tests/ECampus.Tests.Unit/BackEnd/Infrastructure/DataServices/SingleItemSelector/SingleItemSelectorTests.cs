using ECampus.Infrastructure.DataSelectors.SingleItemSelectors;
using ECampus.Shared.Models;
using ECampus.Tests.Shared.DataFactories;
using ECampus.Tests.Shared.Extensions;
using ECampus.Tests.Shared.Mocks.EntityFramework;
using Microsoft.EntityFrameworkCore;

namespace ECampus.Tests.Unit.BackEnd.Infrastructure.DataServices.SingleItemSelector;

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
        await _singleItemSelector.SelectModel(_testDataSource[0].Id, _testSet);
        await _testSet.Received(1).FindAsync(_testDataSource[0].Id);
    }
    
    [Fact]
    public async Task GetById_ReturnsNull_IfItemDoesNotExist()
    {
        await _singleItemSelector.SelectModel(-1, _testSet);
    }
}