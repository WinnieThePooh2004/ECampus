using ECampus.Infrastructure;
using ECampus.Infrastructure.DataSelectors.MultipleItemSelectors;
using ECampus.Shared.Models;
using ECampus.Shared.Models.RelationModels;
using ECampus.Shared.QueryParameters;
using ECampus.Tests.Shared.Mocks.EntityFramework;
using Microsoft.EntityFrameworkCore;

namespace ECampus.Tests.Unit.BackEnd.Infrastructure.DataServices.ParametersSelector;

public class MultipleCourseSelectorTests
{
    private readonly MultipleCourseSelector _sut = new();
    private readonly List<Course> _data;
    private readonly ApplicationDbContext _context = Substitute.For<ApplicationDbContext>();
    
    public MultipleCourseSelectorTests()
    {
        _data = new List<Course>
        {
            new()
            {
                Id = 1,
                CourseGroups = new List<CourseGroup> { new() { GroupId = 10 } },
                CourseTeachers = new List<CourseTeacher>()
            },
            new()
            {
                Id = 2,
                CourseGroups = new List<CourseGroup>(),
                CourseTeachers = new List<CourseTeacher> { new() { TeacherId = 10 } }
            },
            new()
            {
                Id = 3,
                CourseGroups = new List<CourseGroup> { new() { GroupId = 10 } },
                CourseTeachers = new List<CourseTeacher> { new() { TeacherId = 10 } }
            }
        };

        _context.Courses = new DbSetMock<Course>(_data);
    }

    [Fact]
    public async Task SelectData_ShouldIgnoreTeacherAndGroupId_WhenTheyAre0()
    {
        var result = await _sut.SelectData(_context, new CourseParameters { Name = "", TeacherId = 0, GroupId = 0 })
            .ToListAsync();

        result.Should().BeEquivalentTo(_data);
    }

    [Fact]
    public async Task SelectData_ShouldNotIgnoreGroupId_WhenItIsNot0()
    {
        var result = await _sut.SelectData(_context, new CourseParameters { Name = "", TeacherId = 0, GroupId = 10 })
            .ToListAsync();

        _data.RemoveAt(1);
        result.Should().BeEquivalentTo(_data);
    }
    
    [Fact]
    public async Task SelectData_ShouldNotIgnoreTeacherId_WhenItIsNot0()
    {
        var result = await _sut.SelectData(_context, new CourseParameters { Name = "", TeacherId = 10, GroupId = 0 })
            .ToListAsync();

        _data.RemoveAt(0);
        result.Should().BeEquivalentTo(_data);
    }
}