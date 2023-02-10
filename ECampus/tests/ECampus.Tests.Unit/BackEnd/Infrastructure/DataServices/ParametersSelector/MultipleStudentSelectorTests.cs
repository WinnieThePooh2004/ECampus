using ECampus.DataAccess.DataSelectors.MultipleItemSelectors;
using ECampus.Infrastructure;
using ECampus.Shared.Models;
using ECampus.Shared.QueryParameters;
using ECampus.Tests.Shared.Mocks.EntityFramework;
using Microsoft.EntityFrameworkCore;

namespace ECampus.Tests.Unit.BackEnd.Infrastructure.DataServices.ParametersSelector;

public class MultipleStudentSelectorTests
{
    private readonly MultipleStudentSelector _sut;
    private readonly List<Student> _data;
    private readonly ApplicationDbContext _context = Substitute.For<ApplicationDbContext>();

    public MultipleStudentSelectorTests()
    {
        _sut = new MultipleStudentSelector();
        _data = new List<Student>
        {
            new() { LastName = "b", GroupId = 10 },
            new() { LastName = "ab", GroupId = 10, UserEmail = "email" },
            new() { LastName = "ab", GroupId = 11 }
        };
        _context.Students = new DbSetMock<Student>(_data);
    }

    [Fact]
    public void SelectData_ShouldReturnSuitableData()
    {
        var parameters = new StudentParameters { GroupId = 10, LastName = "a", FirstName = "" };

        var selectedData = _sut.SelectData(_context, parameters).ToList();

        selectedData.Should().Contain(_data[1]);
        selectedData.Count.Should().Be(1);
    }

    [Fact]
    public void SelectData_ShouldNotFilterByFacultyId_WhenFacultyIdIs0()
    {
        var parameters = new StudentParameters { GroupId = 0, LastName = "a", FirstName = "" };

        var selectedData = _sut.SelectData(_context, parameters).ToList();

        selectedData.Should().Contain(_data[1]);
        selectedData.Should().Contain(_data[2]);
        selectedData.Count.Should().Be(2);
    }

    [Fact]
    public void SelectData_ShouldNotSelectWithUserEmail_WhenUserIdCanBeNullIsFalse()
    {
        var parameters = new StudentParameters { GroupId = 0, LastName = "a", FirstName = "", UserIdCanBeNull = false };

        var selectedData = _sut.SelectData(_context, parameters).ToList();

        selectedData.Should().Contain(_data[2]);
        selectedData.Count.Should().Be(1);
    }
}