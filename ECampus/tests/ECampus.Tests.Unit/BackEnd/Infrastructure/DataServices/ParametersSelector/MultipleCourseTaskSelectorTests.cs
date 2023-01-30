using ECampus.Infrastructure.DataSelectors.MultipleItemSelectors;
using ECampus.Shared.Models;
using ECampus.Shared.QueryParameters;
using ECampus.Tests.Shared.Mocks.EntityFramework;
using Microsoft.EntityFrameworkCore;

namespace ECampus.Tests.Unit.BackEnd.Infrastructure.DataServices.ParametersSelector;

public class MultipleCourseTaskSelectorTests
{
    private readonly MultipleCourseTaskSelector _sut = new();
    private readonly List<CourseTask> _data;
    private readonly DbSet<CourseTask> _dataSource;

    public MultipleCourseTaskSelectorTests()
    {
        _data = new List<CourseTask>
        {
            new() { CourseId = 10 },
            new() { CourseId = 12 }
        };

        _dataSource = new DbSetMock<CourseTask>(_data);
    }

    [Fact]
    public void Select_ShouldReturnSuitedData()
    {
        var result = _sut.SelectData(_dataSource, new CourseTaskParameters { CourseId = 10 })
            .ToList();

        result.Count.Should().Be(1);
        result.Should().Contain(_data[0]);
    }
}