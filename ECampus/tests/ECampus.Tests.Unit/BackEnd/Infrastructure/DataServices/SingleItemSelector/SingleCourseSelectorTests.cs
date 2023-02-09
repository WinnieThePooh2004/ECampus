using ECampus.DataAccess.DataSelectors.SingleItemSelectors;
using ECampus.Shared.Models;
using ECampus.Tests.Shared.Mocks.EntityFramework;
using Microsoft.EntityFrameworkCore;

namespace ECampus.Tests.Unit.BackEnd.Infrastructure.DataServices.SingleItemSelector;

public class SingleCourseSelectorTests
{
    private readonly SingleCourseSelector _selector;
    private readonly List<Course> _data;
    private readonly DbSet<Course> _set;

    public SingleCourseSelectorTests()
    {
        _data = Enumerable.Range(0, 10).Select(i => new Course { Id = i }).ToList();
        _set = new DbSetMock<Course>(_data).Object;
        _selector = new SingleCourseSelector();
    }

    [Fact]
    public async Task SelectModel_ReturnsModel_IfModelExists_IncludeCalled()
    {
        var model = await _selector.SelectModel(_data[0].Id, _set);

        model.Should().Be(_data[0]);
        _set.Received().Include(c => c.Teachers);
        _set.Received().Include(c => c.Groups);
    }

    [Fact]
    public async Task SelectModel_ReturnsNull_WhenObjectDoesNotExist()
    {
        var model = await _selector.SelectModel(-1, _set);
        model.Should().BeNull();
    }
}