using ECampus.DataAccess.DataSelectors.SingleItemSelectors;
using ECampus.Shared.Models;
using ECampus.Tests.Shared.DataFactories;
using ECampus.Tests.Shared.Extensions;
using ECampus.Tests.Shared.Mocks.EntityFramework;
using Microsoft.EntityFrameworkCore;

namespace ECampus.Tests.Unit.Tests.BackEnd.Infrastructure.DataServices.SingleItemSelector;

public class SingleUserSelectorTests : IClassFixture<UserFactory>
{
    private readonly SingleUserSelector _selector;
    private readonly List<User> _data;
    private readonly DbSet<User> _set;

    public SingleUserSelectorTests(UserFactory dataFactory)
    {
        _data = dataFactory.CreateMany(new Fixture(), 10);
        _set = new DbSetMock<User>(_data).Object;
        _selector = new SingleUserSelector();
    }

    [Fact]
    public async Task SelectModel_ReturnsModel_IfModelExists_IncludeCalled()
    {
        var model = await _selector.SelectModel(_data[0].Id, _set);

        model.Should().Be(_data[0]);
        _set.Received().Include(t => t.SavedAuditories);
        _set.Received().Include(t => t.SavedGroups);
        _set.Received().Include(t => t.SavedTeachers);
    }

    [Fact]
    public async Task SelectModel_ReturnsNull_WhenObjectDoesNotExist()
    {
        var model = await _selector.SelectModel(-1, _set);
        model.Should().BeNull();
    }
}