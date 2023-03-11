using ECampus.DataAccess.DataSelectors.MultipleItemSelectors;
using ECampus.Infrastructure;
using ECampus.Shared.Models;
using ECampus.Shared.QueryParameters;
using ECampus.Tests.Shared.Mocks.EntityFramework;

namespace ECampus.Tests.Unit.Tests.BackEnd.Infrastructure.DataServices.ParametersSelector;

public class MultipleTeacherSelectorTests
{
    private readonly MultipleTeacherSelector _sut;
    private readonly ApplicationDbContext _context = Substitute.For<ApplicationDbContext>();
    private readonly List<Teacher> _data;

    public MultipleTeacherSelectorTests()
    {
        _sut = new MultipleTeacherSelector();
        _data = new List<Teacher>
        {
            new() { LastName = "b", DepartmentId = 10 },
            new() { LastName = "ab", DepartmentId = 10, UserEmail = "" },
            new() { LastName = "ab", DepartmentId = 11 }
        };
        _context.Teachers = new DbSetMock<Teacher>(_data);
    }

    [Fact]
    public void SelectData_ShouldReturnSuitableData()
    {
        var parameters = new TeacherParameters { DepartmentId = 10, LastName = "a", FirstName = "" };

        var selectedData = _sut.SelectData(_context, parameters).ToList();

        selectedData.Should().Contain(_data[1]);
        selectedData.Count.Should().Be(1);
    }

    [Fact]
    public void SelectData_ShouldNotFilterByFacultyId_WhenFacultyIdIs0()
    {
        var parameters = new TeacherParameters { DepartmentId = 0, LastName = "a", FirstName = "" };

        var selectedData = _sut.SelectData(_context, parameters).ToList();

        selectedData.Should().Contain(_data[1]);
        selectedData.Should().Contain(_data[2]);
        selectedData.Count.Should().Be(2);
    }

    [Fact]
    public void SelectData_ShouldNotSelectWithUserEmail_WhenUserIdCanBeNullIsFalse()
    {
        var parameters = new TeacherParameters
            { DepartmentId = 0, LastName = "a", FirstName = "", UserIdCanBeNull = false };

        var selectedData = _sut.SelectData(_context, parameters).ToList();

        selectedData.Should().Contain(_data[2]);
        selectedData.Count.Should().Be(1);
    }
}