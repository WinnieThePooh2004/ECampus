using ECampus.Infrastructure;
using ECampus.Infrastructure.DataSelectors.MultipleItemSelectors;
using ECampus.Shared.Models;
using ECampus.Shared.QueryParameters;
using ECampus.Tests.Shared.Mocks.EntityFramework;
using Microsoft.EntityFrameworkCore;

namespace ECampus.Tests.Unit.BackEnd.Infrastructure.DataServices.ParametersSelector;

public class MultipleGroupSelectorTests
{
    private readonly MultipleGroupSelector _sut;
    private readonly List<Group> _data;
    private readonly ApplicationDbContext _context = Substitute.For<ApplicationDbContext>();

    public MultipleGroupSelectorTests()
    {
        _sut = new MultipleGroupSelector();
        _data = new List<Group>
        {
            new() { Name = "b", DepartmentId = 10 },
            new() { Name = "ab", DepartmentId = 10 },
            new() { Name = "ab", DepartmentId = 11 }
        };
        _context.Groups = new DbSetMock<Group>(_data);
    }

    [Fact]
    public void SelectData_ShouldReturnSuitableData()
    {
        var parameters = new GroupParameters { DepartmentId = 10, Name = "a" };

        var selectedData = _sut.SelectData(_context, parameters).ToList();

        selectedData.Should().Contain(_data[1]);
        selectedData.Count.Should().Be(1);
    }
    
    [Fact]
    public void SelectData_ShouldNotFilterByFacultyId_WhenFacultyIdIs0()
    {
        var parameters = new GroupParameters { DepartmentId = 0, Name = "a" };

        var selectedData = _sut.SelectData(_context, parameters).ToList();

        selectedData.Should().Contain(_data[1]);
        selectedData.Should().Contain(_data[2]);
        selectedData.Count.Should().Be(2);
    }
}