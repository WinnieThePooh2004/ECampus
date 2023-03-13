using ECampus.DataAccess.DataSelectors.MultipleItemSelectors;
using ECampus.Domain.Entities;
using ECampus.Domain.Requests.CourseTask;
using ECampus.Infrastructure;
using ECampus.Tests.Shared.Mocks.EntityFramework;

namespace ECampus.Tests.Unit.Tests.BackEnd.Infrastructure.DataServices.ParametersSelector;

public class MultipleCourseTaskSelectorTests
{
    private readonly MultipleCourseTaskSelector _sut = new();
    private readonly List<CourseTask> _data;
    private readonly ApplicationDbContext _context = Substitute.For<ApplicationDbContext>();

    public MultipleCourseTaskSelectorTests()
    {
        _data = new List<CourseTask>
        {
            new() { CourseId = 10 },
            new() { CourseId = 12 }
        };

        _context.CourseTasks = new DbSetMock<CourseTask>(_data);
    }

    [Fact]
    public void Select_ShouldReturnSuitedData()
    {
        var result = _sut.SelectData(_context, new CourseTaskParameters { CourseId = 10 })
            .ToList();

        result.Count.Should().Be(1);
        result.Should().Contain(_data[0]);
    }
}