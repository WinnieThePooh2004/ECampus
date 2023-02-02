using ECampus.Infrastructure;
using ECampus.Infrastructure.DataSelectors.MultipleItemSelectors;
using ECampus.Shared.Models;
using ECampus.Shared.QueryParameters;
using ECampus.Tests.Shared.Mocks.EntityFramework;
using Microsoft.EntityFrameworkCore;

namespace ECampus.Tests.Unit.BackEnd.Infrastructure.DataServices.ParametersSelector;

public class MultipleTaskSubmissionSelectorTests
{
    private readonly MultipleTaskSubmissionSelector _sut = new();
    private readonly List<TaskSubmission> _data;
    private readonly ApplicationDbContext _context = Substitute.For<ApplicationDbContext>();

    public MultipleTaskSubmissionSelectorTests()
    {
        _data = new List<TaskSubmission>
        {
            new() { CourseTaskId = 10 },
            new() { CourseTaskId = 12 }
        };

        _context.TaskSubmissions = new DbSetMock<TaskSubmission>(_data);
    }

    [Fact]
    public async Task Select_ShouldReturnSuitedData()
    {
        var result = await _sut.SelectData(_context, new TaskSubmissionParameters { CourseTaskId = 10 })
            .ToListAsync();

        result.Count.Should().Be(1);
        result.Should().Contain(_data[0]);
    }
}