using ECampus.DataAccess.DataSelectors.SingleItemSelectors;
using ECampus.Shared.Models;
using ECampus.Tests.Shared.DataFactories;
using ECampus.Tests.Shared.Extensions;
using ECampus.Tests.Shared.Mocks.EntityFramework;
using Microsoft.EntityFrameworkCore;

namespace ECampus.Tests.Unit.Tests.BackEnd.Infrastructure.DataServices.SingleItemSelector;

public class SingleTeacherSelectorTests : IClassFixture<TeacherFactory>
{
    private readonly SingleTeacherSelector _selector;
    private readonly List<Teacher> _data;
    private readonly DbSet<Teacher> _set;

    public SingleTeacherSelectorTests(TeacherFactory dataFactory)
    {
        _data = dataFactory.CreateMany(new Fixture(), 10);
        _set = new DbSetMock<Teacher>(_data).Object;
        _selector = new SingleTeacherSelector();
    }

    [Fact]
    public async Task SelectModel_ReturnsModel_IfModelExists_IncludeCalled()
    {
        var model = await _selector.SelectModel(_data[0].Id, _set);

        model.Should().Be(_data[0]);
        _set.Received().Include(t => t.Subjects);
    }

    [Fact]
    public async Task SelectModel_ReturnsNull_WhenObjectDoesNotExist()
    {
        var model = await _selector.SelectModel(-1, _set);
        model.Should().BeNull();
    }
}